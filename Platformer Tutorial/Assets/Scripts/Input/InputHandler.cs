using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
	public static InputHandler instance;

	private Controls controls;

	[Header("Input Values")]
	public Action<InputArgs> OnJumpPressed;
	public Action<InputArgs> OnJumpReleased;
	public Action<InputArgs> OnDash;

	public Vector2 MoveInput { get; private set; }
	public float ClimbInput { get; private set; }

	private void Awake()
	{
		#region Singleton
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
		#endregion

		controls = new Controls();

		#region Assign Inputs
		controls.Player.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
		controls.Player.Movement.canceled += ctx => MoveInput = Vector2.zero;

		controls.Player.Jump.performed += ctx => OnJumpPressed(new InputArgs { context = ctx });
		controls.Player.JumpUp.performed += ctx => OnJumpReleased(new InputArgs { context = ctx });
		controls.Player.Dash.performed += ctx => OnDash(new InputArgs { context = ctx });

		controls.Player.Climb.performed += ctx => ClimbInput = ctx.ReadValue<float>();
		controls.Player.Climb.canceled += ctx => ClimbInput = 0;
		#endregion
	}

	#region Events
	public class InputArgs
	{
		public InputAction.CallbackContext context;
	}
	#endregion

	#region OnEnable/OnDisable
	private void OnEnable()
	{
		controls.Enable();
	}

	private void OnDisable()
	{
		controls.Disable();
	}
	#endregion
}

