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

        public void AddScore(int value = 1)
        {
            Score += value;
            _textMeshProUGUI.text = Score.ToString();
        }
    }
}
