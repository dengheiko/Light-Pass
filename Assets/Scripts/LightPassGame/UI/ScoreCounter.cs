using LightPassGame.Game;
using TMPro;
using UnityEngine;

namespace LightPassGame.UI
{
    public class ScoreCounter : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUGUI;

        private void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            
        }
        private void Start()
        {
            GameManager.Events.OnScoreChanged += ScoreChanged;
        }

        private void ScoreChanged(int value)
        {
            
            _textMeshProUGUI.text = value.ToString();
        }
    }
}
