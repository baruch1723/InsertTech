using System;
using System.Collections;
using Constants;
using Controllers;
using Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;

        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _coinPrefab;
        
        private GameObject _player;

        public int target;
        
        private int _currentLevel;
        private int _collectedCoins;
        private bool _timerIsRunning;
        private float _timeRemaining;
        private float _spawnRadius;

        public LayerMask groundLayer;
        
        private int _maxAttempts = 100;
        private Vector3 _playerSpawnPosition = new(0f,150f,0f);

        public static event Action<int> OnCoinCollected;
        public static event Action<int> OnLevelChanged;
        public static event Action<float> OnTimeUpdated;
        public static event Action OnLevelLose;
        public static event Action OnLevelWin;
        public static event Action OnPlayerCollectedCoin;

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
            if (_player)
            {
                Destroy(_player);
            }
            _collectedCoins = 0;

            _currentLevel = level.ID;
            target = level.Target;
            _spawnRadius = level.SpreadRadius;
            _timeRemaining = level.Time;

            OnCoinCollected?.Invoke(_collectedCoins);
            OnTimeUpdated?.Invoke(_timeRemaining);
            OnLevelChanged?.Invoke(_currentLevel);
            
            DistributeCoins(target);
            DeployPlayer();
            StartLevelTimer();
            GameManager.instance.ChangeState(GameManager.GameState.Playing);
        }
        
        private void DeployPlayer()
        {
            _player = Instantiate(_playerPrefab,_playerSpawnPosition,Quaternion.identity);
        }

        private void StartLevelTimer()
        {
            if(_timerIsRunning) return;
            
            _timerIsRunning = true;
        }

        private void Update()
        {
            if (!_timerIsRunning) return;
            
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                OnTimeUpdated?.Invoke(_timeRemaining);
            }
            else
            {
                _timerIsRunning = false;
                OnLevelLose?.Invoke();
                _timeRemaining = 0;
                GameManager.instance.ChangeState(GameManager.GameState.GameOver);
            }
        }

        public void CollectCoin()
        {
            if (!_timerIsRunning) return;

            _collectedCoins++;
            OnCoinCollected?.Invoke(_collectedCoins);
            if (_collectedCoins >= target)
            {
                LevelCompleted();
            }
        }

        private void LevelCompleted()
        {
            _timerIsRunning = false;
            if (_currentLevel >= GameManager.instance.GetAvailableLevels)
            {
                WinGame();
                return;
            }
            
            StartCoroutine(OnLevelCompleted());
        }

        private static void WinGame()
        {
            OnLevelWin?.Invoke();
            GameManager.instance.ChangeState(GameManager.GameState.Start);
            GameManager.instance.SwitchScene(Scenes.MenuScene);
        }

        private IEnumerator OnLevelCompleted()
        {
            _currentLevel++;
            OnLevelWin?.Invoke();
            yield return new WaitForSecondsRealtime (2);
            GameManager.instance.SwitchScene(Scenes.GameLevel,_currentLevel);
        }

        private void DistributeCoins(int amount)
        {
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
            
                var coin = CoinFactory.CreateCoin(_coinPrefab, parent.transform);
                coin.transform.position = spawnPosition;
                coin.SetActive(true);
            }
        }

        private Vector3 GetValidSpawnPosition(Terrain terrain)
        {
            for (int i = 0; i < _maxAttempts; i++)
            {
                var randomPosition = new Vector3(
                    Random.Range(-_spawnRadius, _spawnRadius),
                    0,
                    Random.Range(-_spawnRadius, _spawnRadius)
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