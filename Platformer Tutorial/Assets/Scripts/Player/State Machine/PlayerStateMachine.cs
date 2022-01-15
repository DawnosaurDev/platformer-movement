///
///Created by @DAWNOSAURDEV
///Thanks so much for checking this out and I hope you find it helpful! 
///If you have any further queries, questions or feedback feel free to reach out on my twitter or leave a comment on youtube :D
///

using UnityEngine; //The core theory behind this way of doing could be applied to other engines (can't comment too much though on this though)
using System.Collections;

public class PlayerStateMachine : MonoBehaviour
{
	[SerializeField] private PlayerData data;

    #region STATE MACHINE
    public StateMachine StateMachine { get; private set; }
	public PlayerIdleState IdleState { get; private set; }
	public PlayerRunState RunState { get; private set; }
	public PlayerJumpState JumpState { get; private set; }
	public PlayerInAirState InAirState { get; private set; }
	public PlayerWallSlideState WallSlideState { get; private set; }
	public PlayerWallJumpState WallJumpState { get; private set; }
	public PlayerDashState DashState { get; private set; }

	[ReadOnly] public string CurrentState;
    #endregion

    #region COMPONENTS
    public Rigidbody2D RB { get; private set; }
	#endregion

	#region STATE PARAMETERS
	public bool IsFacingRight { get; private set; }
	public float LastOnGroundTime { get; private set; }
	public float LastOnWallTime { get; private set; }
	public float LastOnWallRightTime { get; private set; }
	public float LastOnWallLeftTime { get; private set; }
    #endregion

	#region INPUT PARAMETERS
    public float LastPressedJumpTime { get; private set; }
	public float LastPressedDashTime { get; private set; }
	#endregion

	#region CHECK PARAMETERS
	[Header("Checks")] 
	[SerializeField] private Transform _groundCheckPoint;
	[SerializeField] private Vector2 _groundCheckSize;
	[Space(5)]
	[SerializeField] private Transform _frontWallCheckPoint;
	[SerializeField] private Transform _backWallCheckPoint;
	[SerializeField] private Vector2 _wallCheckSize;
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
	#endregion

	private void Awake()
	{
		#region STATE MACHINE
		StateMachine = new StateMachine();

		IdleState = new PlayerIdleState(this, StateMachine, data);
		RunState = new PlayerRunState(this, StateMachine, data);
		JumpState = new PlayerJumpState(this, StateMachine, data);
		InAirState = new PlayerInAirState(this, StateMachine, data);
		WallSlideState = new PlayerWallSlideState(this, StateMachine, data);
		WallJumpState = new PlayerWallJumpState(this, StateMachine, data);
		DashState = new PlayerDashState(this, StateMachine, data);
		#endregion

		#region COMPONENTS
		RB = GetComponent<Rigidbody2D>();
		#endregion
	}

	private void Start()
	{
		InputHandler.instance.OnJumpPressed += args => OnJump(args);
		InputHandler.instance.OnJumpReleased += args => OnJumpUp(args);
		InputHandler.instance.OnDash += args => OnDash(args);

		StateMachine.Initialize(this, IdleState);

		SetGravityScale(data.gravityScale);
		IsFacingRight = true;
	}

	private void Update()
	{
		StateMachine.CurrentState.LogicUpdate();

		#region CHECKS
		LastOnGroundTime -= Time.deltaTime;
		LastOnWallTime -= Time.deltaTime;
		LastOnWallRightTime -= Time.deltaTime;
		LastOnWallLeftTime -= Time.deltaTime;

		LastPressedJumpTime -= Time.deltaTime;
		LastPressedDashTime -= Time.deltaTime;

		//Ground Check
		if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
			LastOnGroundTime = data.coyoteTime; //if so sets the lastGrounded to coyoteTime

		//Right Wall Check
		if ((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
				|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight))
			LastOnWallRightTime = data.coyoteTime;

		//Right Wall Check
		if ((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
			|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight))
			LastOnWallLeftTime = data.coyoteTime;

		LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
		//Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
		#endregion
	}

	private void FixedUpdate()
	{
		StateMachine.CurrentState.PhysicsUpdate();
	}

	#region INPUT CALLBACKS
	//These functions are called when an even is triggered in my InputHandler. You could call these methods through a if(Input.GetKeyDown) in Update
	public void OnJump(InputHandler.InputArgs args)
	{
		LastPressedJumpTime = data.jumpBufferTime;
	}

	public void OnJumpUp(InputHandler.InputArgs args)
	{
		if (JumpState.CanJumpCut() || WallJumpState.CanJumpCut())
			JumpCut();
	}

