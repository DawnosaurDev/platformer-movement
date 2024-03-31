using System.Collections;
using UnityEngine;

// Handles PlayerMovement Dash
partial class PlayerMovement
{
  private bool ShouldRefillDash{
    get{
      return !IsDashing &&
        !_dashRefilling &&
        OnGround && _dashesLeft < Data.dashAmount;
    }
  }

  private bool CanDash{
    get{
      return _dashesLeft > 0 && LastPressedDashTime > 0;
    }
  }

  private bool DuringDashAttack{
    get{
      return Time.time - _dashStartTime <= Data.dashAttackTime;
    }
  }

  private bool DuringDashEnd{
    get{
      return Time.time - _dashStartTime <= Data.dashEndTime;
    }
  }

  ///

  void UpdateDashChecks(){
    if(ShouldRefillDash)
      StartCoroutine(nameof(RefillDash), 1);

    if(!CanDash) return;

    // Freeze game for split second. Adds juiciness and a bit
    // of forgiveness over directional input
    Sleep(Data.dashSleepTime);

    // If not direction pressed, dash forward
    if(_moveInput != Vector2.zero)
      _lastDashDir = _moveInput;
    else
      _lastDashDir = IsFacingRight ?
        Vector2.right : Vector2.left;

    IsDashing = true;
    IsJumping = false;
    IsWallJumping = false;
    _isJumpCut = false;

    StartCoroutine(nameof(StartDash), _lastDashDir);
  }

  //Dash Coroutine
  private IEnumerator StartDash(Vector2 dir)
  {
    // Overall this method of dashing aims to mimic Celeste,
    // if you're looking for a more physics-based approach
    // try a method similar to that used in the jump

    LastOnGroundTime = 0;
    LastPressedDashTime = 0;

    _dashStartTime = Time.time;

    _dashesLeft--;
    _isDashAttacking = true;

    SetGravityScale(0);

    // Keep the player's velocity at the dash speed during
    // the "attack" phase (in celeste the first 0.15s)
    while(DuringDashAttack){
      RB.velocity = dir.normalized * Data.dashSpeed;

      // Pauses the loop until the next frame, creating
      // something of a Update loop. This is a cleaner implementation
      // opposed to multiple timers and this coroutine approach is
      // actually what is used in Celeste :D
      yield return null;
    }

    _dashStartTime = Time.time;
    _isDashAttacking = false;

    // Begins the "end" of our dash where we return some control
    // to the player but still limit run acceleration
    // (see Update() and Run())
    SetGravityScale(Data.gravityScale);
    RB.velocity = Data.dashEndSpeed * dir.normalized;

    while(DuringDashEnd){
      yield return null;
    }

    // Dash over
    IsDashing = false;
  }

  // Short period before the player is able to dash again
  private IEnumerator RefillDash(int amount){
    // Cooldown, so we can't constantly dash along the ground,
    // again this is the implementation in Celeste.
    _dashRefilling = true;
    yield return new WaitForSeconds(Data.dashRefillTime);

    _dashRefilling = false;
    _dashesLeft = Mathf.Min(Data.dashAmount, _dashesLeft + 1);
  }
}
