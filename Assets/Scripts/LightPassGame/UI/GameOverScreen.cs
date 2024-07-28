using LightPassGame.Game;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LightPassGame.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelTextMeshPro;
        [SerializeField] private HoldButton restartButton;
        
        private void Start()
        {
            UpdateScoreValue();
            restartButton.OnButtonPressed += RestartGame;
        }

        private void UpdateScoreValue()
        {
            labelTextMeshPro.text = "score: " + GameManager.Score.Value;
        }
        
        private void RestartGame()
        {
            SceneManager.LoadScene("Scenes/Main Scene");
        }
    }
}
