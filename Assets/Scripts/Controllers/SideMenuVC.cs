using Constants;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class SideMenuVC : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _sideMenuButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private GameObject _sideMenuContainer;
        
        private bool _opened;
        
        private void Awake()
        {
            _opened = false;
            _sideMenuContainer.SetActive(false);
        }
        
        private void OnEnable()
        {
            _sideMenuButton.onClick.AddListener(OnOpenMenu);
            _restartButton.onClick.AddListener(OnRestartLevel);
            _exitButton.onClick.AddListener(OnReturnToMainScene);
        }

        private void OnDisable()
        {
            _sideMenuButton.onClick.RemoveListener(OnOpenMenu);
            _restartButton.onClick.RemoveListener(OnRestartLevel);
            _exitButton.onClick.RemoveListener(OnReturnToMainScene);
        }

        private void OnOpenMenu()
        {
            _opened = !_opened;
            _sideMenuContainer.SetActive(_opened);
        }
        
        private static void OnRestartLevel()
        {
            GameManager.instance.RestartLevel();
        }
        
        private static void OnReturnToMainScene()
        {
            GameManager.instance.ChangeState(GameManager.GameState.Start);
            GameManager.instance.SwitchScene(Scenes.MenuScene);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnOpenMenu();
            }
        }
    }
}