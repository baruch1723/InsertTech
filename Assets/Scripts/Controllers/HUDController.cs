using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class HUDController : MonoBehaviour
    {
        //public static HUDController instance;

        public Text coinText;
        public Text timerText;
        public Text levelText;
        public GameObject winText;
        //public GameObject winGameText;
        public GameObject loseText;
        
        /*private void Awake()
        {
            if (instance == null)
            {
                instance = this;

            }
            else
            {
                Destroy(gameObject);
            }
        }*/

        /*
        private void Start()
        {
            OnStartLevel();
        }
        */

        void OnEnable()
        {
            LevelManager.OnCoinCollected += UpdateCoinCount;
            LevelManager.OnLevelChanged += UpdateLevelText;
            LevelManager.OnTimeUpdated += UpdateTimer;
            LevelManager.OnGameLoose += OnLoseLevel;
            LevelManager.OnGameWin += OnWinLevel;
        }

        void OnDisable()
        {
            LevelManager.OnCoinCollected -= UpdateCoinCount;
            LevelManager.OnLevelChanged -= UpdateLevelText;
            LevelManager.OnTimeUpdated -= UpdateTimer;
            LevelManager.OnGameLoose -= OnLoseLevel;
            LevelManager.OnGameWin -= OnWinLevel;
        }
        
        public void UpdateCoinCount(int coins)
        {
            coinText.text = "Coins: " + coins + "/" + LevelManager.instance.target;
        }

        public void UpdateTimer(float time)
        {
            timerText.text = "Time: " + Mathf.FloorToInt(time) + "s";
        }

        public void UpdateLevelText(int level)
        {
            levelText.text = "Level: " + level;
            OnStartLevel();
        }
        
        public void OnWinLevel()
        {
            winText.SetActive(true);
        }

        public void OnLoseLevel()
        {
            loseText.SetActive(true);
        }

        public void OnStartLevel()
        {
            winText.SetActive(false);
            //winGameText.SetActive(false);
            loseText.SetActive(false);
            coinText.gameObject.SetActive(true);
            timerText.gameObject.SetActive(true);
        }
    }
}
