using System.Collections.Generic;

namespace Video_5._2___Random
{
    public enum GhostAIState { Random }

    public class GhostAI
    {
        private Grid _grid;
        private Stack<Point> _pathList;
        private GhostAIState _AIState;

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
        /// <returns></returns>
        public Point GetNextMoveGoal(Point ghostPosition)
        {
            if (_pathList == null || _pathList.Count == 0)
            {
                GetNewPath(ghostPosition);
            }
            if (_pathList != null && _pathList.Count > 0)
            {
                return _pathList.Pop();
            }
            return ghostPosition;
        }

        private void GetNewPath(Point ghostPosition)
        {
            List<Point> path = null;

            switch (_AIState)
            {
                case GhostAIState.Random:
                    path = GridSearch.BestFirstSearch(_grid, ghostPosition, _grid.GetRandomOpenPoint()).Path;
                    break;
            }

            _pathList = new Stack<Point>(path);
        }
    }
}
