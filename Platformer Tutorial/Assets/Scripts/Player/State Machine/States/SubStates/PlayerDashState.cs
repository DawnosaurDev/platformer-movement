using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerUsingAbilityState
{
	private Vector2 dir;
	private int dashesLeft;

	private bool dashAttacking;

	public PlayerDashState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data)
	{

	}

	public override void Enter()
	{
		base.Enter();

		dashesLeft--;

		dir = Vector2.zero; //get direction to dash in
		if (InputHandler.instance.MoveInput == Vector2.zero)
			dir.x = (player.IsFacingRight) ? 1 : -1;
		else
			dir = InputHandler.instance.MoveInput;

		dashAttacking = true;
		player.Dash(dir);
	}

	public override void Exit()
	{
		base.Exit();
		player.SetGravityScale(data.gravityScale);

	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if(Time.time - startTime > data.dashAttackTime + data.dashEndTime) //dashTime over transition to another state
		{
			if (player.LastOnGroundTime > 0)
				player.StateMachine.ChangeState(player.IdleState);
			else
				player.StateMachine.ChangeState(player.InAirState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		if (Time.time - startTime > data.dashAttackTime)
		{
			//initial dash phase over, now begin slowing down and giving control back to player
			player.Drag(data.dragAmount);
			player.Run(data.dashEndRunLerp); //able to apply some run force but will be limited (~50% of normal)

			if(dashAttacking)
				StopDash();
		}
		else
		{
			player.Drag(data.dashAttackDragAmount);
		}
	}

	private void StopDash()
    {
		dashAttacking = false;
		player.SetGravityScale(data.gravityScale);

		if (dir.y > 0)
		{
			if (dir.x == 0)
				player.RB.AddForce(Vector2.down * player.RB.velocity.y * (1 - data.dashUpEndMult), ForceMode2D.Impulse);
			else
				player.RB.AddForce(Vector2.down * player.RB.velocity.y * (1 - data.dashUpEndMult) * .7f, ForceMode2D.Impulse);
		}
	}

	public bool CanDash()
	{
		return dashesLeft > 0;
	}

	public void ResetDashes()
	{
		dashesLeft = data.dashAmount;
	}
}
