using System;
using System.Collections;
using Controllers;
using Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;

        public GameObject PlayerPrefab;
        public GameObject coinPrefab;
        private GameObject _player;

        public int _currentLevel;
        public int target;
        public float timeRemaining = 60f;
        public float spawnRadius = 50f;
        
        public int collectedCoins = 0;
        public bool timerIsRunning;

        public LayerMask groundLayer;
        public LayerMask obstacleLayer;
        
        public int maxAttempts = 100;

        public static event Action<int> OnCoinCollected;
        public static event Action<int> OnLevelChanged;
        public static event Action<float> OnTimeUpdated;
        public static event Action OnGameLoose;
        public static event Action OnGameWin;

        
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
        
        public void StartLevel(Level level)
        {
            _currentLevel = level.ID;
            collectedCoins = 0;
            if (_player)
            {
                Destroy(_player);
            }

            DistributeCoins(level.CoinsAmount,level.SpreadRadius);
            timeRemaining = level.Time;
            target = level.Target;
            
            OnCoinCollected?.Invoke(collectedCoins);
            OnTimeUpdated?.Invoke(timeRemaining);
            OnLevelChanged?.Invoke(_currentLevel);
            
            _player = Instantiate(PlayerPrefab);
            _player.transform.position = new Vector3(0.0f,150f,0.0f);
            _player.GetComponent<ParachuteController>().enabled = true;
            _player.GetComponent<ParachuteController>().DeployParachute();
        }
        
        public void StartTimer()
        {
            if(timerIsRunning) return;
            
            timerIsRunning = true;
        }

        private void Update()
        {
            if (!timerIsRunning) return;
            
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                OnTimeUpdated?.Invoke(timeRemaining);
            }
            else
            {
                timerIsRunning = false;
                OnGameLoose?.Invoke();
                PauseManager.instance.Pause();
                timeRemaining = 0;
            }
        }

        public void CollectCoin()
        {
            if (!timerIsRunning) return;

            collectedCoins++;
            OnCoinCollected?.Invoke(collectedCoins);
            if (collectedCoins >= target)
            {
                LevelCompleted();
            }
        }

        private void LevelCompleted()
        {
            timerIsRunning = false;
            if (_currentLevel >= GameManager.instance.GetAvailableLevels)
            {
                OnGameWin?.Invoke();
                GameManager.instance.SwitchScene("MainScene",1);
                return;
            }
            
            _currentLevel++;
            StartCoroutine(OnLevelCompleted());
        }

        private IEnumerator OnLevelCompleted()
        {
            OnGameWin?.Invoke();
            yield return new WaitForSecondsRealtime (2);
            GameManager.instance.SwitchScene("Game",_currentLevel);
        }

        private void DistributeCoins(int amount, int radius)
        {
            spawnRadius = radius;
            var parent = new GameObject("Coins")
            {
                transform =
                {
                    position = Vector3.zero
                }
            };
            
            var terrain = FindObjectOfType<Terrain>();
            
            for (int i = 0; i < amount; i++)
            {
                var spawnPosition = GetValidSpawnPosition(terrain);
                if (spawnPosition == Vector3.zero) continue;
            
                var coin = CoinFactory.CreateCoin(coinPrefab, parent.transform);
                coin.transform.position = spawnPosition;
                coin.SetActive(true);
            }
        }

        private Vector3 GetValidSpawnPosition(Terrain terrain)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                var randomPosition = new Vector3(
                    Random.Range(-spawnRadius, spawnRadius),
                    0,
                    Random.Range(-spawnRadius, spawnRadius)
                );

                randomPosition.y = terrain.SampleHeight(randomPosition) + terrain.GetPosition().y + randomPosition.y + 1f;

                if (IsValidPosition(randomPosition))
                {
                    return randomPosition;
                }
            }
        
            return Vector3.zero;
        }

        private bool IsValidPosition(Vector3 position)
        {
            if (!Physics.Raycast(position + Vector3.up * 50, Vector3.down, Mathf.Infinity, groundLayer))
            {
                return false;
            }

            return !Physics.CheckSphere(position, 0.5f);
        }
    }
}