using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
	//PHYSICS
	[Header("Gravity")]
	public float gravityScale; //overrides rb.gravityScale
	public float fallGravityMult;
	public float quickFallGravityMult;

	[Header("Drag")]
	public float dragAmount; //drag in air
	public float frictionAmount; //drag on ground

	[Header("Other Physics")]
	[Range(0, 0.5f)] public float coyoteTime; //grace time to Jump after the player has fallen off a platformer


	//GROUND
	[Header("Run")]
	public float runMaxSpeed;
	public float runAccel;
	public float runDeccel;
	[Range(0, 1)] public float accelInAir;
	[Range(0, 1)] public float deccelInAir;
	[Space(5)]
	[Range(.5f, 2f)] public float accelPower;   
	[Range(.5f, 2f)] public float stopPower;
	[Range(.5f, 2f)] public float turnPower;


	//JUMP
	[Header("Jump")]
	public float jumpForce;
	[Range(0, 1)] public float jumpCutMultiplier;
	[Space(10)]
	[Range(0, 0.5f)] public float jumpBufferTime; //time after pressing the jump button where if the requirements are met a jump will be automatically performed

	[Header("Wall Jump")]
	public Vector2 wallJumpForce;
	[Space(5)]
	[Range(0f, 1f)] public float wallJumpRunLerp; //slows the affect of player movement while wall jumping
	[Range(0f, 1.5f)] public float wallJumpTime;


	//WALL
	[Header("Slide")]
	public float slideAccel;
	[Range(.5f, 2f)] public float slidePower;


	//ABILITIES
	[Header("Dash")]
	public int dashAmount;
	public float dashSpeed;
	[Space(5)]
	public float dashAttackTime;
	public float dashAttackDragAmount;
	[Space(5)]
	public float dashEndTime; //time after you finish the inital drag phase, smoothing the transition back to idle (or any standard state)
	[Range(0f, 1f)] public float dashUpEndMult; //slows down player when moving up, makes dash feel more responsive (used in Celeste)
	[Range(0f, 1f)] public float dashEndRunLerp; //slows the affect of player movement while dashing
	[Space(5)]
	[Range(0, 0.5f)] public float dashBufferTime;


	//OTHER
	[Header("Other Settings")]
	public bool doKeepRunMomentum; //player movement will not decrease speed if above maxSpeed, letting only drag do so. Allows for conservation of momentum
	public bool doTurnOnWallJump; //player will rotate to face wall jumping direction
}

//Think a setting should be renamed or added? Reach out to me @DawnosaurDev
