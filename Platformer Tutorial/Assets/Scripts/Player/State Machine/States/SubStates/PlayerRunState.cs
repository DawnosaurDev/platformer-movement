using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundedState
{
	public PlayerRunState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data)
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

		if(InputHandler.instance.MoveInput.x == 0)
		{
			stateMachine.ChangeState(player.IdleState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Run(1);
	}
}
