using Managers;
using UnityEngine;

namespace Controllers
{
    public class CoinCollector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Coin")) return;
            
            LevelManager.instance.CollectCoin();
            Destroy(other.gameObject);
        }
    }
}