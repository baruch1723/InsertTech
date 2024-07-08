using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;

        private readonly string GameScene = "Game";

        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartGame);
            _continueButton.onClick.AddListener(OnContinueGame);
            _exitButton.onClick.AddListener(OnExitGame);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartGame);
            _continueButton.onClick.RemoveListener(OnContinueGame);
            _exitButton.onClick.RemoveListener(OnExitGame);
        }

        private void OnStartGame()
        {
            GameManager.instance.SwitchScene(GameScene,1);
        }

        private void OnContinueGame()
        {
            var currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            GameManager.instance.SwitchScene("Game",currentLevel);
        }

        private void OnExitGame()
        {
            Application.Quit();
        }
    }
}