using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Search
{
    class IDDFS : Search
    {
        private Node focus;

        private int curDepth;
        private int iterDepth;

        private bool finished;
        private bool maxDepthReached;

        private List<Node> iterationCheckedNodes;

        /// <summary>
        /// Denotes the result of each iteration of the search
        /// </summary>
        class SearchResult
        {
            public SearchResult(Node goal, bool remaining)
            {
                Goal = goal;
                Remaining = remaining;
            }

            // the node that reaches the goal
            public Node Goal { get; }
            // if there is still more to search
            public bool Remaining { get; }
        }

        public IDDFS(Node startingNode) : base(startingNode)
        {
            focus = startingNode;
            finished = false;
            maxDepthReached = false;
            curDepth = 0;
            iterDepth = 0;
        }

        public override string RunSearch()
        {
            bool done = false;
            int depth = 0;
            while (!done)
            {
                iterationCheckedNodes = new List<Node>();
                iterationCheckedNodes.Add(StartingNode);

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

                // checks node is not in the current path
                Node parent = node;
                bool inCurPath = false;
                while(parent.Parent is Node)
                {
                    parent = parent.Parent;
                    if (parent.EqualsPos(child))
                    {
                        inCurPath = true;
                    }
                }
                // jump out of cycle if in current path
                if (inCurPath) 
                    continue;

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
            if (iterDepth == 0)
            {
                maxDepthReached = true;
                if (focus.Cell == CellTypes.GOAL)
                {
                    finished = true;
                }
                else
                {
                    if (focus.Parent is Node)
                    {
                        focus = focus.Parent;
                        iterDepth++;
                    }
                    else
                    {
                        focus = StartingNode;
                        curDepth++;
                        iterDepth = curDepth;
                        iterationCheckedNodes = new List<Node>();
                        iterationCheckedNodes.Add(StartingNode);
                        maxDepthReached = false;
                    }
                    
                }
                return;
            }

            foreach (Node child in focus.Children)
            {
                if (!ContainsNode(iterationCheckedNodes, child))
                {
                    iterationCheckedNodes.Add(child);
                    focus = child;
                    iterDepth--;
                    return;
                }
            }

            // has no children 
            if (focus.Parent is Node)
            {
                focus = focus.Parent;
                iterDepth++;
            }
            else if (maxDepthReached == true)
            {
                focus = StartingNode;
                curDepth++;
                iterDepth = curDepth;
                iterationCheckedNodes = new List<Node>();
                iterationCheckedNodes.Add(StartingNode);
                maxDepthReached = false;
            }
        }

        public override void Draw(int cellSize, RenderWindow window)
        {
            foreach (Node node in iterationCheckedNodes)
            {
                CircleShape circleChecked = new CircleShape(cellSize / 4);
                circleChecked.Origin = new SFML.System.Vector2f(cellSize / 4, cellSize / 4);
                circleChecked.FillColor = new Color(156, 18, 0);
                circleChecked.Position = new SFML.System.Vector2f(node.X * cellSize + cellSize / 2, node.Y * cellSize + cellSize / 2);
                window.Draw(circleChecked);
                //SwinGame.FillCircle(Color.DarkRed, node.X * cellSize + cellSize / 2, node.Y * cellSize + cellSize / 2, cellSize / 4);
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
                    //SwinGame.DrawLine(Color.Black,
                    //    node.X * cellSize + cellSize / 2, node.Y * cellSize + cellSize / 2,
                    //     node.Parent.X * cellSize + cellSize / 2, node.Parent.Y * cellSize + cellSize / 2);

                }
            }
            if (finished)
            {
                CircleShape circle = new CircleShape(cellSize / 3);
                circle.Origin = new SFML.System.Vector2f(cellSize / 3, cellSize / 3);
                circle.FillColor = new Color(0, 189, 32);
                circle.Position = new SFML.System.Vector2f(focus.X * cellSize + cellSize / 2, focus.Y * cellSize + cellSize / 2);
                window.Draw(circle);
                //SwinGame.FillCircle(Color.LightGreen, focus.X * cellSize + cellSize / 2, focus.Y * cellSize + cellSize / 2, cellSize / 3);
                Node p = focus;
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
            else
            {
                CircleShape circleFronteir = new CircleShape(cellSize / 3);
                circleFronteir.Origin = new SFML.System.Vector2f(cellSize / 3, cellSize / 3);
                circleFronteir.FillColor = new Color(199, 135, 6);
                circleFronteir.Position = new SFML.System.Vector2f(focus.X * cellSize + cellSize / 2, focus.Y * cellSize + cellSize / 2);
                window.Draw(circleFronteir);
                //winGame.FillCircle(Color.DarkOrange, focus.X * cellSize + cellSize / 2, focus.Y * cellSize + cellSize / 2, cellSize / 3);
            }
        }
    }
}
