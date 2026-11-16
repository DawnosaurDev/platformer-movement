using UnityEngine;

// Handles PlayerMovement input
partial class PlayerMovement
{
  private float TargetRunSpeed(float lerpAmount){
    // Direction we want to move in and our desired velocity
    float targetSpeed = _moveInput.x * Data.runMaxSpeed;

    // Smooth changes to are direction and speed
    targetSpeed = Mathf.Lerp(
      RB.velocity.x, targetSpeed, lerpAmount
    );

    return targetSpeed;
  }

  private float BaseAcceleration(float targetSpeed){
    // Acceleration value based on if we are
    // accelerating (includes turning) or trying to decelerate (stop).
    float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ?
      Data.runAccelAmount : Data.runDeccelAmount;

    // Applying a multiplier if we're air borne.
    if(!OnGround) accelRate *= Data.accelInAir;

    return accelRate;
  }

  private bool ShouldConserveMomentum(float targetSpeed){
    // We won't slow the player down if they are moving in their
    // desired direction but at a greater speed than their maxSpeed
    bool xFaster = Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed);
    bool xSameDir = Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed);
    bool isMoving = Mathf.Abs(targetSpeed) > 0.01f;

    return Data.doConserveMomentum &&
      xFaster && xSameDir && isMoving && InAir;
  }

  ///

  void FixedUpdateRun(){
    if(!IsDashing){
      if(IsWallJumping)
        Run(Data.wallJumpRunLerp);
      else
        Run(1);

    }else if(_isDashAttacking)
      Run(Data.dashEndRunLerp);
  }

  private void Run(float lerpAmount)
  {
    float targetSpeed = TargetRunSpeed(lerpAmount);
    float accelRate = BaseAcceleration(targetSpeed);

    // Increase are acceleration and maxSpeed when at the
    // apex of their jump, makes the jump feel a bit more bouncy,
    // responsive and natural
    if(AtJumpApex){
      accelRate *= Data.jumpHangAccelerationMult;
      targetSpeed *= Data.jumpHangMaxSpeedMult;
    }

    // Prevent any deceleration from happening, or in other
    // words conserve are current momentum. You could experiment
    // with allowing for the player to slightly increase their speed
    // whilst in this "state".
    if(ShouldConserveMomentum(targetSpeed))
      accelRate = 0; 

    ///

    // Calculate difference between current velocity and desired velocity
    float speedDif = targetSpeed - RB.velocity.x;

    // Calculate force along x-axis to apply to thr player
    float movement = speedDif * accelRate;

    // Convert this to a vector and apply to rigidbody
    RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
  }
}
