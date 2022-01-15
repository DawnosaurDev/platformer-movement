using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUsingAbilityState : PlayerState
{
	public PlayerUsingAbilityState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

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
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
