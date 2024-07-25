using System;
using TMPro;
using UnityEngine;

namespace LightPassGame
{
    public class ScoreCounter : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUGUI;
        public int Score { get; private set; }

        private void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            
        }
        private void Start()
        {
            GameManager.Events.OnCoinDestroy += AddScore;
        }

        private void AddScore(Coin coin)
        {
            Score++;
            _textMeshProUGUI.text = Score.ToString();
        }
    }
}
