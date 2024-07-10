using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class LevelViewController : MonoBehaviour
    {
        [SerializeField] private Text _coinText;
        [SerializeField] private Text _timerText;
        [SerializeField] private Text _levelText;
        
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;

        void OnEnable()
        {
            LevelManager.OnCoinCollected += UpdateCoinCount;
            LevelManager.OnLevelChanged += UpdateLevelText;
            LevelManager.OnTimeUpdated += UpdateTimer;
            
            LevelManager.OnLevelLose += OnLoseLevel;
            LevelManager.OnLevelWin += OnWinLevel;
        }

        void OnDisable()
        {
            LevelManager.OnCoinCollected -= UpdateCoinCount;
            LevelManager.OnLevelChanged -= UpdateLevelText;
            LevelManager.OnTimeUpdated -= UpdateTimer;
            
            LevelManager.OnLevelLose -= OnLoseLevel;
            LevelManager.OnLevelWin -= OnWinLevel;
        }

        private void UpdateCoinCount(int coins)
        {
            _coinText.text = "Coins: " + coins + "/" + LevelManager.instance._goal;
        }

        private void UpdateLevelText(int level)
        {
            _levelText.text = "Level: " + level;
            //OnStartLevel();
        }
        
        private void UpdateTimer(float time)
        {
            _timerText.text = "Time: " + Mathf.FloorToInt(time) + "s";
        }

        private void OnLoseLevel()
        {
            _losePanel.SetActive(true);
        }

        private void OnWinLevel()
        {
            _winPanel.SetActive(true);
        }

        /*//TODO:need to triggred at level start
        public void OnStartLevel()
        {
            winText.SetActive(false);
            loseText.SetActive(false);
            
            coinText.gameObject.SetActive(true);
            timerText.gameObject.SetActive(true);
        }*/
    }
}
