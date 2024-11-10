using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    // declare reference variables
    PlayerInput _playerInput;
    CharacterController _characterController;
    Animator _animator;

    // variables to store optimized setter/getter parameter IDs
    int _isWalkingHash;
    int _isRunningHash;
    int _isFallingHash;

    // variables to store player input values
    Vector2 _currentMovementInput;
    Vector3 _currentMovement;
    Vector3 _currentRunMovement;
    Vector3 _appliedMovement;
    bool _isMovementPressed;
    bool _isRunPressed;

    // constants
    public float _rotationFactorPerFrame = 15.0f;
    public float _runMultiplier = 4.5f;
    public float _walkMultiplier = 1.5f;

    // _gravity variables
    public float _gravity = -9.8f;

    // jumping variables
    public bool _masterJump = false;
    bool _isJumpPressed = false;
    float _initialJumpVelocity;
    float _maxJumpHeight = 2.0f;
    float _maxJumpTime = 0.75f;
    bool _isJumping = false;
    bool _reqiureNewJumpPress = false;
    int _jumpCount = 0;
    int _jumpCountHash;
    int _isJumpingHash;
    Dictionary<int, float> _initalJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    Coroutine _currentJumpResetRoutine = null;

    // state variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // getters and setters
    public PlayerBaseState CurrentState { get {return _currentState; } set { _currentState = value; } }
    public Animator Animator { get {return _animator; } }
    public CharacterController CharacterController { get {return _characterController; } set {_characterController = value; } }
    public Coroutine CurrentJumpResetRoutine { get {return _currentJumpResetRoutine; } set{_currentJumpResetRoutine = value; } }
    public Dictionary<int, float> InitalJumpVelocities { get {return _initalJumpVelocities; } }
    public Dictionary<int, float> JumpGravities { get {return _jumpGravities; } }
    public int JumpCount { get {return _jumpCount;} set {_jumpCount = value; } }
    public int IsWalkingHash { get {return _isWalkingHash; } }
    public int IsRunningHash { get {return _isRunningHash; } }
    public int IsFallingHash { get {return _isFallingHash; } }
    public int JumpCountHash { get {return _jumpCountHash; } }
    public int IsJumpingHash { get {return _isJumpingHash; } }
    public bool IsMovementPressed { get {return _isMovementPressed; } }
    public bool IsRunPressed { get {return _isRunPressed; } }
    public bool ReqiureNewJumpPress { get { return _reqiureNewJumpPress; } set { _reqiureNewJumpPress = value; } }
    public bool IsJumping { set {_isJumping = value; } }
    public bool IsJumpPressed { get {return _isJumpPressed; } }
    public float Gravity { get { return _gravity; } }
    public float CurrentMovementY { get {return _currentMovement.y;} set {_currentMovement.y = value; } }
    public float AppliedMovementY { get {return _appliedMovement.y; } set {_appliedMovement.y = value; } }
    public float AppliedMovementX { get {return _appliedMovement.x; } set {_appliedMovement.x = value; } }
    public float AppliedMovementZ { get {return _appliedMovement.z; } set {_appliedMovement.z = value; } }
    public float RunMultiplier { get {return _runMultiplier; } }
    public float WalkMultiplier { get {return _walkMultiplier; } }
    public Vector2 CurrentMovementInput { get {return _currentMovementInput; } }


    void Awake(){
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        // set the parameter hash refrences
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isFallingHash = Animator.StringToHash("isFalling");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _jumpCountHash = Animator.StringToHash("jumpCount");

        // set up states
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        // set the player input callbacks
        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;

        _playerInput.CharacterControls.Run.started += OnRun;
        _playerInput.CharacterControls.Run.canceled += OnRun;

        // _playerInput.CharacterControls.Jump.started += OnJump;
        // _playerInput.CharacterControls.Jump.canceled += OnJump;

        setupJumpVariables();
    }

    void setupJumpVariables(){
        float timeToApex = _maxJumpTime / 2;
        float initialGravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;

        float secondJumpGravity = (-2 * (_maxJumpHeight + 2)) / Mathf.Pow((timeToApex * 1.25f), 2);
        float secondJumpInitalVelocity = (2 * (_maxJumpHeight + 2)) / (timeToApex * 1.25f);

        float thirdJumpGravity = (-2 * (_maxJumpHeight + 4)) / Mathf.Pow((timeToApex * 1.5f), 2);
        float thirdJumpInitalVelocity = (2 * (_maxJumpHeight + 4)) / (timeToApex * 1.5f);

        _initalJumpVelocities.Add(1, _initialJumpVelocity);
        _initalJumpVelocities.Add(2, secondJumpInitalVelocity);
        _initalJumpVelocities.Add(3, thirdJumpInitalVelocity);

        _jumpGravities.Add(0, initialGravity);
        _jumpGravities.Add(1, initialGravity);
        _jumpGravities.Add(2, secondJumpGravity);
        _jumpGravities.Add(3, thirdJumpGravity);
    }

    void Start() {
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }

    void Update() {
        handleRotation();
        _currentState.UpdateStates();
        _characterController.Move(_appliedMovement * Time.deltaTime);


    }

    void handleRotation() {
        Vector3 positionToLookAt;
        // the change in position our character should point to
        positionToLookAt.x = _currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = _currentMovement.z;
        // the current rotation of our character
        Quaternion currentRoation = transform.rotation;

        if (_isMovementPressed){
            // creates a new rotation based on where the player is currently pressing
            Quaternion targertRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRoation, targertRotation, _rotationFactorPerFrame * Time.deltaTime);
        }
    }

    // handler function to set the player input values
    void OnMovementInput(InputAction.CallbackContext context){
            _currentMovementInput = context.ReadValue<Vector2>();
            _currentMovement.x = _currentMovementInput.x * _walkMultiplier;
            _currentMovement.z = _currentMovementInput.y * _walkMultiplier;
            _currentRunMovement.x = _currentMovementInput.x * _runMultiplier;
            _currentRunMovement.z = _currentMovementInput.y * _runMultiplier;
            _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
        }

    void OnRun(InputAction.CallbackContext context){
        _isRunPressed = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context){
        _isJumpPressed = context.ReadValueAsButton();
        _reqiureNewJumpPress = false;
    }

    void OnEnable(){
        // enable the character controls action map
        _playerInput.CharacterControls.Enable();
    }

    void OnDisable(){
        // disable the character controls action map
        _playerInput.CharacterControls.Disable();
    }
}
