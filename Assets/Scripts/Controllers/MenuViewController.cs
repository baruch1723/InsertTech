using Constants;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class MenuViewController : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;

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

        private static void OnStartGame()
        {
            GameManager.instance.SwitchScene(Scenes.GameLevel, 1);
        }

        private static void OnContinueGame()
        {
            GameManager.instance.SwitchScene(Scenes.MenuScene, GameManager.GetCurrentLevel());
        }

        private static void OnExitGame()
        {
            Application.Quit();
        }
    }
}