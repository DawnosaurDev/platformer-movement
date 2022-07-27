/*
	Created by @DawnosaurDev at youtube.com/c/DawnosaurStudios
	Thanks so much for checking this out and I hope you find it helpful! 
	If you have any further queries, questions or feedback feel free to reach out on my twitter or leave a comment on youtube :D

	Feel free to use this in your own games, and I'd love to see anything you make!
 */

using System.Collections;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
	//Scriptable object which holds all the player's movement parameters. If you don't want to use it
	//just paste in all the parameters, though you will need to manuly change all references in this script
	public PlayerData Data;

	#region COMPONENTS
    public Rigidbody2D RB { get; private set; }
	//Script to handle all player animations, all references can be safely removed if you're importing into your own project.
	#endregion

	#region STATE PARAMETERS
	//Variables control the various actions the player can perform at any time.
	//These are fields which can are public allowing for other sctipts to read them
	//but can only be privately written to.
	public bool IsFacingRight { get; private set; }
	public float LastOnGroundTime { get; private set; }
	#endregion

	#region INPUT PARAMETERS
	private Vector2 _moveInput;
	#endregion

	#region CHECK PARAMETERS
	//Set all of these up in the inspector
	[Header("Checks")] 
	[SerializeField] private Transform _groundCheckPoint;
	[SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);;
	#endregion

	#region LAYERS & TAGS
    [Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
	#endregion

    private void Awake()
	{
		RB = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		IsFacingRight = true;
	}

	private void Update()
	{	
		#region TIMERS
        LastOnGroundTime -= Time.deltaTime;
		#endregion

		#region INPUT HANDLER
		_moveInput.x = Input.GetAxisRaw("Horizontal");

		if (_moveInput.x != 0)
			CheckDirectionToFace(_moveInput.x > 0);
		#endregion

		#region COLLISION CHECKS
		//Ground Check
		if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
			LastOnGroundTime = 0.1f;
		#endregion
    }

    private void FixedUpdate()
	{
		Run();
    }

	//MOVEMENT METHODS
    #region RUN METHODS
    private void Run()
	{
		//Calculate the direction we want to move in and our desired velocity
		float targetSpeed = _moveInput.x * Data.runMaxSpeed;
        //Calculate difference between current velocity and desired velocity
		float speedDif = targetSpeed - RB.velocity.x;

		#region Acceleration Rate
		float accelRate;

		//Gets an acceleration value based on if we are accelerating (includes turning) 
		// or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
		if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccel : Data.runDeccel;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccel * Data.accelInAir : Data.runDeccel * Data.deccelInAir;

		//If we want to run but are already going faster than max run speed/
		if (((RB.velocity.x > targetSpeed && targetSpeed > 0.01f) || (RB.velocity.x < targetSpeed && targetSpeed < -0.01f)) && Data.doKeepRunMomentum)
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			accelRate = 0; 
		}
		#endregion

		//Calculate force along x-axis to apply to thr player
		float movement = speedDif * accelRate;

		//Convert this to a vector and apply to rigidbody
		RB.AddForce(movement * Vector2.right);
	}

	private void Turn()
	{
		//stores scale and flips the player along the x axis, 
		Vector3 scale = transform.localScale; 
		scale.x *= -1;
		transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
	}
    #endregion


    #region CHECK METHODS
    public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}
    #endregion
}

// created by Dawnosaur :D