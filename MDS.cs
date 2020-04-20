using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Search
{
    class MDS : Search
    {
        private List<Node> goals;
        private List<Node> focuses;

        private List<List<Node>> fronteirs;

        private List<Node> goalSideCheckedNodes;

        public MDS(Node startingNode, Enviroment enviroment) : base(startingNode)
        {
            goals = new List<Node>();
            goalSideCheckedNodes = new List<Node>();

            fronteirs = new List<List<Node>>();
            fronteirs.Add(new List<Node>());
            fronteirs[0].Add(startingNode);

            focuses = new List<Node>();
            focuses.Add(startingNode);

            // find all goal states
            for (int y = 0; y < enviroment.Height; y++)
            {
                for (int x = 0; x < enviroment.Width; x++)
                {
                    if (enviroment.GetCell(x, y) == CellTypes.GOAL)
                    {
                        Node goal = new Node(x, y, enviroment, Directions.START, null);
                        goals.Add(goal);

                        goalSideCheckedNodes.Add(goal);

                        fronteirs.Add(new List<Node>());
                        fronteirs[fronteirs.Count - 1].Add(goal);

                        focuses.Add(goal);
                    }
                }
            }
        }

        public override string RunSearch()
        {
            while (fronteirs[0].Count != 0)
            {
                // run BFS for each search
                for (int i = 0; i < fronteirs.Count; i++)
                {
                    // check the node was checked by another list
                    List<Node> forNodes = (i == 0) ? CheckedNodes : goalSideCheckedNodes;
                    List<Node> againstNodes = (i != 0) ? CheckedNodes : goalSideCheckedNodes;

                    if (!ContainsNode(againstNodes, fronteirs[i][0]))
                    {
                        return "found";
                    }
                    // get the children of the current node
                    List<Node> children = fronteirs[i][0].Children;
                    // remove the node from the fronteir
                    fronteirs[i].RemoveAt(0);
                    foreach (Node child in children)
                    {
                        if (!ContainsNode(forNodes, child))
                        {
                            forNodes.Add(child);
                            fronteirs[i].Add(child);
                        }
                    }
                }
            }
            return "No solution found";
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
