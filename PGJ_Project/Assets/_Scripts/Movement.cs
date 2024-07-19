using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody Rb;
    [SerializeField] private Transform Cam;
    [SerializeField] private FloatVariable MovementSpeed;
    [SerializeField] private BoolVariable IsGrounded;
    [SerializeField] private float JumpForce;
    [SerializeField] private float RaycastLength;
    private Vector2 _moveInput;

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (!IsGrounded.Value) return;
        Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

    private void Update()
    {
        IsGrounded.Value = Physics.Raycast(transform.position, Vector3.down, RaycastLength);
    }

    private void FixedUpdate()
    {
        var move = Cam.transform.forward * _moveInput.y + Cam.transform.right * _moveInput.x;
        move.y = 0;
        var speed = MovementSpeed.Value;
        var newSpeed = IsGrounded.Value ? speed : speed / 2;
        Rb.AddForce(move.normalized * newSpeed, ForceMode.VelocityChange);
    }
}