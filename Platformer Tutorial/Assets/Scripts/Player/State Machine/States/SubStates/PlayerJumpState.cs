using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerUsingAbilityState
{
	public PlayerJumpState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data)
	{
	}

	public override void Enter()
	{
		base.Enter();

		player.Jump();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (player.LastPressedDashTime > 0 && player.DashState.CanDash())
		{
			player.StateMachine.ChangeState(player.DashState);
		}
		else if (player.LastPressedJumpTime > 0 && player.LastOnWallTime > 0)
		{
			player.StateMachine.ChangeState(player.WallJumpState);
		}
		else if (player.RB.velocity.y <= 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.InAirState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Drag(data.dragAmount);
		player.Run(1);
	}

	public bool CanJumpCut()
	{
		if (player.StateMachine.CurrentState == this && player.RB.velocity.y > 0) //if the player is jumping and still moving up
			return true;
		else
			return false;
	}
}
