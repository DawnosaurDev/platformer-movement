using UnityEngine;

// Handles PlayerMovement Collisions
partial class PlayerMovement
{
  bool GroundOverlap{
    get{
      return Physics2D.OverlapBox(
        _groundCheckPoint.position,
        _groundCheckSize,
        0, _groundLayer
      );
    }
  }

  bool FrontOverlapsRightWall{
    get{
      return IsFacingRight &&
        Physics2D.OverlapBox(
          _frontWallCheckPoint.position,
          _wallCheckSize,
          0, _groundLayer
        );
    }
  }

  bool BackOverlapsRightWall{
    get{
      return !IsFacingRight &&
        Physics2D.OverlapBox(
          _backWallCheckPoint.position,
          _wallCheckSize,
          0, _groundLayer
        );
    }
  }

  bool RightWallOverlap{
    get{
      return FrontOverlapsRightWall ||
             BackOverlapsRightWall;
    }
  }

  bool FrontOverlapsLeftWall{
    get{
      return !IsFacingRight &&
        Physics2D.OverlapBox(
          _frontWallCheckPoint.position,
          _wallCheckSize,
          0, _groundLayer
        );
    }
  }

  bool BackOverlapsLeftWall{
    get{
      return IsFacingRight &&
        Physics2D.OverlapBox(
          _backWallCheckPoint.position,
          _wallCheckSize,
          0, _groundLayer
        );
    }
  }

  bool LeftWallOverlap{
    get{
      return FrontOverlapsLeftWall ||
             BackOverlapsLeftWall;
    }
  }

  bool JustLanded{
    get{
      return LastOnGroundTime < -0.1f;
    }
  }

  ///

  void UpdateCollisions(){
    if(IsDashing || IsJumping) return;

    if(GroundOverlap){
      if(JustLanded)
        AnimHandler.justLanded = true;

      LastOnGroundTime = Data.coyoteTime;
    }

    if(!IsWallJumping){
      if(RightWallOverlap)
        LastOnWallRightTime = Data.coyoteTime;
      if(LeftWallOverlap)
        LastOnWallLeftTime = Data.coyoteTime;
    }

    // Two checks needed for both left and right
    // walls since whenever the play turns the wall
    // checkPoints swap sides
    LastOnWallTime = Mathf.Max(
      LastOnWallLeftTime,
      LastOnWallRightTime
    );
  }
}
