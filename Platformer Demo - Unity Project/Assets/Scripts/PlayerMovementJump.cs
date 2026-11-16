using UnityEngine;

// Handles PlayerMovement Jump
partial class PlayerMovement
{
  private bool IsInJumpApexState{
    get{
      return IsJumping || IsWallJumping || _isJumpFalling;
    }
  }

  private bool HasJumpApexVelocity{
    get{
      return Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold;
    }
  }

  private bool AtJumpApex{
    get{
      return IsInJumpApexState && HasJumpApexVelocity;
    }
  }

  private bool ShouldEndWallJump{
    get{
      return Time.time - _wallJumpStartTime > Data.wallJumpTime;
    }
  }

  private bool JumpPressed{
    get{
      return LastPressedJumpTime > 0;
    }
  }

  private bool CanJump{
    get{
      return OnGround && !IsJumping;
    }
  }

  // Jump in opposite direction of wall
  private int WallJumpDir{
    get{
      return (LastOnWallRightTime > 0) ? -1 : 1;
    }
  }

  private bool LastWallJumpRight{
    get{
      return _lastWallJumpDir == 1;
    }
  }

  private bool LastWallJumpLeft{
    get{
      return _lastWallJumpDir == -1;
    }
  }

  private bool CanWallJump{
    get{
      return JumpPressed &&
        LastOnWallTime > 0 && LastOnGroundTime <= 0 &&
        (!IsWallJumping || WallJumpDirValid);
    }
  }

  private bool WallJumpDirValid{
    get{
      return 
         (LastOnWallRightTime > 0 && LastWallJumpRight) ||
         (LastOnWallLeftTime > 0 && LastWallJumpLeft);
    }
  }

  ///

  void UpdateJumpChecks(){
    if(IsJumping && RB.velocity.y < 0){
      IsJumping = false;
      _isJumpFalling = true;
    }

    if(IsWallJumping && ShouldEndWallJump)
      IsWallJumping = false;

    if(OnGround && !IsJumping && !IsWallJumping){
      _isJumpCut = false;
      _isJumpFalling = false;
    }

    DoJump();
  }

  void DoJump(){
    // Do not allow jump when dashing
    if(IsDashing) return;

    // Jump
    if(CanJump && JumpPressed){
      IsJumping = true;
      IsWallJumping = false;
      _isJumpCut = false;
      _isJumpFalling = false;
      Jump();

      AnimHandler.startedJumping = true;
    }

    // Wall Jump
    else if(CanWallJump && JumpPressed){
      IsWallJumping = true;
      IsJumping = false;
      _isJumpCut = false;
      _isJumpFalling = false;

      _wallJumpStartTime = Time.time;
      _lastWallJumpDir = WallJumpDir;

      WallJump(_lastWallJumpDir);
    }
  }


  private void Jump(){
    // Ensures we can't call Jump multiple times from one press
    LastPressedJumpTime = 0;
    LastOnGroundTime = 0;

    // Increase the force applied if we are falling
    // This means we'll always feel like we jump the same amount 
    // (setting the player's Y velocity to 0 beforehand will likely
    //  work the same, but I find this more elegant :D)
    float force = Data.jumpForce;
    if (RB.velocity.y < 0)
      force -= RB.velocity.y;

    RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
  }

  private void WallJump(int dir){
    // Ensures we can't call Wall Jump multiple times from one press
    LastPressedJumpTime = 0;
    LastOnGroundTime = 0;
    LastOnWallRightTime = 0;
    LastOnWallLeftTime = 0;

    // Apply force in opposite direction of wall
    Vector2 force = new Vector2(
      Data.wallJumpForce.x, Data.wallJumpForce.y
    );
    force.x *= dir;

    if(Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
      force.x -= RB.velocity.x;

    // Checks whether player is falling, if so we subtract
    // the velocity.y (counteracting force of gravity).
    // This ensures the player always reaches our desired
    // jump force or greater
    if(RB.velocity.y < 0)
      force.y -= RB.velocity.y;

    // Unlike in the run we want to use the Impulse mode.
    // The default mode will apply are force instantly
    // ignoring masss
    RB.AddForce(force, ForceMode2D.Impulse);
  }
}
