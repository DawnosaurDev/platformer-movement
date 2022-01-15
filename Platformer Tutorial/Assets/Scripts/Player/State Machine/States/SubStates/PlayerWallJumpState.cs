using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerUsingAbilityState
{
	private int jumpDir;

	public PlayerWallJumpState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data)
	{
	}

	public override void Enter()
	{
		base.Enter();

		jumpDir = player.LastOnWallRightTime > 0 ? -1 : 1;
		player.WallJump(jumpDir);
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
		else if (player.LastOnGroundTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.IdleState);
		}
		else if(player.LastPressedJumpTime > 0 && ((player.LastOnWallRightTime > 0 && jumpDir == 1) || (player.LastOnWallLeftTime > 0 && jumpDir == -1)))
        {
			player.StateMachine.ChangeState(player.WallJumpState);
		}
		else if (Time.time - startTime > data.wallJumpTime) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.InAirState);
		}

		if ((InputHandler.instance.MoveInput.x != 0))
			player.CheckDirectionToFace(InputHandler.instance.MoveInput.x > 0);
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Drag(data.dragAmount);
		player.Run(data.wallJumpRunLerp);
	}

	public bool CanJumpCut()
	{
		if (player.StateMachine.CurrentState == this && player.RB.velocity.y > 0) //if the player is jumping and still moving up
			return true;
		else
			return false;
	}
}
