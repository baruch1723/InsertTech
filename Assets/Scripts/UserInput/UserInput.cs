using UnityEngine;
using UnityEngine.InputSystem;

namespace UserInput
{
    public class UserInput : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; } = Vector2.zero;
        public Vector2 LookInput { get; private set; } = Vector2.zero;

        private InputActions _input;

        public bool MoveIsPressed = false;
        private void OnEnable()
        {
            _input = new InputActions();
            _input.Ground.Enable();

            _input.Ground.Move.performed += SetMove;
            _input.Ground.Move.canceled += SetMove;

            _input.Ground.Look.performed += SetLook;
            _input.Ground.Look.canceled += SetLook;
        }

        private void OnDisable()
        {
            _input.Ground.Move.performed -= SetMove;
            _input.Ground.Move.canceled -= SetMove;

            _input.Ground.Look.performed -= SetLook;
            _input.Ground.Look.canceled -= SetLook;
            
            _input.Ground.Enable();
        }

        private void SetMove(InputAction.CallbackContext callbackContext)
        {
            MoveInput = callbackContext.ReadValue<Vector2>();
        }
        
        private void SetLook(InputAction.CallbackContext callbackContext)
        {
            LookInput = callbackContext.ReadValue<Vector2>();
        }
    }
}