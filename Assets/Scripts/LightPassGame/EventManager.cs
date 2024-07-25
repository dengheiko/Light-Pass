using System;
using UnityEngine;
using UnityEngine.Events;

namespace LightPassGame
{
    public class EventManager
    {
        public Action<Coin> OnCoinDestroy;
    }
}