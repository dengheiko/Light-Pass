namespace LightPassGame.Maze
{
    public static class CellOffsetCoordinates
    {
        private static CellCoordinate[] _all;

        public static CellCoordinate[] All
        {
            get
            {
                return _all ??= new[]
                {
                    new CellCoordinate(0, -1),
                    new CellCoordinate(1, 0),
                    new CellCoordinate(0, 1),
                    new CellCoordinate(-1, 0)
                };
            }
        }

        public static CellCoordinate Get(int index) => All[index];
        public static int Length => All.Length;

    }
}
