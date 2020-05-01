using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SFML.Graphics;

namespace Search
{
    class RBFS : InformedSearch
    {
        private Node focus;

        private List<Node> frontier; 

        private bool finished;

        public RBFS(Node startingNode, Enviroment enviroment) : base(startingNode, enviroment)
        {
            focus = startingNode;
            finished = false;
            frontier = new List<Node>();
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

            Dictionary<Node, int> score = new Dictionary<Node, int>();

            // give a score to each node
            foreach (Node child in node.Children)
            {
                if (!ContainsNode(CheckedNodes, child))
                {
                    CheckedNodes.Add(child);
                    score.Add(child, MovePortential(child.X, child.Y));
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
            // checks if node is at goal
            if (focus.Cell == CellTypes.GOAL)
            {
                finished = true;
                return;
            }

            Node bestNode = null;
            int bestScore = -1;

            foreach (Node child in focus.Children)
            {
                if (!ContainsNode(CheckedNodes, child) && !ContainsNode(frontier, child))
                {
                    int score = MovePortential(child.X, child.Y);
                    if (score < bestScore || bestScore == -1)
                    {
                        bestNode = child;
                        bestScore = score;
                        
                    }
                    frontier.Add(child);
                }
            }

            // select focus
            if (bestNode is Node)
            {
                focus = bestNode;
                CheckedNodes.Add(bestNode);
                frontier.Remove(bestNode);
                return;
            }

            // has no children set focus to parent
            if (focus.Parent is Node)
            {
                focus = focus.Parent;

                // remove chilren from frontier
                foreach (Node child in focus.Children)
                {
                    frontier.RemoveAll(n => child.EqualsPos(n));
                }
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
                CircleShape circlefrontier = new CircleShape(cellSize / 3);
                circlefrontier.Origin = new SFML.System.Vector2f(cellSize / 3, cellSize / 3);
                circlefrontier.FillColor = new Color(199, 135, 6);
                circlefrontier.Position = new SFML.System.Vector2f(focus.X * cellSize + cellSize / 2, focus.Y * cellSize + cellSize / 2);
                window.Draw(circlefrontier);
                //winGame.FillCircle(Color.DarkOrange, focus.X * cellSize + cellSize / 2, focus.Y * cellSize + cellSize / 2, cellSize / 3);
            }
        }
    }
}
