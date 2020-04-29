using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SFML.Graphics;

namespace Search
{
    class RBFS : InformedSearch
    {
        public RBFS(Node startingNode, Enviroment enviroment) : base(startingNode, enviroment)
        {

        }

        public override string RunSearch()
        {
            Node result = RecursiveSearch(StartingNode);

            if (result is Node)
            {
                return result.Path + " - GOAL!";
            }

            return "No solution found";
        }

        private Node RecursiveSearch(Node node)
        {
            // checks if node is at goal
            if (node.Cell == CellTypes.GOAL)
            {
                return node;
            }

            List<Node> children = node.Children;

            if (children.Count == 0)
            {
                return null;
            }


            Dictionary<Node, int> score = new Dictionary<Node, int>();

            // give a score to each node
            foreach (Node child in children)
            {
                if (!ContainsNode(CheckedNodes, child))
                {
                    CheckedNodes.Add(child);
                    score.Add(child, MovePortential(child.X, child.Y) + NodeCost(child));
                }
            }

            while (score.Count != 0)
            {
                Node best = score.OrderBy(n => n.Value).First().Key;
                Node result = RecursiveSearch(best);
                if (result is Node)
                {
                    return result;
                }
                score.Remove(best);
            }
            return null;
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Draw(int cellSize, RenderWindow window)
        {
            throw new NotImplementedException();
        }
    }
}
