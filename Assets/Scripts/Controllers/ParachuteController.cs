using System;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class ParachuteController : MonoBehaviour
    {
        public GameObject parachutePrefab;
        public float descentSpeed = 10.0f;
        public float horizontalSpeed = 20.0f;
        public float deployVelocity = 5.0f;

        private bool isParachuteDeployed = false;
        private GameObject parachuteInstance;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnDisable()
        {
            CloseParachute();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (!isParachuteDeployed && rb.velocity.magnitude > deployVelocity)
                {
                    DeployParachute();
                }
                else
                {
                    CloseParachute();
                }
            }

            if (isParachuteDeployed)
            {
                ControlDescent();
            }
        }

        public void DeployParachute()
        {
            CameraController.instance.ChangeCamera(2);
            isParachuteDeployed = true;
            rb.velocity = Vector3.zero;
            var target = GameObject.Find("ParachuteTarget");
            parachuteInstance = Instantiate(parachutePrefab, target.transform.position, Quaternion.identity);
            parachuteInstance.transform.SetParent(target.transform);
            rb.drag = 15.0f;
        }

        private void ControlDescent()
        {
            rb.velocity = new Vector3(rb.velocity.x, -descentSpeed, rb.velocity.z);
            var moveHorizontal = Input.GetAxis("Horizontal");
            var moveVertical = Input.GetAxis("Vertical");
            var moveDirection = (transform.forward * moveVertical + transform.right * moveHorizontal).normalized;
            var movement = moveDirection * horizontalSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }

        private void OnCollisionEnter(Collision collision)
        {
            LevelManager.instance.StartTimer();
            if (isParachuteDeployed)
            {
                CloseParachute();
            }

            this.enabled = false;
        }

        private void CloseParachute()
        {
            CameraController.instance.ChangeCamera(0);
            
            isParachuteDeployed = false;
            rb.drag = 0.0f;
            Destroy(parachuteInstance);
        }
    }
}