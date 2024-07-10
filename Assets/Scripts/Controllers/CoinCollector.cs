using System;
using UnityEngine;

namespace Controllers
{
    public class CoinCollector : MonoBehaviour
    {
        public static event Action OnCollectCoin;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Coin")) return;
            
            OnCollectCoin?.Invoke();
            Destroy(other.gameObject);
        }
    }
}