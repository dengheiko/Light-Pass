using UnityEngine.Events;

namespace LightPassGame.Maze
{
    public class CellPathfinderData
    {
        public int DistanceFromSource;
        public int DistanceToTarget;
        public bool Passed;

        public int Cost => DistanceFromSource + DistanceToTarget;
        
        public CellPathfinderData()
        {
            Reset();
            ResetEvent.AddListener(Reset);
        }

        public void UpdateData(int source, int target)
        {
            // DistanceFromSource = Mathf.Min(DistanceFromSource, source);
            // DistanceToTarget = Mathf.Min(DistanceToTarget, target);

            DistanceFromSource = source;
            DistanceToTarget = target;
        }
        
        private void Reset()
        {
            DistanceToTarget = int.MaxValue;
            DistanceToTarget = int.MaxValue;
            Passed = false;
        }
        
        private static UnityEvent _resetEvent;

        private static UnityEvent ResetEvent => _resetEvent ??= new UnityEvent();

        public static void ResetAll()
        {
            ResetEvent.Invoke();
        }
        
        
    }
}