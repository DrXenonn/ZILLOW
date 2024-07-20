using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private Rigidbody Rb;
    [SerializeField] private Transform Cam;
    [SerializeField] private Transform PlayerFeet;
    [SerializeField] private BoolVariable IsGrounded;
    [SerializeField] private FloatVariable MovementSpeed;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;
    [Header("Settings")]
    [SerializeField] private float DefaultMovementSpeed;
    [SerializeField] private float MinFOV, MaxFOV;
    [SerializeField] private float LerpSpeed;
    [SerializeField] private float SprintMultiplier;
    [SerializeField] private float JumpForce;
    [SerializeField] private float GroundCheckRadius;
    [SerializeField] private float JumpCooldown;
    
    private Vector2 _moveInput;
    private bool _isSprinting;
    private bool _canStrafe;
    private float _targetFOV;
    private float _currentFOV;
    private float _timeSinceLatestJump;

    private void Start()
    {
        MovementSpeed.Value = DefaultMovementSpeed;
        IsGrounded.Value = false;
        _currentFOV = VirtualCamera.m_Lens.FieldOfView;
        _targetFOV = _currentFOV;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (!IsGrounded.Value) return;
        if (_timeSinceLatestJump < JumpCooldown) return;
        Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        _timeSinceLatestJump = 0;
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (ctx.performed)
        {
            if (!_isSprinting)
            {
                MovementSpeed.Value *= SprintMultiplier;
                _isSprinting = true;
            }
        }

        if (!ctx.canceled) return;
        if (!_isSprinting) return;
        MovementSpeed.Value /= SprintMultiplier;
        _isSprinting = false;
    }
    
    private void UpdateCameraFOV()
    {
        _targetFOV = _isSprinting && _moveInput != Vector2.zero ? MaxFOV : MinFOV;
        
        _currentFOV = Mathf.Lerp(_currentFOV, _targetFOV, LerpSpeed * Time.deltaTime);
        VirtualCamera.m_Lens.FieldOfView = _currentFOV;
    }

    private void Update()
    {
        IsGrounded.Value = Physics.CheckSphere(PlayerFeet.position, GroundCheckRadius, GroundLayer);
        _canStrafe = Physics.CheckSphere(PlayerFeet.position, GroundCheckRadius * 3.25f, GroundLayer);
        UpdateCameraFOV();

        _timeSinceLatestJump += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        var move = Cam.transform.forward * _moveInput.y + Cam.transform.right * _moveInput.x;
        move.y = 0;
        var speed = MovementSpeed.Value;
        var newSpeed = _canStrafe ? speed : speed / 3;
        Rb.AddForce(move.normalized * newSpeed, ForceMode.VelocityChange);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(PlayerFeet.position, GroundCheckRadius);
    }
}