using System;
using System.Collections;
using System.Linq;
using Constants;
using Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum GameState
        {
            Start,
            Playing,
            Paused,
            GameOver
        }

        private GameState CurrentState { get; set; }

        private LevelsWrapper _loadedLevels;
        private int _availableLevels;

        private const string CurrentLevelKey = "CurrentLevel";

        public int AvailableLevels => _loadedLevels?.Levels.Count ?? 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGameManager();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeGameManager()
        {
            _loadedLevels = LoadLevelsFromJson();
            if (_loadedLevels == null)
            {
                Debug.LogError("Levels not found");
                return;
            }

            ChangeState(GameState.Start);
        }

        public static int GetCurrentLevel() => PlayerPrefs.GetInt(CurrentLevelKey, 1);


        private static void SaveCurrentLevel(int level) => PlayerPrefs.SetInt(CurrentLevelKey, level);


        public void ChangeState(GameState newState) => CurrentState = newState;


        public void SwitchScene(string scene, int level = 1)
        {
            StartCoroutine(LoadScene(scene, level));
        }

        private void LoadLevel(int levelNumber)
        {
            var level = _loadedLevels.Levels.FirstOrDefault(a => a.Index == levelNumber);
            if (level != null)
            {
                SaveCurrentLevel(levelNumber);
                LevelManager.instance.StartLevel(level);
            }
            else
            {
                Debug.LogError($"Level {levelNumber} not found");
            }
        }


        private IEnumerator LoadScene(string scene, int level)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(scene);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            if (!string.Equals(Scenes.MenuScene, scene))
            {
                LoadLevel(level);
            }
        }

        public void RestartLevel() => SwitchScene(Scenes.GameLevel, GetCurrentLevel());


        private static LevelsWrapper LoadLevelsFromJson()
        {
            var jsonFile = Resources.Load<TextAsset>("LevelParams");

            if (jsonFile != null)
            {
                var json = jsonFile.text;
                var levelsWrapper = JsonUtility.FromJson<LevelsWrapper>(json);
                return levelsWrapper;
            }

            Debug.LogError("JSON file not found in Resources");
            return null;
        }
    }
}