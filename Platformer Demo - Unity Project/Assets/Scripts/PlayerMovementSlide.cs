using UnityEngine;

// Handles PlayerMovement Slide
partial class PlayerMovement
{
  public bool CanSlide{
    get{
      return LastOnWallTime > 0 &&
        !IsJumping &&
        !IsWallJumping &&
        !IsDashing &&
        LastOnGroundTime <= 0;
    }
  }

  private bool DoLeftSlide{
    get{
      return LastOnWallLeftTime > 0 && _moveInput.x < 0;
    }
  }

  private bool DoRightSlide{
    get{
      return LastOnWallRightTime > 0 && _moveInput.x > 0;
    }
  }

  private bool DoSlide{
    get{
      return DoLeftSlide || DoRightSlide;
    }
  }

  private float SlideForce{
    get{
      // Works the same as the Run but only in the y-axis
      // This seems to work fine, buit maybe you'll find
      // a better way to implement a slide into this system
      float speedDif = Data.slideSpeed - RB.velocity.y;  
      float movement = speedDif * Data.slideAccel;

      // Clamp the movement here to prevent any over corrections
      // (these aren't noticeable in the Run).
      // The force applied can't be greater than the (negative)
      // speedDifference * by how many times a second FixedUpdate()
      // is called.
      float clamp = Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime);
      movement = Mathf.Clamp(movement, -clamp, clamp);

      return movement;
    }
  }

  void UpdateSlideChecks(){
    IsSliding = CanSlide && DoSlide;
  }

  void FixedUpdateSlide(){
    if(IsSliding)
      Slide();
  }

  private void Slide(){
    // Remove the remaining upwards.
    // Impulse to prevent upwards sliding
    if(RB.velocity.y > 0)
      RB.AddForce(
        -RB.velocity.y * Vector2.up,
        ForceMode2D.Impulse
      );

    RB.AddForce(SlideForce * Vector2.up);
  }
}
