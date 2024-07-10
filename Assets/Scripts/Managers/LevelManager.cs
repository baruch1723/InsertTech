using System;
using System.Collections;
using Constants;
using Controllers;
using Factory;
using Models;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;

        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _coinPrefab;

        private GameObject _player;

        
        public int _goal;
        private int _currentLevel;
        private int _collectedCoins;
        private bool _timerIsRunning;
        private float _timeRemaining;
        private float _spawnRadius;

        private readonly Vector3 _playerSpawnPosition = new(0f,250f,0f);

        public LayerMask GroundLayer;
        public static event Action<int> OnCoinCollected;
        public static event Action<int> OnLevelChanged;
        public static event Action<float> OnTimeUpdated;
        public static event Action OnLevelLose;
        public static event Action OnLevelWin;

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
            ResetLevelState();

            _currentLevel = level.Index;
            _goal = level.Goal;
            _spawnRadius = level.SpreadRadius;
            _timeRemaining = level.Time;

            NotifyLevelStart();

            DistributeCoins(level.TotalAmount);
            DeployPlayer();
            CoinCollector.OnCollectCoin += CollectCoin;
            StartLevelTimer();
            GameManager.Instance.ChangeState(GameManager.GameState.Playing);
        }

        private void ResetLevelState()
        {
            if (_player != null)
            {
                Destroy(_player);
            }

            _collectedCoins = 0;
            _timerIsRunning = false;
        }

        private void NotifyLevelStart()
        {
            OnCoinCollected?.Invoke(_collectedCoins);
            OnTimeUpdated?.Invoke(_timeRemaining);
            OnLevelChanged?.Invoke(_currentLevel);
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
                EndLevel(false);
            }
        }

        private void EndLevel(bool won)
        {
            _timerIsRunning = false;
            _timeRemaining = 0;
            CoinCollector.OnCollectCoin -= CollectCoin;

            if (won)
            {
                OnLevelWin?.Invoke();
                if (_currentLevel >= GameManager.Instance.AvailableLevels)
                {
                    GameManager.Instance.SwitchScene(Scenes.MenuScene);
                }
                else
                {
                    StartCoroutine(ProceedToNextLevel());
                }
            }
            else
            {
                OnLevelLose?.Invoke();
                GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
                GameManager.Instance.SwitchScene(Scenes.GameLevel);
            }
        }

        private void CollectCoin()
        {
            if (!_timerIsRunning) return;

            _collectedCoins++;
            OnCoinCollected?.Invoke(_collectedCoins);

            if (_collectedCoins >= _goal)
            {
                EndLevel(true);
            }
        }

        private IEnumerator ProceedToNextLevel()
        {
            _currentLevel++;
            yield return new WaitForSecondsRealtime(2);
            GameManager.Instance.SwitchScene(Scenes.GameLevel, _currentLevel);
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
            int maxAttempts = 100;
            for (int i = 0; i < maxAttempts; i++)
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
            if (!Physics.Raycast(position + Vector3.up * 50, Vector3.down, Mathf.Infinity, GroundLayer))
            {
                return false;
            }

            return !Physics.CheckSphere(position, 0.5f);
        }
    }
}