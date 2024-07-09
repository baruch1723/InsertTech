using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class UserController : MonoBehaviour
    {
        private LocomotionController _character;
        private ParachuteController _parachuteController;
        private UserCameraController _userCameraController;
        private bool _isParachuting;

        private Transform _camera;
        private Vector3 _cameraForward;
        private Vector2 _look;
        private Vector2 _move;
        
        private bool _jump;
        
        [SerializeField] float _rotationPower = 3f;
        
        public GameObject FollowTransform;

        private void Start()
        {
            _camera = Camera.main.transform;
            _character = GetComponent<LocomotionController>();
            _parachuteController = GetComponent<ParachuteController>();
            _userCameraController = GetComponent<UserCameraController>();
        }

        private void Update()
        {
            if (!_jump)
            {
                _jump = Input.GetButtonDown("Jump");
            }
            CameraOrientation();
            
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (!_isParachuting)
                {
                    _isParachuting = _parachuteController.TryDeployParachute();
                }
                else
                {
                    _parachuteController.CloseParachute();
                    _isParachuting = false;
                }
            }

            if (_isParachuting)
            {
                _parachuteController.HandleInput(_move);
            }
        }

        private void CameraOrientation()
        {
            FollowTransform.transform.rotation *= Quaternion.AngleAxis(_look.x * _rotationPower, Vector3.up);
            FollowTransform.transform.rotation *= Quaternion.AngleAxis(_look.y * _rotationPower, Vector3.right);

            var angles = FollowTransform.transform.localEulerAngles;
            angles.z = 0;

            var angle = FollowTransform.transform.localEulerAngles.x;

            if (angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if (angle < 180 && angle > 40)
            {
                angles.x = 40;
            }

            FollowTransform.transform.localEulerAngles = angles;
            transform.rotation = Quaternion.Euler(0, FollowTransform.transform.rotation.eulerAngles.y, 0);
            FollowTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

        }

        private void FixedUpdate()
        {
            if(!_isParachuting)
            {
                Movement();
            }
        }

        private void Movement()
        {
            var crouch = Input.GetKey(KeyCode.C);
            _cameraForward = Vector3.Scale(_camera.forward, new Vector3(1, 0, 1)).normalized;
            var move = _move.y * _cameraForward + _move.x * _camera.right;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                move *= 0.5f;
            }

            _character.Move(move, crouch, _jump);
            _jump = false;
        }
        
        public void OnLook(InputValue value)
        {
            _look = value.Get<Vector2>();
        }
        
        public void OnMove(InputValue value)
        {
            _move = value.Get<Vector2>();
        }
    }
}