using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
	private PlayerStateMachine player;
	public PlayerState CurrentState { get; private set; }

	public void Initialize(PlayerStateMachine player, PlayerState startingState)
	{
		CurrentState = startingState;
		this.player = player;
		this.player.CurrentState = CurrentState.ToString();
	}

	public void ChangeState(PlayerState newState)
	{
		if (CurrentState.ExitingState)
			return;

		CurrentState.Exit();
		CurrentState = newState;
		player.CurrentState = CurrentState.ToString();
		CurrentState.Enter();
	}
}
