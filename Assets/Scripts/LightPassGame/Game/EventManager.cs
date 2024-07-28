using System;

namespace LightPassGame.Game
{
    public class EventManager
    {
        public Action<Coin.Coin> OnCoinDestroy;
        public Action<int> OnScoreChanged;
        public Action OnPlayerDamage;
    }
}