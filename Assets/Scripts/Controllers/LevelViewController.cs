using System.Collections;
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
        [SerializeField] private GameObject _instructionsPanel;

        private string InstructionsState = "Instructions";
        
        private void OnEnable()
        {
            LevelManager.OnCoinCollected += UpdateCoinCount;
            LevelManager.OnLevelChanged += OnLevelChanged;
            LevelManager.OnTimeUpdated += UpdateTimer;
            
            LevelManager.OnLevelLose += OnLoseLevel;
            LevelManager.OnLevelWin += OnWinLevel;
        }

        private void OnDisable()
        {
            LevelManager.OnCoinCollected -= UpdateCoinCount;
            LevelManager.OnLevelChanged -= OnLevelChanged;
            LevelManager.OnTimeUpdated -= UpdateTimer;
            
            LevelManager.OnLevelLose -= OnLoseLevel;
            LevelManager.OnLevelWin -= OnWinLevel;
        }

        private void UpdateCoinCount(int coins)
        {
            _coinText.text = "Coins: " + coins + "/" + LevelManager.instance._goal;
        }

        private void OnLevelChanged(int level)
        {
            OnStartLevel();
            _levelText.text = "Level: " + level;
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
        
        private void OnStartLevel()
        {
            _winPanel.SetActive(false);
            _losePanel.SetActive(false);

            _coinText.gameObject.SetActive(true);
            _timerText.gameObject.SetActive(true);
            _levelText.gameObject.SetActive(true);
            ShowInstructions();
        }

        private IEnumerator PlayInstructions()
        {
            _instructionsPanel.SetActive(true);
            yield return new WaitForSecondsRealtime(3);
            _instructionsPanel.SetActive(false);
        }

        private void ShowInstructions()
        {
            if (PlayerPrefs.GetInt(InstructionsState, 0) != 0) return;
            
            StartCoroutine(PlayInstructions());
            PlayerPrefs.SetInt(InstructionsState, 1);
        }
    }
}