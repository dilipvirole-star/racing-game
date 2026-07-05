using UnityEngine;
using TMPro;

namespace RacingGame.UI
{
    /// <summary>
    /// Central UI manager coordinating all screen elements.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        public static UIManager Instance => _instance;

        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private TextMeshProUGUI _speedometerText;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TextMeshProUGUI _levelText;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        public void UpdateSpeedometer(float speed)
        {
            if (_speedometerText != null)
            {
                _speedometerText.text = $"Speed: {speed:F1} km/h";
            }
        }

        public void UpdateMoney(float amount)
        {
            if (_moneyText != null)
            {
                _moneyText.text = $"${amount:F0}";
            }
        }

        public void UpdateLevel(int level)
        {
            if (_levelText != null)
            {
                _levelText.text = $"Level {level}";
            }
        }

        public void ShowMainMenu()
        {
            Debug.Log("Showing Main Menu");
        }

        public void ShowPauseMenu()
        {
            Debug.Log("Showing Pause Menu");
        }

        public void ShowGarage()
        {
            Debug.Log("Showing Garage");
        }
    }
}
