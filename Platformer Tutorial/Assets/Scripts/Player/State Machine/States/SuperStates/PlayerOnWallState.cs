using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnWallState : PlayerState
{
	public PlayerOnWallState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data)
	{

	}

	public override void Enter()
	{
		base.Enter();
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
		else if (player.LastOnGroundTime > 0)
		{
			player.StateMachine.ChangeState(player.IdleState);
		}
		else if(player.LastOnWallTime <= 0)
		{
			player.StateMachine.ChangeState(player.InAirState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
