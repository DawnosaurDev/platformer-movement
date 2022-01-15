using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
	protected PlayerStateMachine player;
	protected StateMachine stateMachine;
	protected PlayerData data;

	protected float startTime;
	public bool ExitingState { get; protected set; }

	public PlayerState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data)
	{
		this.player = player;
		this.stateMachine = stateMachine;
		this.data = data;
	}

	public virtual void Enter()
	{
		startTime = Time.time;
		ExitingState = false;
	}

	public virtual void Exit()
	{
		ExitingState = true;
	}

	public virtual void LogicUpdate() { }

	public virtual void PhysicsUpdate() { }
}
