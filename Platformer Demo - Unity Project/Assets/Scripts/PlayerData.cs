using UnityEngine;

// Create a new playerData object by right clicking in the
// Project Menu then Create/Player/Player Data and drag onto
// the player
[CreateAssetMenu(menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
  [Header("Gravity")]

  // Multiplier to the player's gravityScale when falling.
  public float fallGravityMult;

  // Maximum fall speed (terminal velocity) of the player
  // when falling.
  public float maxFallSpeed;

  // Downwards force (gravity) needed for the
  // desired jumpHeight and jumpTimeToApex.
  [HideInInspector]
  public float gravityStrength;

  // Strength of the player's gravity as a multiplier
  // of gravity (set in ProjectSettings/Physics2D).
  // Player's rigidbody2D.gravityScale is set to this.
  [HideInInspector]
  public float gravityScale;

  [Space(5)]

  // Larger multiplier to the player's gravityScale when
  // they are falling and a downwards input is pressed.
  // Seen in games such as Celeste, lets the player fall
  // extra fast if they wish.
  public float fastFallGravityMult;

  // Maximum fall speed(terminal velocity) of the player
  // when performing a faster fall.
  public float maxFastFallSpeed;
  
  [Space(20)]

  /////////////////////////////////////////////////////

  [Header("Run")]

  // Target speed we want the player to reach.
  public float runMaxSpeed;

  // The speed at which the player accelerates to max speed,
  // can be set to runMaxSpeed for instant acceleration down
  // to 0 for none at all
  public float runAcceleration;

  [HideInInspector]
  // The actual force (multiplied with speedDiff) applied
  // to the player.
  public float runAccelAmount;

  // The speed at which the player decelerates from their
  // current speed, can be set to runMaxSpeed for instant
  // deceleration down to 0 for none at all
  public float runDecceleration;

  // Actual force (multiplied with speedDiff) applied to
  // the player
  [HideInInspector]
  public float runDeccelAmount;

  [Space(5)]

  // Multipliers applied to acceleration rate when airborne.
  [Range(0f, 1)]
  public float accelInAir;

  [Range(0f, 1)]
  public float deccelInAir;

  [Space(5)]
  public bool doConserveMomentum = true;

  [Space(20)]

  /////////////////////////////////////////////////////

  [Header("Jump")]

  // Height of the player's jump
  public float jumpHeight;

  // Time between applying the jump force and reaching the
  // desired jump height. These values also control the
  // player's gravity and jump force.
  public float jumpTimeToApex;

  // The actual force applied (upwards) to the player when
  // they jump.
  [HideInInspector]
  public float jumpForce;

  [Header("Both Jumps")]

  // Multiplier to increase gravity if the player releases
  // the jump button while still jumping
  public float jumpCutGravityMult;

  // Reduces gravity while close to the apex
  // (desired max height) of the jump
  [Range(0f, 1)]
  public float jumpHangGravityMult;

  // Speeds (close to 0) where the player will
  // experience extra "jump hang". The player's velocity.y
  // is closest to 0 at the jump's apex (think of the gradient
  // of a parabola or quadratic function)
  public float jumpHangTimeThreshold;

  [Space(0.5f)]

  public float jumpHangAccelerationMult; 
  public float jumpHangMaxSpeedMult;         

  /////////////////////////////////////////////////////

  [Header("Wall Jump")]

  // The actual force (this time set by us) applied to
  // the player when wall jumping.
  public Vector2 wallJumpForce;

  [Space(5)]

  // Reduces the effect of player's movement while wall jumping.
  [Range(0f, 1f)]
  public float wallJumpRunLerp;

  // Time after wall jumping the player's movement is slowed for.
  [Range(0f, 1.5f)]
  public float wallJumpTime;

  // Player will rotate to face wall jumping direction
  public bool doTurnOnWallJump;

  [Space(20)]

  /////////////////////////////////////////////////////

  [Header("Slide")]

  public float slideSpeed;
  public float slideAccel;

  /////////////////////////////////////////////////////

  [Header("Assists")]

  // Grace period after falling off a platform, where
  // you can still jump
  [Range(0.01f, 0.5f)]
  public float coyoteTime;
 
  // Grace period after pressing jump where a jump will
  // be automatically performed once the requirements
  // (eg. being grounded) are met.
  [Range(0.01f, 0.5f)]
  public float jumpInputBufferTime;

  [Space(20)]

  /////////////////////////////////////////////////////

  [Header("Dash")]

  public int dashAmount;

  public float dashSpeed;

  // Duration for which the game freezes when we press dash
  // but before we read directional input and apply a force
  public float dashSleepTime;

  [Space(5)]

  public float dashAttackTime;

  [Space(5)]

  // Time after you finish the inital drag phase, smoothing
  // the transition back to idle (or any standard state)
  public float dashEndTime;
 
  // Slows down player, makes dash feel more responsive
  // (used in Celeste)
  public Vector2 dashEndSpeed;

  // Slows the affect of player movement while dashing
  [Range(0f, 1f)]
  public float dashEndRunLerp;

  [Space(5)]

  public float dashRefillTime;

  [Space(5)]

  [Range(0.01f, 0.5f)]
  public float dashInputBufferTime;
  

  // Unity Callback, called when the inspector updates
  private void OnValidate(){
   // Calculate gravity strength using the formula
   // (gravity = 2 * jumpHeight / timeToJumpApex^2) 
   gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
    
   // Calculate the rigidbody's gravity scale
   // (ie: gravity strength relative to unity's gravity value,
   //  see project settings/Physics2D)
   gravityScale = gravityStrength / Physics2D.gravity.y;

   // Calculate are run acceleration & deceleration forces
   // using formula:
   // amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
   runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
   runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

   // Calculate jumpForce using the formula
   // (initialJumpVelocity = gravity * timeToJumpApex)
   jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

   #region Variable Ranges
   runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
   runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
   #endregion
  }
}
