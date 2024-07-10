using Constants;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class MenuViewController : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _deleteDataButton;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartGame);
            _continueButton.onClick.AddListener(OnContinueGame);
            _exitButton.onClick.AddListener(OnExitGame);
            _deleteDataButton.onClick.AddListener(OnDeleteGameData);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartGame);
            _continueButton.onClick.RemoveListener(OnContinueGame);
            _exitButton.onClick.RemoveListener(OnExitGame);
            _exitButton.onClick.RemoveListener(OnDeleteGameData);
        }

        private static void OnStartGame()
        {
            GameManager.Instance.SwitchScene(Scenes.GameLevel, 1);
        }

        private static void OnContinueGame()
        {
            GameManager.Instance.SwitchScene(Scenes.GameLevel, GameManager.GetCurrentLevel());
        }

        private static void OnExitGame()
        {
            Application.Quit();
        }
        
        private static void OnDeleteGameData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}