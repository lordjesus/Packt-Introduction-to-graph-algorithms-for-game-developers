using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_5._4___Fleeing
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is Point)
            {
                Point p = obj as Point;
                return this.X == p.X && this.Y == p.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 6949;
                // Suitable nullity checks etc, of course :)
                hash = hash * 7907 + X.GetHashCode();
                hash = hash * 7907 + Y.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return "P(" + this.X + ", " + this.Y + ")";
        }
    }

    public enum CellType
    {
        Wall, Empty, Coin, Powerup, Ghostery, Start, GhostBegin
    }

    public enum MoveDirection
    {
        Up, Left, Down, Right
    }

    public class Grid
    {
        private CellType[,] _grid;
        private int _width;
        public int Width { get { return _width; } }
        private int _height;
        public int Height { get { return _height; } }
        public Point GhostBeginPosition { get; private set; }
        private List<Point> _openList = new List<Point>();

        public CellType this[int i, int j]
        {
            get
            {
                return _grid[i, j];
            }
            set
            {
                if (IsCellPassable(value))
                {
                    _openList.Add(new Point(i, j));
                }
                if (value == CellType.GhostBegin)
                {
                    GhostBeginPosition = new Point(i, j);
                }
                _grid[i, j] = value;
            }
        }

        public static CellType ParseCellType(string s)
        {
            switch (s)
            {
                case "w":
                    return CellType.Wall;
                case "g":
                    return CellType.Ghostery;
                case "0":
                    return CellType.Empty;
                case "1":
                    return CellType.Coin;
                case "2":
                    return CellType.Powerup;
                case "s":
                    return CellType.Start;
                case "b":
                    return CellType.GhostBegin;
                default:
                    return CellType.Empty;
            }

        }

        public static bool IsCellPassable(CellType cellType)
        {
            return cellType != CellType.Wall && cellType != CellType.Ghostery;
        }

        public static Grid LoadPacmanLevel(string levelData)
        {
            int width = 28, height = 31;
            string[] lines = levelData.Split('\n');

            Grid grid = new Grid(width, height);

            for (int j = 0; j < lines.Length; j++)
            {
                string[] cells = lines[j].Trim().Split(' ');
                for (int i = 0; i < cells.Length; i++)
                {
                    string cell = cells[i];
                    grid[i, j] = ParseCellType(cells[i]);
                }
            }

            return grid;
        }

        public Grid(int width, int height)
        {
            _width = width;
            _height = height;
            _grid = new CellType[width, height];
        }

        public Point GetRandomOpenPoint()
        {
            Random rand = new Random();
            return _openList[rand.Next(0, _openList.Count - 1)];
        }

        public List<Point> GetAdjacentCells(Point cell)
        {
            return GetAdjacentCells((int)cell.X, (int)cell.Y);
        }

        public float GetCostOfEnteringCell(Point cell)
        {
            return 1;
        }

        public List<Point> GetAdjacentCells(int x, int y)
        {
            List<Point> adjacentCells = new List<Point>();
            if (x > 0)
            {
                // Look left
                if (IsCellPassable(_grid[x - 1, y]))
                {
                    adjacentCells.Add(new Point(x - 1, y));
                }
            }
            if (x < _width - 1)
            {
                if (IsCellPassable(_grid[x + 1, y]))
                {
                    adjacentCells.Add(new Point(x + 1, y));
                }
            }
            if (y > 0)
            {
                if (IsCellPassable(_grid[x, y - 1]))
                {
                    adjacentCells.Add(new Point(x, y - 1));
                }
            }
            if (y < _height - 1)
            {
                if (IsCellPassable(_grid[x, y + 1]))
                {
                    adjacentCells.Add(new Point(x, y + 1));
                }
            }
            return adjacentCells;
        }
    }
}
