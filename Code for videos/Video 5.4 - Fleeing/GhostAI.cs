using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_5._4___Fleeing
{
    public enum GhostAIState { Random, TargetingWithLag, Fleeing }

    public class GhostAI
    {
        private Grid _grid;
        private Stack<Point> _pathList;
        private GhostAIState _AIState;
        private int _AIrefreshCount = 0;

        public GhostAI(Grid grid)
        {
            _grid = grid;
        }

        /// <summary>
        /// Set the AI state of the ghost
        /// </summary>
        /// <param name="state">New AI state to use</param>
        public void SetAIState(GhostAIState state)
        {
            _AIState = state;
            _pathList = null;
        }

        /// <summary>
        /// Get next point to move to for the AI. Call when last goal has been reached
        /// </summary>
        /// <param name="ghostPosition">Position of ghost</param>
        /// <param name="playerPosition">Position of player</param>
        /// <returns></returns>
        public Point GetNextMoveGoal(Point ghostPosition, Point playerPosition)
        {
            if (_AIState == GhostAIState.TargetingWithLag && _AIrefreshCount > 9)
            {
                _AIrefreshCount = 0;
                GetNewPath(ghostPosition, playerPosition);
            }
            if (_pathList == null || _pathList.Count == 0)
            {
                GetNewPath(ghostPosition, playerPosition);
            }
            if (_pathList != null && _pathList.Count > 0)
            {
                _AIrefreshCount++;
                if (_pathList.Count == 1 && _AIState == GhostAIState.Fleeing)
                {
                    _AIState = GhostAIState.Random;
                    GetNewPath(ghostPosition, playerPosition);
                }
                return _pathList.Pop();
            }

            return ghostPosition;
        }

        private void GetNewPath(Point ghostPosition, Point playerPosition)
        {
            List<Point> path = null;

            switch (_AIState)
            {
                case GhostAIState.Random:
                    path = GridSearch.BestFirstSearch(_grid, ghostPosition, _grid.GetRandomOpenPoint()).Path;
                    break;
                case GhostAIState.TargetingWithLag:
                    path = GridSearch.AStarSearch(_grid, ghostPosition, playerPosition).Path;
                    break;
                case GhostAIState.Fleeing:

                    SearchResult result = GridSearch.DijkstraGeneral(_grid, playerPosition);

                    Point best = null;
                    float largestDist = 0;
                    foreach (Point k in result.DistanceMap.Keys)
                    {
                        if (result.DistanceMap[k] > largestDist)
                        {
                            best = k;
                            largestDist = result.DistanceMap[k];
                        }
                    }

                    Dictionary<Point, float> costMap = InvertCostMap(result.DistanceMap, largestDist);

                    path = GridSearch.AStarSearchWithCost(_grid, ghostPosition, best, costMap).Path;

                    break;
            }

            _pathList = new Stack<Point>(path);
        }

        private Dictionary<Point, float> InvertCostMap(Dictionary<Point, float> map, float largestCost)
        {
            Dictionary<Point, float> inverted = new Dictionary<Point, float>();
            foreach (Point p in map.Keys)
            {
                float cost = largestCost - map[p];
                cost = cost * cost * cost; // cost^3
                inverted.Add(p, cost);
            }
            return inverted;
        }
    }
}