	public void OnDash(InputHandler.InputArgs args)
	{
		LastPressedDashTime = data.dashBufferTime;
	}
	#endregion

	#region MOVEMENT METHODS

	public void SetGravityScale (float scale)
	{
		RB.gravityScale = scale;
	}

	public void Drag(float amount)
	{
		Vector2 force = amount * RB.velocity.normalized;
		force.x = Mathf.Min(Mathf.Abs(RB.velocity.x), Mathf.Abs(force.x)); //ensures we only slow the player down, if the player is going really slowly we just apply a force stopping them
		force.y = Mathf.Min(Mathf.Abs(RB.velocity.y), Mathf.Abs(force.y));
		force.x *= Mathf.Sign(RB.velocity.x); //finds direction to apply force
		force.y *= Mathf.Sign(RB.velocity.y);

		RB.AddForce(-force, ForceMode2D.Impulse); //applies force against movement direction
	}

	public void Run(float lerpAmount)
	{
		float targetSpeed = InputHandler.instance.MoveInput.x * data.runMaxSpeed; //calculate the direction we want to move in and our desired velocity
		float speedDif = targetSpeed - RB.velocity.x; //calculate difference between current velocity and desired velocity

		#region Acceleration Rate
		float accelRate;

		//gets an acceleration value based on if we are accelerating (includes turning) or trying to stop (decelerating). As well as applying a multiplier if we're air borne
		if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel : data.runDeccel;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel * data.accelInAir : data.runDeccel * data.deccelInAir;

		//if we want to run but are already going faster than max run speed
		if (((RB.velocity.x > targetSpeed && targetSpeed > 0.01f) || (RB.velocity.x < targetSpeed && targetSpeed < -0.01f)) && data.doKeepRunMomentum)
		{
			accelRate = 0; //prevent any deceleration from happening, or in other words conserve are current momentum
		}
		#endregion

		#region Velocity Power
		float velPower;
		if (Mathf.Abs(targetSpeed) < 0.01f)
		{
			velPower = data.stopPower;
		}
		else if (Mathf.Abs(RB.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(RB.velocity.x)))
		{
			velPower = data.turnPower;
		}
		else
		{
			velPower = data.accelPower;
		}
		#endregion

		//applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction
		float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
		movement = Mathf.Lerp(RB.velocity.x, movement, lerpAmount);

		RB.AddForce(movement * Vector2.right); //applies force force to rigidbody, multiplying by Vector2.right so that it only affects X axis 

		if (InputHandler.instance.MoveInput.x != 0)
			CheckDirectionToFace(InputHandler.instance.MoveInput.x > 0);
	}

	private void Turn()
	{
		Vector3 scale = transform.localScale; //stores scale and flips x axis, "flipping" the entire gameObject around. (could rotate the player instead)
		scale.x *= -1;
		transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
	}

	public void Jump()
	{
		//ensures we can't call a jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;

		#region Perform Jump
		float adjustedJumpForce = data.jumpForce;
		if (RB.velocity.y < 0)
			adjustedJumpForce -= RB.velocity.y;

		RB.AddForce(Vector2.up * adjustedJumpForce, ForceMode2D.Impulse);
		#endregion
	}

	public void WallJump(int dir)
	{
		//ensures we can't call a jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;
		LastOnWallRightTime = 0;
		LastOnWallLeftTime = 0;

		#region Perform Wall Jump
		Vector2 force = new Vector2(data.wallJumpForce.x, data.wallJumpForce.y);
		force.x *= dir; //apply force in opposite direction of wall

		if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
			force.x -= RB.velocity.x;

		if (RB.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
			force.y -= RB.velocity.y;

		RB.AddForce(force, ForceMode2D.Impulse);
        #endregion
    }

    private void JumpCut()
	{
		//applies force downward when the jump button is released. Allowing the player to control jump height
		RB.AddForce(Vector2.down * RB.velocity.y * (1 - data.jumpCutMultiplier), ForceMode2D.Impulse);
	}

	public void Slide()
	{
		//works the same as the Run but only in the y-axis
		float targetSpeed = 0;
		float speedDif = targetSpeed - RB.velocity.y;

		float movement = Mathf.Pow(Mathf.Abs(speedDif) * data.slideAccel, data.slidePower) * Mathf.Sign(speedDif);
		RB.AddForce(movement * Vector2.up, ForceMode2D.Force);
	}

	public void Dash(Vector2 dir)
	{
		LastOnGroundTime = 0;
		LastPressedDashTime = 0;

		RB.velocity = dir.normalized * data.dashSpeed;

		SetGravityScale(0);
	}

	#endregion

	#region CHECKS

	public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}

	#endregion
}