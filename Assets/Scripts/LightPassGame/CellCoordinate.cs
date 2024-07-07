using System;
using UnityEngine;

namespace LightPassGame
{
    [Serializable]
    public struct CellCoordinate
    {
        public CellCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public int X;
        public int Y;

        public static CellCoordinate operator +(CellCoordinate cell1, CellCoordinate cell2)
        {
            return new (cell1.X + cell2.X, cell1.Y + cell2.Y);
        }

        public bool IsValid(GameSettings gameSettings) =>
            X >= 0 && Y >= 0 && X < gameSettings.width && Y < gameSettings.height;

        public override string ToString()
        {
            return "( " + X + ", " + Y + " )";
        }
    }
}
