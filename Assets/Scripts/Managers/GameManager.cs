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
        public static GameManager instance;
     
        public enum GameState { Start, Playing, Paused, GameOver }

        private GameState CurrentState { get; set; }
        
        private LevelsWrapper loadedLevels;
        private int _availableLevels;
        
        private const string CurrentLevel = "CurrentLevel";
        
        public int GetAvailableLevels => _availableLevels;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            loadedLevels = LoadLevelsFromJson();
            if (loadedLevels == null)
            {
                Debug.LogError("Levels not found");
                return;
            }
            
            _availableLevels = loadedLevels.Levels.Count;
            ChangeState(GameState.Start);
        }

        public static int GetCurrentLevel()
        {
            var currentLevel = PlayerPrefs.GetInt(CurrentLevel, 1);
            return currentLevel;
        }

        private static void SaveCurrentLevel(int level)
        {
            PlayerPrefs.SetInt(CurrentLevel, level);
        }
        
        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
        }
        
        public void SwitchScene(string scene,int level = 1)
        {
            StartCoroutine(LoadScene(scene,level));
        }

        private void LoadLevel(int levelNumber)
        {
            var level = loadedLevels.Levels.FirstOrDefault(a => a.ID.Equals(levelNumber));
            SaveCurrentLevel(levelNumber);
            LevelManager.instance.StartLevel(level);
        }

        private static LevelsWrapper LoadLevelsFromJson()
        {
            var  jsonFile = Resources.Load<TextAsset>("LevelParams");

            if (jsonFile != null)
            {
                var json = jsonFile.text;
                var levelsWrapper = JsonUtility.FromJson<LevelsWrapper>(json);
                return levelsWrapper;
            }

            Debug.LogError("JSON file not found in Resources");
            return null;
        }

        private IEnumerator LoadScene(string scene,int level)
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

        public void RestartLevel()
        {
            SwitchScene(Scenes.GameLevel,GetCurrentLevel());
        }
    }
}