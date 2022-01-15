using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasic : MonoBehaviour
{
	[SerializeField] private PlayerData data;

	#region COMPONENTS
	private Rigidbody2D rb;
	#endregion

	#region STATE PARAMETERS
	private bool _isFacingRight;
	private bool _isJumping;
	private bool _isWallJumping;
	private bool _isDashing;

	private int _lastWallJumpDir;
	private float _wallJumpStartTime;

	private int _dashesLeft;
	private float _dashStartTime;
	private Vector2 _lastDashDir;

	private float _lastOnGroundTime;
	private float _lastOnWallTime;
	private float _lastOnWallRightTime;
	private float _lastOnWallLeftTime;
	#endregion

	#region INPUT PARAMETERS
	private Vector2 _moveInput;
	private float _lastPressedJumpTime;
	private float _lastPressedDashTime;
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

	#region Layers & Tags
	[Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
    #endregion

    private void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		SetGravityScale(data.gravityScale);
    }

    private void Update()
    {
        #region INPUT HANDLER
        _moveInput.x = Input.GetAxisRaw("Horizontal");
		_moveInput.y = Input.GetAxisRaw("Vertical");

		if(Input.GetKeyDown(KeyCode.C))
        {
			_lastPressedJumpTime = data.jumpBufferTime;
        }

		if (Input.GetKeyDown(KeyCode.X))
		{
			_lastPressedDashTime = data.dashBufferTime;
		}
		#endregion

		#region TIMERS
		_lastOnGroundTime -= Time.deltaTime;
		_lastOnWallTime -= Time.deltaTime;
		_lastOnWallRightTime -= Time.deltaTime;
		_lastOnWallLeftTime -= Time.deltaTime;

		_lastPressedJumpTime -= Time.deltaTime;
		_lastPressedDashTime -= Time.deltaTime;
		#endregion

		#region GENERAL CHECKS
		if (_moveInput.x != 0)
			CheckDirectionToFace(_moveInput.x > 0);
		#endregion

		#region PHYSICS CHECKS
		//Ground Check
		if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
			_lastOnGroundTime = data.coyoteTime; //if so sets the lastGrounded to coyoteTime

		//Right Wall Check
		if ((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && _isFacingRight)
				|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !_isFacingRight))
			_lastOnWallRightTime = data.coyoteTime;

		//Right Wall Check
		if ((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !_isFacingRight)
			|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && _isFacingRight))
			_lastOnWallLeftTime = data.coyoteTime;

		_lastOnWallTime = Mathf.Max(_lastOnWallLeftTime, _lastOnWallRightTime);
		//Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
		#endregion

		#region JUMP CHECKS
		if (_isJumping && rb.velocity.y < 0)
			_isJumping = false;

		if (_isWallJumping && rb.velocity.y < 0)
			_isWallJumping = false;

		if (_lastPressedJumpTime > 0 && !_isDashing)
        {
			if(_lastOnGroundTime > 0)
            {
				_isJumping = true;
				_isWallJumping = false;
				Jump();
            }
			else if(_lastOnWallTime > 0)
            {
				_isJumping = false;
				_isWallJumping = true;
				WallJump((_lastOnWallRightTime > 0) ? -1 : 1);
            }
        }
        #endregion
    }

    private void FixedUpdate()
    {
		if(_isWallJumping)
			Run(0);
		else
			Run(1);
	}

    #region MOVEMENT METHODS
    public void SetGravityScale(float scale)
	{
		rb.gravityScale = scale;
	}

	private void Run(float lerpAmount)
	{
		float speedX = data.runMaxSpeed * _moveInput.x;
		if (lerpAmount < 1) lerpAmount *= 0.2f;
		speedX = Mathf.Lerp(rb.velocity.x, speedX, lerpAmount);

		rb.velocity = new Vector2(speedX, rb.velocity.y); //set player velocity
	}

	private void Turn()
	{
		Vector3 scale = transform.localScale; //stores scale and flips x axis, "flipping" the entire gameObject around. (could rotate the player instead)
		scale.x *= -1;
		transform.localScale = scale;

		_isFacingRight = !_isFacingRight; //flip bool
	}

	private void Jump()
	{
		//ensures we can't call a jump multiple times from one press
		_lastPressedJumpTime = 0;
		_lastOnGroundTime = 0;

		rb.velocity = new Vector2(rb.velocity.x, data.jumpForce);
	}

	private void WallJump(int dir)
	{
		//ensures we can't call a jump multiple times from one press
		_lastPressedJumpTime = 0;
		_lastOnGroundTime = 0;
		_lastOnWallRightTime = 0;
		_lastOnWallLeftTime = 0;

		Vector2 force = new Vector2(data.wallJumpForce.x, data.wallJumpForce.y);
		force.x *= -dir; //apply force in opposite direction of wall
		rb.velocity = force;
	}

	private void JumpCut()
	{
		//applies force downward when the jump button is released. Allowing the player to control jump height
		rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * data.jumpCutMultiplier);
	}

	private void Slide()
	{
		//works the same as the Run but only in the y-axis
		rb.velocity = new Vector2(rb.velocity.x, -data.slideAccel);
	}

	private void StartDash(Vector2 dir)
	{
		_lastOnGroundTime = 0;
		_lastPressedDashTime = 0;

		rb.velocity = dir.normalized * data.dashSpeed;

		_isDashing = true;
		SetGravityScale(0);
	}

	private void StopDash(Vector2 dir)
	{
		_isDashing = false;
		SetGravityScale(data.gravityScale);
	}
	#endregion

	#region CHECK METHODS
	public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != _isFacingRight)
			Turn();
	}
	#endregion
}
