using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Search
{
    class BFS : Search
    {
        private List<Node> frontier;
        private Node focus;

        private bool finished;

        private States state;

        enum States
        {
            EXPANDING,
            CHECKING
        }

        public BFS(Node startingNode) : base(startingNode)
        {
            frontier = new List<Node>();
            frontier.Add(startingNode);
            focus = startingNode;
            finished = false;
            state = States.CHECKING;
        }

        public override string RunSearch()
        {
            List<Node> frontier = new List<Node>();
            frontier.Add(StartingNode);
            while (frontier.Count != 0)
            {
                // checks if node is at the goal
                if (frontier[0].Cell == CellTypes.GOAL)
                {
                    return frontier[0].Path + " - GOAL!";
                }
                // gets the children of the current node
                List<Node> children = frontier[0].Children;
                // removes the node from the frontier
                frontier.RemoveAt(0);
                foreach (Node child in children)
                {
                    if (!ContainsNode(CheckedNodes, child))
                    {
                        CheckedNodes.Add(child);
                        frontier.Add(child);
                    }
                }
            }
            return "No solution found";
        }

        public override void Update()
        {
            if (state == States.CHECKING)
            {
                if (frontier.Count == 0)
                {
                    return;
                }
                // checks if node is at the goal
                if (focus.Cell == CellTypes.GOAL)
                {
                    finished = true;
                    return;
                }
                // set the focus to the start of the frontier
                focus = frontier[0];
                // removes the node from the frontier
                frontier.RemoveAt(0);
                // checks new position is at the goal before expending the node
                if (focus.Cell == CellTypes.GOAL)
                {
                    finished = true;
                    return;
                }

                state = States.EXPANDING;
            }
            else if (state == States.EXPANDING)
            {

                // gets the children of the current node
                List<Node> children = focus.Children;

                foreach (Node child in children)
                {
                    if (!ContainsNode(CheckedNodes, child))
                    {
                        CheckedNodes.Add(child);
                        frontier.Add(child);
                    }
                }
                state = States.CHECKING;
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

            foreach (Node node in frontier)
            {
                CircleShape circlefrontier = new CircleShape(cellSize / 3);
                circlefrontier.Origin = new SFML.System.Vector2f(cellSize / 3, cellSize / 3);
                circlefrontier.FillColor = new Color(199, 135, 6);
                circlefrontier.Position = new SFML.System.Vector2f(node.X * cellSize + cellSize / 2, node.Y * cellSize + cellSize / 2);
                window.Draw(circlefrontier);
            }

            CircleShape circle = new CircleShape(cellSize / 3);
            circle.Origin = new SFML.System.Vector2f(cellSize / 3, cellSize / 3);
            circle.FillColor = new Color(0, 189, 32);
            circle.Position = new SFML.System.Vector2f(focus.X * cellSize + cellSize / 2, focus.Y * cellSize + cellSize / 2);
            window.Draw(circle);

            if (finished)
            {
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
        }
    }
}
