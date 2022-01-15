using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
	public PlayerIdleState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data)
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

		if(InputHandler.instance.MoveInput.x != 0) //change state to runnig, when moveInput detected
		{
			stateMachine.ChangeState(player.RunState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Run(1);
	}
}
