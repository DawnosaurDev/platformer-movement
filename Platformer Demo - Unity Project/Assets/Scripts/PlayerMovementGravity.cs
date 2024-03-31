using UnityEngine;

// Handles PlayerMovement Gravity
partial class PlayerMovement
{
  private bool HoldingDown{
    get{
      return RB.velocity.y < 0 && _moveInput.y < 0;
    }
  }

  private bool IsFalling{
    get{
      return RB.velocity.y < 0;
    }
  }

  private float DownHoldGravityScale{
    get{
      // Much higher gravity if holding down
      return Data.gravityScale * Data.fastFallGravityMult;
    }
  }

  private float JumpCutGravityScale{
    get{
      // Higher gravity if jump button released
      return Data.gravityScale * Data.jumpCutGravityMult;
    }
  }

  private float JumpApexGravityScale{
    get{
      return Data.gravityScale * Data.jumpHangGravityMult;
    }
  }

  private float FallingGravityScale{
    get{
      // Higher gravity if falling
      return Data.gravityScale * Data.fallGravityMult;
    }
  }

  private float GravityScale{
    get{
      if(IsSliding)
        return 0;

      else if(HoldingDown)
        return DownHoldGravityScale;

      else if(_isJumpCut)
        return JumpCutGravityScale;

      else if(AtJumpApex)
        return JumpApexGravityScale;

      else if(IsFalling)
        return FallingGravityScale;

      // Default gravity if standing on a
      // platform or moving upwards
      else
        return Data.gravityScale;
    }
  }

  private bool ShouldCapVelocity{
    get{
      return HoldingDown || _isJumpCut || IsFalling;
    }
  }

  private float VelocityCap{
    get{
      if(HoldingDown)
        return Data.maxFastFallSpeed;

      else if(_isJumpCut)
        return Data.maxFallSpeed;

      else if(IsFalling)
        return Data.maxFallSpeed;

      // We'll never get here:
      return 0;
    }
  }

  ///

  void UpdateGravity(){
    // No gravity when dashing (returns to normal
    //  once initial dashAttack phase over)
    if(_isDashAttacking){
      SetGravityScale(0);
      return;
    }

    SetGravityScale(GravityScale);
    if(ShouldCapVelocity)
      CapVelocity(VelocityCap);
  }

  private void SetGravityScale(float scale){
    RB.gravityScale = scale;
  }

  private void CapVelocity(float maxSpeed){
    // Cap maximum fall speed, so when falling over large distances
    // we don't accelerate to insanely high speeds
    RB.velocity = new Vector2(
      RB.velocity.x,
      Mathf.Max(RB.velocity.y, -maxSpeed)
    );
  }
}
