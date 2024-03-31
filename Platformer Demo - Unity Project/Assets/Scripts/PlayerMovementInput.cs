using UnityEngine;

// Handles PlayerMovement input
partial class PlayerMovement
{
  void UpdateInput(){
    _moveInput.x = Input.GetAxisRaw("Horizontal");
    _moveInput.y = Input.GetAxisRaw("Vertical");

    if (_moveInput.x != 0)
      CheckDirectionToFace(_moveInput.x > 0);

    if(Input.GetKeyDown(KeyCode.Space) ||
       Input.GetKeyDown(KeyCode.C) ||
       Input.GetKeyDown(KeyCode.J))
      OnJumpInput();

    if(Input.GetKeyUp(KeyCode.Space) ||
       Input.GetKeyUp(KeyCode.C) ||
       Input.GetKeyUp(KeyCode.J))
      OnJumpUpInput();

    if(Input.GetKeyDown(KeyCode.X) ||
       Input.GetKeyDown(KeyCode.LeftShift) ||
       Input.GetKeyDown(KeyCode.K))
      OnDashInput();
  }

  public void CheckDirectionToFace(bool isMovingRight){
    if(isMovingRight != IsFacingRight)
      Turn();
  }

  public void OnJumpInput(){
    LastPressedJumpTime = Data.jumpInputBufferTime;
  }

  public void OnJumpUpInput(){
    if (CanJumpCut() || CanWallJumpCut())
      _isJumpCut = true;
  }

  public void OnDashInput(){
    LastPressedDashTime = Data.dashInputBufferTime;
  }

  private void Turn(){
    // Flip the player along the x axis, 
    Vector3 scale = transform.localScale; 
    scale.x *= -1;
    transform.localScale = scale;

    IsFacingRight = !IsFacingRight;
  }

  private bool CanJumpCut(){
    return IsJumping && RB.velocity.y > 0;
  }

  private bool CanWallJumpCut(){
    return IsWallJumping && RB.velocity.y > 0;
  }
}
