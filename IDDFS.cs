using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Search
{
    class SearchResult
    {
        public SearchResult(Node goal, bool remaining)
        {
            Goal = goal;
            Remaining = remaining;
        }

        public Node Goal { get; }
        public bool Remaining { get; }
    }

    class IDDFS : Search
    {
        private Node focus;

        private bool finished;

        private List<Node> iterationCheckedNodes;

        public IDDFS(Node startingNode) :  base(startingNode)
        {
            focus = startingNode;
            finished = false;
        }

        public override string RunSearch()
        {
            bool done = false;
            int depth = 0;
            while(!done)
            {
                iterationCheckedNodes = new List<Node>();

                SearchResult result = RecursiveSearch(StartingNode, depth);

                if (result.Goal is Node)
                {
                    return result.Goal.Path + " - GOAL!";
                }
                else if (!result.Remaining)
                {
                    done = true;
                }

                depth++;
            }

            return "No solution found";
        }

        private SearchResult RecursiveSearch(Node node, int depth)
        {
            // checks if node is at goal
            if (depth == 0)
            {
                if (node.Cell == CellTypes.GOAL)
                {
                    return new SearchResult(node, true);
                }
                else
                {
                    return new SearchResult(null, true);
                }
            }

            bool remaining = false;
            foreach (Node child in node.Children)
            {
                CheckedNodes.Add(child);

                if (ContainsNode(iterationCheckedNodes, child))
                {
                    continue; // skip itteration of the loop since already look
                }
                iterationCheckedNodes.Add(child);

                SearchResult result = RecursiveSearch(child, depth - 1);
                if (result.Goal is Node)
                {
                    return result;
                }
                if (result.Remaining)
                {
                    remaining = true;
                }
            }
            return new SearchResult(null, remaining);
        }

        public override void Update()
        {
            // checks if node is at goal
            if (focus.Cell == CellTypes.GOAL)
            {
                finished = true;
                return;
            }

            foreach (Node child in focus.Children)
            {
                // loops through the already checked nodes to see if child has already been looked at
                if (ContainsNode(CheckedNodes, child))
                {
                    continue; // skip itteration of the loop since already look
                }
                CheckedNodes.Add(child); // since node not found in the list it is added to the checked nodes list
                focus = child; // make the child the focus
                return;
            }

            // has no children set target to parent
            if (focus.Parent is Node)
            {
                focus = focus.Parent;
            }
            return;
        }

        public override void Draw(int cellSize, RenderWindow window)
        {
            throw new NotImplementedException();
        }
    }
}
