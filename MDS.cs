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

        private bool finished;

        private int curSearch;

        private States state;

        enum States
        {
            EXPANDING,
            CHECKING
        }

        public MDS(Node startingNode, Enviroment enviroment) : base(startingNode)
        {
            curSearch = 0;
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

            finished = false;
            state = States.CHECKING;
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
                    //foreach (Node child in children)
                    //{
                    //    if (!ContainsNode(forNodes, child))
                    //    {
                    //        forNodes.Add(child);
                    //        fronteirs[i].Add(child);
                    //    }
                    //}
                }
            }
            return "No solution found";
        }

        public override void Update()
        {
            if (finished)
                return;

            if (state == States.CHECKING)
            {
                state = States.EXPANDING;
                if (fronteirs[curSearch].Count == 0)
                {
                    return;
                }

                // set the focus to the start of the fronteir
                focuses[curSearch] = fronteirs[curSearch][0];
                // removes the node from the fronteir
                fronteirs[curSearch].RemoveAt(0);
                
            }
            else if (state == States.EXPANDING)
            {
                List<Node> forNodes = (curSearch == 0) ? CheckedNodes : goalSideCheckedNodes;
                List<Node> againstNodes = (curSearch != 0) ? CheckedNodes : goalSideCheckedNodes;

                // gets the children of the current node
                List<Node> children = focuses[curSearch].Children;

                foreach (Node child in children)
                {
                    if (!ContainsNode(forNodes, child))
                    {
                        forNodes.Add(child);
                        fronteirs[curSearch].Add(child);
                        // checks new position is at the goal before expending the node
                        if (ContainsNode(againstNodes, child))
                        {
                            finished = true;
                            return;
                        }
                    }
                }
                state = States.CHECKING;
                curSearch = (curSearch + 1) % fronteirs.Count;
            }
            
        }

        public override void Draw(int cellSize, RenderWindow window)
        {
            foreach (Node node in CheckedNodes)
            {
                CircleShape circleChecked = new CircleShape(cellSize / 4);
                circleChecked.Origin = new SFML.System.Vector2f(cellSize / 4, cellSize / 4);
                circleChecked.FillColor = new Color(156, 18, 0);
                circleChecked.Position = new SFML.System.Vector2f(node.X * cellSize + cellSize / 2, node.Y * cellSize + cellSize / 2);
                window.Draw(circleChecked);
                if (node.Parent is Node)
                {
                    VertexArray line = new VertexArray(PrimitiveType.LineStrip, 2);

                    Vertex vertex0 = new Vertex();
                    vertex0.Position = new SFML.System.Vector2f(node.X * cellSize + cellSize / 2, node.Y * cellSize + cellSize / 2);
                    vertex0.Color = Color.Black;
                    line[0] = vertex0;

                    Vertex vertex1 = new Vertex();
                    vertex1.Position = new SFML.System.Vector2f(node.Parent.X * cellSize + cellSize / 2, node.Parent.Y * cellSize + cellSize / 2);
                    vertex1.Color = Color.Black;
                    line[1] = vertex1;

                    window.Draw(line);
                }
            }

            foreach (Node node in goalSideCheckedNodes)
            {
                CircleShape circleChecked = new CircleShape(cellSize / 4);
                circleChecked.Origin = new SFML.System.Vector2f(cellSize / 4, cellSize / 4);
                circleChecked.FillColor = new Color(156, 18, 0);
                circleChecked.Position = new SFML.System.Vector2f(node.X * cellSize + cellSize / 2, node.Y * cellSize + cellSize / 2);
                window.Draw(circleChecked);
                if (node.Parent is Node)
                {
                    VertexArray line = new VertexArray(PrimitiveType.LineStrip, 2);

                    Vertex vertex0 = new Vertex();
                    vertex0.Position = new SFML.System.Vector2f(node.X * cellSize + cellSize / 2, node.Y * cellSize + cellSize / 2);
                    vertex0.Color = Color.Black;
                    line[0] = vertex0;

                    Vertex vertex1 = new Vertex();
                    vertex1.Position = new SFML.System.Vector2f(node.Parent.X * cellSize + cellSize / 2, node.Parent.Y * cellSize + cellSize / 2);
                    vertex1.Color = Color.Black;
                    line[1] = vertex1;

                    window.Draw(line);
                }
            }

            for (int i = 0; i < fronteirs.Count; i++)
            {
                foreach (Node node in fronteirs[i])
                {
                    CircleShape circleFronteir = new CircleShape(cellSize / 3);
                    circleFronteir.Origin = new SFML.System.Vector2f(cellSize / 3, cellSize / 3);
                    circleFronteir.FillColor = new Color(199, 135, 6);
                    circleFronteir.Position = new SFML.System.Vector2f(node.X * cellSize + cellSize / 2, node.Y * cellSize + cellSize / 2);
                    window.Draw(circleFronteir);
                }

                CircleShape circle = new CircleShape(cellSize / 3);
                circle.Origin = new SFML.System.Vector2f(cellSize / 3, cellSize / 3);
                circle.FillColor = (curSearch == i) ? new Color(0, 189, 32) : new Color(255, 189, 32);
                circle.Position = new SFML.System.Vector2f(focuses[i].X * cellSize + cellSize / 2, focuses[i].Y * cellSize + cellSize / 2);
                window.Draw(circle);

                if (finished)
                {
                    Node p = focuses[i];
                    while (p is Node)
                    {
                        RectangleShape rect = new RectangleShape();
                        rect.FillColor = new Color(1, 112, 20);
                        switch (p.Dir)
                        {
                            case Directions.UP:
                                rect.Size = new SFML.System.Vector2f(14, cellSize + 14);
                                rect.Position = new SFML.System.Vector2f(p.X * cellSize + cellSize / 2 - 7, p.Y * cellSize + cellSize / 2 - 7);
                                break;
                            case Directions.DOWN:
                                rect.Size = new SFML.System.Vector2f(14, cellSize + 14);
                                rect.Position = new SFML.System.Vector2f(p.X * cellSize + cellSize / 2 - 7, p.Y * cellSize - cellSize / 2 - 7);
                                break;
                            case Directions.LEFT:
                                rect.Size = new SFML.System.Vector2f(cellSize + 14, 14);
                                rect.Position = new SFML.System.Vector2f(p.X * cellSize + cellSize / 2 - 7, p.Y * cellSize + cellSize / 2 - 7);
                                break;
                            case Directions.RIGHT:
                                rect.Size = new SFML.System.Vector2f(cellSize + 14, 15);
                                rect.Position = new SFML.System.Vector2f(p.X * cellSize - cellSize / 2 - 7, p.Y * cellSize + cellSize / 2 - 7);
                                break;
                        }
                        window.Draw(rect);
                        p = p.Parent;
                    }
                }
            }
        }
    }
}
