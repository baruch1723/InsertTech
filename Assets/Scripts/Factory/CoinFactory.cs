using UnityEngine;

namespace Factory
{
    public class CoinFactory
    {
        public static GameObject CreateCoin(GameObject coinPrefab, Transform parent)
        {
            return GameObject.Instantiate(coinPrefab,parent);
        }
    }
}