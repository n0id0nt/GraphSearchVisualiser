using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Search
{
    abstract class Search
    {
        protected Node StartingNode { get; }
        public List<Node> CheckedNodes { get; set; }

        protected Search(Node startingNode)
        {
            StartingNode = startingNode;
            CheckedNodes = new List<Node>();
            CheckedNodes.Add(startingNode);
        }

        public abstract void Update();

        public abstract void Draw(int cellSize, RenderWindow window);

        public abstract string RunSearch();

        /// <summary>
        /// checks if a list has contains a node
        /// used to check if node has aready checked a node
        /// </summary>
        /// <param name="nodes">the list of nodes</param>
        /// <param name="node">the node to check against</param>
        /// <returns>whether the node is in the list</returns>
        protected bool ContainsNode(List<Node> nodes, Node node)
        {
            foreach (Node a in nodes)
            {
                if (node.EqualsPos(a))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
