using UnityEngine;

namespace LightPassGame.Game
{
    [DisallowMultipleComponent]
    public class ScoreManager : MonoBehaviour
    {
        public int Value { get; private set; }

        private void Start()
        {
            GameManager.Events.OnCoinDestroy += ChangeScore;
        }

        private void ChangeScore(Coin.Coin _)
        {
            Value++;
            GameManager.Events.OnScoreChanged?.Invoke(Value);
        }
    }
}
