using UnityEngine;

namespace Controllers
{
    public class ParachuteController : MonoBehaviour
    {
        [SerializeField] private GameObject _parachuteModel;
        [SerializeField] private float descentSpeed = 10.0f;
        [SerializeField] private float horizontalSpeed = 20.0f;
        [SerializeField] private float deployVelocity = 3f;

        private Rigidbody rb;
        public bool isParachuteDeployed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnDisable()
        {
            CloseParachute();
        }
        
        /*private void Update()
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
        }*/

        /*private void Update()
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

            //mabye merge or work with gengeral input
            if (isParachuteDeployed)
            {
                ControlDescent();
            }
        }*/
        
        public void HandleInput(Vector2 movement)
        {
            Debug.Log("asdasd");
            //mabye merge or work with gengeral input
            if (isParachuteDeployed)
            {
                ControlDescent(movement);
            }
        }

        //get referemce tp parachu target and create once
        public void DeployParachute()
        {
            isParachuteDeployed = true;
            //rb.velocity = Vector3.zero;
            _parachuteModel.SetActive(true);
            rb.drag = 15.0f;
        }

        //Cjamge input 
        private void ControlDescent(Vector2 move)
        {
            rb.velocity = new Vector3(rb.velocity.x, -descentSpeed, rb.velocity.z);
            
            var moveHorizontal = move.x;
            var moveVertical = move.y;
            
            var moveDirection = (transform.forward * moveVertical + transform.right * moveHorizontal).normalized;
            var movement = moveDirection * horizontalSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isParachuteDeployed)
            {
                CloseParachute();
            }
        }
        
        public bool TryDeployParachute()
        {
            var heightEnough = false; 
            
            if(Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out var hitInfo))
            {
                var distance = hitInfo.distance;
                heightEnough = distance > 5;
            }

            if (isParachuteDeployed || !(rb.velocity.magnitude > deployVelocity) || !heightEnough) return false;
            
            DeployParachute();
            
            return true;
        }

        public void CloseParachute()
        {
            isParachuteDeployed = false;
            rb.drag = 0.0f;
            _parachuteModel.SetActive(false);
        }
    }
}