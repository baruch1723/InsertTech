using System.Collections;
using System.Linq;
using Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
     
        public enum GameState { Start, Playing, Paused, GameOver }

        
        private LevelsWrapper loadedLevels;
        private int _availableLevels;
        public int GetAvailableLevels => _availableLevels;
        public GameState CurrentState { get; private set; }

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
            
            loadedLevels = LoadLevels();
            _availableLevels = loadedLevels.Levels.Count;
        }
        
        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
            // Handle state transitions
        }
        public void SwitchScene(string scene,int level = 1)
        {
            StartCoroutine(LoadScene(scene,level));
        }

        private void LoadLevel(int level)
        {
            var l = loadedLevels.Levels.FirstOrDefault(a => a.ID.Equals(level));
            PlayerPrefs.SetInt("CurrentLevel", l.ID);
            LevelManager.instance.StartLevel(l);
        }

        private static LevelsWrapper LoadLevels()
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
            
            if (!string.Equals("MainScene", scene))
            {
                LoadLevel(level);
            }
        }
    }
}