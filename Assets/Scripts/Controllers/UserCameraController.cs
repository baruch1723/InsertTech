using Cinemachine;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class UserCameraController : MonoBehaviour
    {
        private CinemachineVirtualCamera _activeCamera;
        [SerializeField] private CinemachineVirtualCamera[] _cameras;
        private int _currentCameraIndex;
        
        private void Start()
        {
            SetCamera(_currentCameraIndex);
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeCamera();
            }
        }

        private void ChangeCamera()
        {
            _currentCameraIndex = (_currentCameraIndex + 1) % _cameras.Length;
            SetCamera(_currentCameraIndex);
        }

        private void SetCamera(int desiredCameraIndex)
        {
            _activeCamera = _cameras[desiredCameraIndex];
            _activeCamera.Priority = 20;

            for (int i = 0; i < _cameras.Length; i++)
            {
                if (i != desiredCameraIndex)
                {
                    _cameras[i].Priority = 10;
                }
            }
        }
    }
}