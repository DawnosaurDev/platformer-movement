using UnityEngine;

[CreateAssetMenu(menuName = "Player Data")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the player
public class PlayerData : ScriptableObject
{
	[Header("Gravity")]
	[ReadOnly] public float gravityStrength; //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
	[ReadOnly] public float gravityScale; //Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).
										  //Also the value the player's rigidbody2D.gravityScale is set to.
	[Space(10)]
	public float fallGravityMult; //Multiplier to the player's gravityScale when falling.
	public float maxFallSpeed; //Maximum fall speed (terminal velocity) of the player when falling.
	[Space(10)]
	public float fastFallGravityMult; //Larger multiplier to the player's gravityScale when they are falling and a downwards input is pressed.
									  //Seen in games such as Celeste, lets the player fall extra fast if they wish.
	public float maxFastFallSpeed; //Maximum fall speed(terminal velocity) of the player when performing a faster fall.


	[Header("Run")]
	public float runMaxSpeed; //Target speed we want the player to reach.
	public float runAccelTime; //Time (approx.) time we want it to take for the player to accelerate from 0 to the runMaxSpeed.
	[ReadOnly] public float runAccel; //The actual force (multiplied with speedDiff) applied to the player.
	public float runDeccelTime; //Time (approx.) we want it to take for the player to accelerate from runMaxSpeed to 0.
	[ReadOnly] public float runDeccel; //Actual force (multiplied with speedDiff) applied to the player .
	[Space(10)]
	[Range(0.01f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
	[Range(0.01f, 1)] public float deccelInAir;

	[Header("Jump")]
	public float jumpHeight; //Height of the player's jump
	public float jumpTimeToApex; //Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.
	[ReadOnly] public float jumpForce; //The actual force applied (upwards) to the player when they jump.
	[Space(10)]

	[Header("Wall Jump")]
	public Vector2 wallJumpForce; //The actual force (this time set by us) applied to the player when wall jumping.
	[Space(5)]
	[Range(0.01f, 1f)] public float wallJumpRunLerp; //Reduces the effect of player's movement while wall jumping.
	[Range(0.01f, 1.5f)] public float wallJumpTime; //Time after wall jumping the player's movement is slowed for.

	[Header("Both Jumps")]
	public float jumpCutGravityMult; //Multiplier to increase gravity if the player releases thje jump button while still jumping
	[Range(0.1f, 1)] public float jumpHangGravityMult; //Reduces gravity while close to the apex (desired max height) of the jump
	public float jumpHangTimeThreshold; //Speeds (close to 0) where the player will experience extra "jump hang".
										//The player's velocity.y is closest to 0 around the jump's apex (think of the gradient of a parabola or quadratic function)
					


	[Header("Slide")]
	public float slideSpeed;
	public float slideAccel;

    [Header("Assists")]
	[Range(0.01f, 0.5f)] public float coyoteTime; //Grace period after falling off a platform, where you can still jump
	[Range(0.01f, 0.5f)] public float jumpInputBufferTime; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.


	[Header("Dash")]
	public int dashAmount;
	public float dashSpeed;
	public float dashSleepTime; //Duration for which the game freezes when we press dash but before we read directional input and apply a force
	[Space(5)]
	public float dashAttackTime;
	[Space(5)]
	public float dashEndTime; //Time after you finish the inital drag phase, smoothing the transition back to idle (or any standard state)
	public Vector2 dashEndSpeed; //Slows down player, makes dash feel more responsive (used in Celeste)
	[Range(0.01f, 1f)] public float dashEndRunLerp; //Slows the affect of player movement while dashing
	[Space(5)]
	public float dashRefillTime;
	[Space(5)]
	[Range(0.01f, 0.5f)] public float dashInputBufferTime;


	[Header("Other Settings")]
	public bool doKeepRunMomentum; //Player's movement will not try to decrease speed if above maxSpeed. Allows for better conservation of momentum
	public bool doTurnOnWallJump; //Player will rotate to face wall jumping direction

    private void OnValidate()
    {
		//Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
		gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
		
		//Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
		gravityScale = gravityStrength / Physics2D.gravity.y;

		//Calculate are run acceleration & deceleration forces using formula (acceleration = velocity/time)
		runAccel = (runMaxSpeed) / runAccelTime;
		runDeccel = (runMaxSpeed) / runDeccelTime;

		#region NOT USED: Alternate Way to Calculate This
		// This commented out code is (in theory) the correct way to do this. Below is what you will get by rearranging
		// the movement formula, however the simpler, more intuitive, formula above seems to produce very similar results
		// and so in the video, I chose to just show it. But if you use this "more correct" method it will result in your character
		// reaching max speed (minus the necessary threshold) at the exact time you specify (when accelerating from 0 to maxSpeed).

		/*
		float threshold = 1;
		runAccel = (runMaxSpeed * threshold) / (runAccelTime - threshold);
		runDeccel = (runMaxSpeed * threshold) / (runDeccelTime - threshold);
		*/

		// A theshold is needed since, if we take a look at the acceleration graph, you'll see that we never actually reach are max speed.
		// We only get closer and closer to it and so a theshold (1.0 works great) is needed

		#endregion

		//Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
		jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;
	}
}
