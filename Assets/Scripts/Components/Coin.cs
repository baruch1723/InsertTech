using Managers;
using UnityEngine;

namespace Components
{
    public class Coin : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LevelManager.instance.CollectCoin();
                Destroy(gameObject);
            }
        }
    }
}