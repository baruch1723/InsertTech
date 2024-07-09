/*
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class Player : MonoBehaviour
    {
        private UserController _userController;
        private ParachuteController _parachuteController;
        private bool _isParachuting;
        private Vector2 _move;
        private Vector2 _look;
        private bool _jump;
        private bool _crouch;

        private void Start()
        {
            _userController = GetComponent<UserController>();
            _parachuteController = GetComponent<ParachuteController>();
        }

        private void FixedUpdate()
        {
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
            else
            {
                _userController.HandleInput(_move);
                _jump = false;
            }
        }

        public void OnMove(InputValue value)
        {
            _move = value.Get<Vector2>();
        }
    }
}
*/
