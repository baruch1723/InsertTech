using Cinemachine;
using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        private CinemachineVirtualCamera _activeCamera;
        public static CameraController instance;

        public CinemachineVirtualCamera[] Cameras;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            ChangeCamera(2);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.Alpha1))
            {
                ChangeCamera(0);
            }

            if (UnityEngine.Input.GetKey(KeyCode.Alpha2))
            {
                ChangeCamera(1);
            }
            
            if (UnityEngine.Input.GetKey(KeyCode.Alpha3))
            {
                ChangeCamera(2);
            }
        }

        public void ChangeCamera(int camera)
        {
            _activeCamera = Cameras[camera];
            _activeCamera.Priority = 20;

            for (int i = 0; i < Cameras.Length; i++)
            {
                if (Cameras[i] != _activeCamera)
                {
                    Cameras[i].Priority = 10;
                }
            }
        }
    }
}