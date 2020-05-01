using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Search
{
    abstract class InformedSearch : Search
    {
        protected List<int[]> Goals { get; }

        public InformedSearch(Node startingNode, Enviroment enviroment) : base(startingNode)
        {
            Goals = new List<int[]>();
            // find all goal states
            for (int y = 0; y < enviroment.Height; y++)
            {
                for (int x = 0; x < enviroment.Width; x++)
                {
                    if (enviroment.GetCell(x, y) == CellTypes.GOAL)
                    {
                        int[] pos = { x, y };
                        Goals.Add(pos);
                    }
                }
            }
        }

        /// <summary>
        /// Finds the shortest distance to any goal ignoring walls
        /// </summary>
        /// <param name="x">x pos of goal</param>
        /// <param name="y">y pos of goal</param>
        /// <returns>Distance to closest goal</returns>
        protected int MovePortential(int x, int y)
        {
            int score = -1;
            foreach (int[] goal in Goals)
            {
                int s = Math.Max(x - goal[0], goal[0] - x) + Math.Max(y - goal[1], goal[1] - y); // max function used to select positive value
                score = (score == -1) ? s : Math.Min(s, score);
            }
            return score;
        }

        /// <summary>
        /// Finds the cost of the Nodes current path
        /// </summary>
        /// <param name="node"></param>
        /// <returns>the path cast</returns>
        protected int NodeCost(Node node)
        {
            // each move has a uniform cost of one
            int cost = 0;
            Node p = node.Parent;
            while (p is Node)
            {
                cost += 1;
                p = p.Parent;
            }
            return cost;
        }

        public override abstract string RunSearch();

        public override abstract void Update();

        public override abstract void Draw(int cellSize, RenderWindow window);
    }
}
