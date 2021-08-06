using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBasic : MonoBehaviour
{
	[Header("References")]
	private Rigidbody2D rb;

	[Header("Movement")]
	public float moveSpeed;
	private float moveInput;

	[Header("Jump")]
	public float jumpForce;
	private bool isGrounded;
	private bool isJumping;

	[Header("Checks")]
	public Transform groundCheckPoint;
	public Vector2 groundCheckSize;
	[Space(10)]
	public LayerMask groundLayer;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		moveInput = Input.GetAxisRaw("Horizontal");

		isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);

		if (rb.velocity.y < 0)
		{
			isJumping = false;
		}

		if (Input.GetKey(KeyCode.C))
		{
			if (isGrounded)
				Jump();
		}
	}

	private void FixedUpdate()
	{
		rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
	}

	private void Jump()
	{
		rb.velocity = new Vector2(rb.velocity.x, jumpForce);
		isJumping = true;
	}
}