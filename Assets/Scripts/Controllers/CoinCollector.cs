using Managers;
using UnityEngine;

namespace Controllers
{
    public class CoinCollector : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Coin"))
            {
                LevelManager.instance.CollectCoin();
                Destroy(other.gameObject);
            }
        }
    }
}
