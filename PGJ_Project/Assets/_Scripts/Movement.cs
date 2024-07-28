using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody Rb;
    [SerializeField] private Transform Cam;
    [SerializeField] private Transform PlayerFeet;
    [SerializeField] private BoolVariable IsGrounded;
    [SerializeField] private FloatVariable MovementSpeed;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;
    [SerializeField] private BoolVariable CanMove;
    [SerializeField] private AudioSource AudioSource;

    [Header("Settings")]
    [SerializeField] private float DefaultMovementSpeed;
    [SerializeField] private float MinFOV, MaxFOV;
    [SerializeField] private float LerpSpeed;
    [SerializeField] private float SprintMultiplier;
    [SerializeField] private float JumpForce;
    [SerializeField] private float GroundCheckRadius;
    [SerializeField] private float JumpCooldown;
    [SerializeField] private float FadeInDuration;

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
        CanMove.Value = true;
        _currentFOV = VirtualCamera.m_Lens.FieldOfView;
        _targetFOV = _currentFOV;
        AudioSource.Play();
        AudioSource.Pause();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
        HandleFootstepSound();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!CanMove.Value || !ctx.performed || !IsGrounded.Value || _timeSinceLatestJump < JumpCooldown)
            return;

        Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        _timeSinceLatestJump = 0;
        HandleFootstepSound();
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !_isSprinting)
        {
            MovementSpeed.Value *= SprintMultiplier;
            _isSprinting = true;
        }

        if (ctx.canceled && _isSprinting)
        {
            MovementSpeed.Value /= SprintMultiplier;
            _isSprinting = false;
        }
        
        AudioSource.pitch = _isSprinting ? 1.3f : 1f;
        HandleFootstepSound();
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
        _canStrafe = Physics.CheckSphere(PlayerFeet.position, GroundCheckRadius * 4.5f, GroundLayer);
        UpdateCameraFOV();

        _timeSinceLatestJump += Time.deltaTime;

        HandleFootstepSound();
    }

    private void FixedUpdate()
    {
        if (!CanMove.Value) return;

        var move = Cam.transform.forward * _moveInput.y + Cam.transform.right * _moveInput.x;
        move.y = 0;
        var speed = MovementSpeed.Value;
        var newSpeed = _canStrafe ? speed : speed / 3;
        Rb.AddForce(move.normalized * newSpeed, ForceMode.VelocityChange);
    }

    private void HandleFootstepSound()
    {
        var isMoving = _moveInput != Vector2.zero;
        if (IsGrounded.Value && isMoving)
        {
            if (!AudioSource.isPlaying)
            {
                StartCoroutine(FadeInAudio());
            }
        }
        else
        {
            if (AudioSource.isPlaying)
            {
                AudioSource.Pause();
            }
        }
    }

    private IEnumerator FadeInAudio()
    {
        const float startVolume = 0f;
        AudioSource.volume = startVolume;
        AudioSource.UnPause();

        while (AudioSource.volume < 0.15f)
        {
            AudioSource.volume += Time.deltaTime / FadeInDuration;
            yield return null;
        }

        AudioSource.volume = 0.15f;
    }
}
