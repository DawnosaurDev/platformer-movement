using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("References")]
	private Rigidbody2D rb;

	[Header("Movement")]
	public float moveSpeed;
	public float acceleration;
	public float decceleration;
	public float velPower;
	[Space(10)]
	public float frictionAmount;

	[Header("Jump")]
	public float jumpForce;
	[Range(0,1)]
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

	[Header("Checks")]
	public Transform groundCheckPoint;
	public Vector2 groundCheckSize;
	[Space(10)]
	public LayerMask groundLayer;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		InputHandler.instance.OnJumpPressed += args => OnJump(args);
		InputHandler.instance.OnJumpReleased += args => OnJumpUp(args);

		gravityScale = rb.gravityScale;
	}

	private void Update()
	{
		#region Checks
		if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer)) //checks if set box overlaps with ground
		{
			lastGroundedTime = jumpCoyoteTime; //if so sets the lastGrounded to coyoteTime
		}

		if(rb.velocity.y < 0)
		{
			isJumping = false;
		}
		#endregion

		#region Jump
		if (lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping) //checks if was last grounded within coyoteTime and that jump has been pressed within bufferTime
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
		float targetSpeed = InputHandler.instance.MoveInput * moveSpeed; //calculate the direction we want to move in and our desired velocity
		float speedDif = targetSpeed - rb.velocity.x; //calculate difference between current velocity and desired velocity
		float accelRate = (Mathf.Abs(targetSpeed) > 0.01f)? acceleration : decceleration;
		float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif); //applies acceleration to speed difference, the raises to a set power so acceleration increases with higher speeds, finally multiplies by sign to reapply direction

		rb.AddForce(movement * Vector2.right); //applies force force to rigidbody, multiplying by Vector2.right so that it only affects X axis 
		#endregion

		#region Friction
		if (lastGroundedTime > 0 && Mathf.Abs(InputHandler.instance.MoveInput) < 0.01f)
		{
			float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
			amount *= Mathf.Sign(rb.velocity.x);
			rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
		}
		#endregion

		#region Jump Gravity
		if (rb.velocity.y < 0)
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
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		isJumping = true;
	}

	public void OnJump(InputHandler.InputArgs args)
	{
		lastJumpTime = jumpBufferTime;
	}

	public void OnJumpUp(InputHandler.InputArgs args)
	{
		if(rb.velocity.y > 0 && isJumping)
		{
			rb.AddForce(Vector2.down * rb.velocity.y * jumpCutMultiplier, ForceMode2D.Impulse);
		}
	}
}
