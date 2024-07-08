using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class PauseManager : MonoBehaviour
    {
        private string MainScene = "MainScene";
        
        [SerializeField] private Button _runtimeExitButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _restartButton;

        [SerializeField] private GameObject _pauseMenu;
        private bool _paused;
        
        public static PauseManager instance;
        
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
            
            _pauseMenu.SetActive(false);
            _paused = true;
        }
        
        private void OnEnable()
        {
            _pauseButton.onClick.AddListener(OnPause);
            _restartButton.onClick.AddListener(OnRestartLevel);
            _runtimeExitButton.onClick.AddListener(OnReturnToMainScene);
        }

        private void OnDisable()
        {
            _pauseButton.onClick.RemoveListener(OnPause);
            _restartButton.onClick.RemoveListener(OnRestartLevel);
            _runtimeExitButton.onClick.RemoveListener(OnReturnToMainScene);
        }

        private void OnPause()
        {
            Debug.Log($"_paused: {_paused}");
            _paused = !_paused;
            _pauseMenu.SetActive(_paused);
        }

        public void Pause() => OnPause();
        
        private void OnRestartLevel()
        {
            _pauseMenu.SetActive(false);
            var currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            GameManager.instance.SwitchScene("Game",currentLevel);
        }
        
        private void OnReturnToMainScene()
        {
            _pauseMenu.SetActive(false);
            GameManager.instance.SwitchScene(MainScene);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPause();
            }
        }
    }
}