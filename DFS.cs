using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Search
{
    class DFS : Search
    {
        private Node startingNode;
        private Node focus;

        private bool finished;

        public DFS(Node startingNode) : base(startingNode)
        {
            this.startingNode = startingNode;
            focus = startingNode;
            finished = false;
        }

        public override string RunSearch()
        {
            return RecursiveSearch(startingNode);
        }

        private string RecursiveSearch(Node node)
        {
            // checks if node is at goal
            if (node.Cell == CellTypes.GOAL)
            {
                return node.Path + " - GOAL!";
            }

            foreach (Node child in node.Children)
            {
                // loops through the already checked nodes to see if child has already been looked at
                if (ContainsNode(CheckedNodes, child))
                {
                    continue; // skip itteration of the loop since already look
                }
                CheckedNodes.Add(child); // since node not found in the list it is added to the checked nodes list
                string result = RecursiveSearch(child);
                if (result != "No solution found") return result;
            }
            return "No solution found";
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
