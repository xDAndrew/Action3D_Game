using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")] public float speed;
        private Vector2 _currentVelocity;
        public float jumpForce;
        public LayerMask groundLayerMask;

        [Header("Look")] public Transform cameraContainer;
        public float minXLook;
        public float maxXLook;
        public float lookSensitivity;

        private Vector2 _mouseDelta;
        private float _camCurXRot;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void LateUpdate()
        {
            CameraLook();
        }

        private void CameraLook()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                return;
            }
            
            _camCurXRot += _mouseDelta.y * lookSensitivity;
            _camCurXRot = Mathf.Clamp(_camCurXRot, minXLook, maxXLook);
            cameraContainer.localEulerAngles = new Vector3(-_camCurXRot, 0, 0);
            transform.eulerAngles += new Vector3(0, _mouseDelta.x * lookSensitivity, 0);
        }

        private void Movement()
        {
            var dir = transform.forward * _currentVelocity.y + transform.right * _currentVelocity.x;
            dir *= speed;
            dir.y = _rigidbody.linearVelocity.y;
            _rigidbody.linearVelocity = dir;
        }

        public void OnLookInput(InputAction.CallbackContext context)
        {
            _mouseDelta = context.ReadValue<Vector2>();
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            _currentVelocity = context.phase switch
            {
                InputActionPhase.Performed => context.ReadValue<Vector2>(),
                InputActionPhase.Canceled => Vector2.zero,
                _ => _currentVelocity
            };
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (context.started && IsGrounded())
            {
                _rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }

        private bool IsGrounded()
        {
            var rays = new Ray[]
            {
                new(transform.position + transform.forward * 0.2f + Vector3.up * 0.01f, Vector3.down),
                new(transform.position + -transform.forward * 0.2f + Vector3.up * 0.01f, Vector3.down),
                new(transform.position + transform.right * 0.2f + Vector3.up * 0.01f, Vector3.down),
                new(transform.position + -transform.right * 0.2f + Vector3.up * 0.01f, Vector3.down)
            };

            return rays.Any(ray => Physics.Raycast(ray, 0.2f, groundLayerMask));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + transform.forward * 0.2f, Vector3.down);
            Gizmos.DrawRay(transform.position + -transform.forward * 0.2f, Vector3.down);
            Gizmos.DrawRay(transform.position + transform.right * 0.2f, Vector3.down);
            Gizmos.DrawRay(transform.position + -transform.right * 0.2f, Vector3.down);
        }
    }
}