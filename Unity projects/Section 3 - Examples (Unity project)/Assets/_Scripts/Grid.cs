using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {

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
    }

    // True means passable, false means impassable
    private int[,] _grid;
    private int _width;
    public int Width { get { return _width; } }
    private int _height;
    public int Height { get { return _height; } }

    public int this[int i, int j]
    {
        get
        {
            return _grid[i, j];
        }
        set
        {
            _grid[i, j] = value;
        }
    }

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
        _grid = new int[width, height];
    }

    public List<Point> GetAdjacentCells(Point cell)
    {
        return GetAdjacentCells((int)cell.X, (int)cell.Y);
    }

    public float GetCostOfEnteringCell(Point cell)
    {
        return _grid[(int)cell.X, (int)cell.Y];
    }

    public List<Point> GetAdjacentCells(int x, int y)
    {
        List<Point> adjacentCells = new List<Point>();
        if (x > 0)
        {
            // Look left

            adjacentCells.Add(new Point(x - 1, y));

        }
        if (x < _width - 1)
        {

            adjacentCells.Add(new Point(x + 1, y));

        }
        if (y > 0)
        {
            adjacentCells.Add(new Point(x, y - 1));

        }
        if (y < _height - 1)
        {

            adjacentCells.Add(new Point(x, y + 1));

        }
        return adjacentCells;
    }

    //public List<Point> GetAdjacentCells(int x, int y)
    //{
    //    List<Point> adjacentCells = new List<Point>();
    //    if (x > 0)
    //    {
    //        // Look left
    //        if (_grid[x-1, y])
    //        {
    //            adjacentCells.Add(new Point(x - 1, y));
    //        }
    //    }
    //    if (x < _width -1)
    //    {
    //        if (_grid[x+1, y])
    //        {
    //            adjacentCells.Add(new Point(x + 1, y));
    //        }
    //    }
    //    if (y > 0)
    //    {
    //        if (_grid[x, y-1])
    //        {
    //            adjacentCells.Add(new Point(x, y - 1));
    //        }
    //    }
    //    if (y < _height - 1)
    //    {
    //        if (_grid[x, y+1])
    //        {
    //            adjacentCells.Add(new Point(x, y + 1));
    //        }
    //    }
    //    return adjacentCells;
    //}
}
