/*
 * Original created by @DawnosaurDev at youtube.com/c/DawnosaurStudios
 * Thanks so much for checking this out and I hope you find it helpful!
 * If you have any further queries, questions or feedback feel free to reach out on my twitter or leave a comment on youtube :D
 * Feel free to use this in your own games, and I'd love to see anything you make!
 *
 * Refactored by @DevNullProd at https://devnullprod.com/
 */

using UnityEngine;

partial class PlayerMovement : MonoBehaviour
{
  // Scriptable object which holds all the
  // player's movement parameters.
  public PlayerData Data;

  #region COMPONENTS
  public Rigidbody2D RB { get; private set; }

  // Script to handle all player animations,
  public PlayerAnimator AnimHandler { get; private set; }
  #endregion

  /////////////////////////////////////////////////////

  #region STATE PARAMETERS
  // Player state properties
  public bool IsFacingRight { get; private set; }
  public bool IsJumping { get; private set; }
  public bool IsWallJumping { get; private set; }
  public bool IsDashing { get; private set; }
  public bool IsSliding { get; private set; }

  // Player timers
  public float LastOnGroundTime { get; private set; }
  public float LastOnWallTime { get; private set; }
  public float LastOnWallRightTime { get; private set; }
  public float LastOnWallLeftTime { get; private set; }

  // Jump
  private bool _isJumpCut;
  private bool _isJumpFalling;

  // Wall Jump
  private float _wallJumpStartTime;
  private int _lastWallJumpDir;

  // Dash
  private float _dashStartTime;
  private int _dashesLeft;
  private bool _dashRefilling;
  private Vector2 _lastDashDir;
  private bool _isDashAttacking;
  #endregion

  /////////////////////////////////////////////////////

  #region INPUT PARAMETERS
  private Vector2 _moveInput;

  public float LastPressedJumpTime { get; private set; }
  public float LastPressedDashTime { get; private set; }
  #endregion

  /////////////////////////////////////////////////////

  #region CHECK PARAMETERS
  // Set all of these up in the inspector
  [Header("Checks")] 

  [SerializeField]
  private Transform _groundCheckPoint;

  // Size of groundCheck depends on the size
  // of your character. Generally you want them
  // slightly small than width (for ground)
  // and height (for the wall check)
  [SerializeField]
  private Vector2 _groundCheckSize =
    new Vector2(0.49f, 0.03f);

  [Space(5)]
  [SerializeField]
  private Transform _frontWallCheckPoint;

  [SerializeField]
  private Transform _backWallCheckPoint;

  [SerializeField]
  private Vector2 _wallCheckSize =
    new Vector2(0.5f, 1f);
  #endregion

  /////////////////////////////////////////////////////

  #region LAYERS & TAGS
  [Header("Layers & Tags")]

  [SerializeField]
  private LayerMask _groundLayer;
  #endregion

  /////////////////////////////////////////////////////

  private void Awake(){
    RB = GetComponent<Rigidbody2D>();
    AnimHandler = GetComponent<PlayerAnimator>();
  }

  private void Start(){
    SetGravityScale(Data.gravityScale);
    IsFacingRight = true;
  }

  private void Update(){
    UpdateTimers();
    UpdateInput();
    UpdateCollisions();
    UpdateJumpChecks();
    UpdateDashChecks();
    UpdateSlideChecks();
    UpdateGravity();
  }

  private void FixedUpdate(){
    FixedUpdateRun();
    FixedUpdateSlide();
  }

  /////////////////////////////////////////////////////

  #region EDITOR METHODS
  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
    Gizmos.color = Color.blue;
    Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
    Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
  }
  #endregion
}
