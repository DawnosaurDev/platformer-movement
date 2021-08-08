using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementImproved : MonoBehaviour
{
	[Header("References")]
	private Rigidbody2D rb;

	[Header("Movement")]
	public float moveSpeed;
	public float acceleration;
	public float deceleration;
	public float velPower;
	[Space(10)]
	private float moveInput;
	[Space(10)]
	public float frictionAmount;

	[Header("Jump")]
	public float jumpForce;
	[Range(0, 1)]
	public float jumpCutMultiplier;
	[Space(10)]
	public float jumpCoyoteTime;
	private float lastGroundedTime;
	public float jumpBufferTime;
	private float lastJumpTime;
	[Space(10)]
	public float fallGravityMultiplier;
	private float gravityScale;
	[Space(10)]
	private bool isJumping;
	private bool jumpInputReleased;

	[Header("Checks")]
	public Transform groundCheckPoint;
	public Vector2 groundCheckSize;
	[Space(10)]
	public LayerMask groundLayer;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		gravityScale = rb.gravityScale;
	}

	private void Update()
	{
		#region Inputs
		moveInput = Input.GetAxisRaw("Horizontal");

		if(Input.GetKey(KeyCode.C))
		{
			lastJumpTime = jumpBufferTime;
		}

		if(Input.GetKeyUp(KeyCode.C))
		{
			OnJumpUp();
		}
		#endregion

		#region Checks
		//checks if set box overlaps with ground
		if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer)) 
		{
			//if so sets the lastGrounded to coyoteTime
			lastGroundedTime = jumpCoyoteTime; 
		}

		if(rb.velocity.y <= 0 && jumpInputReleased)
		{
			isJumping = false;
		}
		#endregion

		#region Jump
		//checks if was last grounded within coyoteTime and that jump has been pressed within bufferTime
		if (lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping)
		{
			Jump();
		}
		#endregion

		#region Timer
		lastGroundedTime -= Time.deltaTime;
		lastJumpTime -= Time.deltaTime;
		#endregion
	}

	private void FixedUpdate()
	{
		#region Run
		//calculate the direction we want to move in and our desired velocity
		float targetSpeed = moveInput * moveSpeed;
		//calculate difference between current velocity and desired velocity
		float speedDif = targetSpeed - rb.velocity.x;
		//change acceleration rate depending on situation
		float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
		//applies acceleration to speed difference, the raises to a set power so acceleration increases with higher speeds
		//finally multiplies by sign to reapply direction
		float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

		//applies force force to rigidbody, multiplying by Vector2.right so that it only affects X axis 
		rb.AddForce(movement * Vector2.right);  
		
		#endregion

		#region Friction
		//check if we're grounded and that we are trying to stop (not pressing forwards or backwards)
		if (lastGroundedTime > 0 && Mathf.Abs(InputHandler.instance.MoveInput) < 0.01f) 
		{
			//then we use either the friction amount (~ 0.2) or our velocity
			float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
			//sets to movement direction
			amount *= Mathf.Sign(rb.velocity.x);
			//applies force against movement direction
			rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse); 		
		}
		#endregion

		#region Jump Gravity
		if (rb.velocity.y < 0 && lastGroundedTime <= 0)
		{
			rb.gravityScale = gravityScale * fallGravityMultiplier;
		}
		else
		{
			rb.gravityScale = gravityScale;
		}
		#endregion
	}

	private void Jump()
	{
		//apply force, using impluse force mode
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		lastGroundedTime = 0;
		lastJumpTime = 0;
		isJumping = true;
		jumpInputReleased = false;
	}

	public void OnJump()
	{
		lastJumpTime = jumpBufferTime;
		jumpInputReleased = false;
	}

	public void OnJumpUp()
	{
		if (rb.velocity.y > 0 && isJumping)
		{
			//reduces current y velocity by amount (0 - 1)
			rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
		}

		jumpInputReleased = true;
		lastJumpTime = 0;
	}
}
