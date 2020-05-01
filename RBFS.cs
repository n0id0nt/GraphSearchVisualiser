using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SFML.Graphics;

namespace Search
{
    class RBFS : InformedSearch
    {
        private List<SearchResult> focus;
        private List<int> limit;

        private int returnValue;

        private int state; // the state of the update function -1 moving into 0 looping through 1 moving out

        private bool finished;

        private List<List<SearchResult>> score;

        public override ulong Count { get; protected set; }

        /// <summary>
        /// Denotes the result of each iteration of the search
        /// </summary>
        class SearchResult
        {
            public SearchResult(Node result, int remaining)
            {
                Result = result;
                CostLimit = remaining;
            }

            // the node that reaches the goal
            public Node Result { get; }
            // if there is still more to search
            public int CostLimit { get; set; }
        }

        public RBFS(Node startingNode, Enviroment enviroment) : base(startingNode, enviroment)
        {
            focus = new List<SearchResult>();
            focus.Add(new SearchResult(StartingNode, int.MaxValue));
            limit = new List<int>();
            limit.Add(int.MaxValue);
            finished = false;
            state = -1;
            score = new List<List<SearchResult>>();
            Count = 0;
        }

        public override string RunSearch()
        {
            int score = int.MaxValue;
            Node result = RecursiveSearch(new SearchResult(StartingNode, score), score).Result;

            if (result is Node)
            {
                return result.Path + " - GOAL!";
            }

            return "No solution found";
        }

        private SearchResult RecursiveSearch(SearchResult node, int limit)
        {
            // checks if node is at goal
            if (node.Result.Cell == CellTypes.GOAL)
            {
                return node;
            }

            
            List<SearchResult> score = new List<SearchResult>();

            // give a score to each node
            foreach (Node child in node.Result.Children)
            {
                // checks node is not in the current path
                Node parent = node.Result;
                bool inCurPath = false;
                while (parent.Parent is Node)
                {
                    parent = parent.Parent;
                    if (parent.EqualsPos(child))
                    {
                        inCurPath = true;
                    }
                }
                // jump out of cycle if in current path
                if (!inCurPath)
                {
                    score.Add(new SearchResult(child, MovePortential(child.X, child.Y) + NodeCost(child)));
                    Count++;
                }
            }

            while (score.Count != 0)
            {
                SearchResult best = score.OrderBy(n => n.CostLimit).First();
                if (best.CostLimit > limit || best.CostLimit == int.MaxValue)
                    return new SearchResult(null, best.CostLimit);
                SearchResult result = RecursiveSearch(best, (score.Count > 1) ? Math.Min(limit, score.OrderBy(n => n.CostLimit).ElementAt(1).CostLimit) : limit);
                best.CostLimit = result.CostLimit;
                if (result.Result is Node)
                {
                    return result;
                }
                //score.Remove(best);
            }
            return new SearchResult(null, int.MaxValue);
        }

        public override void Update()
        {
            if (state == -1)
            {
                // checks if node is at goal
                if (focus[0].Result.Cell == CellTypes.GOAL)
                {
                    finished = true;
                    return;
                }

                state = 0;

                score.Insert(0, new List<SearchResult>());

                foreach (Node child in focus[0].Result.Children)
                {
                    if (!ContainsNode(CheckedNodes, child))
                    {
                        CheckedNodes.Add(child);
                    }
                    // checks node is not in the current path
                    Node parent = focus[0].Result;
                    bool inCurPath = false;
                    while (parent.Parent is Node)
                    {
                        parent = parent.Parent;
                        if (parent.EqualsPos(child))
                        {
                            inCurPath = true;
                        }
                    }
                    if (!inCurPath)
                        score[0].Add(new SearchResult(child, MovePortential(child.X, child.Y) + NodeCost(child)));
                }

                if (score[0].Count == 0)
                {
                    returnValue = int.MaxValue;
                    state = 1;
                    score.RemoveAt(0);
                }
                return;
            }

            if (state == 0)
            {
                SearchResult best = score[0].OrderBy(n => n.CostLimit).First();
                if (best.CostLimit > limit[0] || best.CostLimit == int.MaxValue)
                {
                    returnValue = best.CostLimit;
                    state = 1;
                    score.RemoveAt(0);
                    return;
                }
                // enter into next depth 
                focus.Insert(0, best);
                limit.Insert(0, (score[0].Count > 1) ? Math.Min(limit[0], score[0].OrderBy(n => n.CostLimit).ElementAt(1).CostLimit) : limit[0]);
                state = -1;
                return;
            }
            
            if (state == 1)
            {
                state = 0;
                focus[0].CostLimit = returnValue;
                focus.RemoveAt(0);
                limit.RemoveAt(0);
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

            //if (finished)
            {
                CircleShape circle = new CircleShape(cellSize / 3);
                circle.Origin = new SFML.System.Vector2f(cellSize / 3, cellSize / 3);
                circle.FillColor = new Color(0, 189, 32);
                circle.Position = new SFML.System.Vector2f(focus[0].Result.X * cellSize + cellSize / 2, focus[0].Result.Y * cellSize + cellSize / 2);
                window.Draw(circle);
                Node p = focus[0].Result;
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
            //else
            {
                CircleShape circlefrontier = new CircleShape(cellSize / 3);
                circlefrontier.Origin = new SFML.System.Vector2f(cellSize / 3, cellSize / 3);
                circlefrontier.FillColor = new Color(199, 135, 6);
                circlefrontier.Position = new SFML.System.Vector2f(focus[0].Result.X * cellSize + cellSize / 2, focus[0].Result.Y * cellSize + cellSize / 2);
                window.Draw(circlefrontier);
            }
        }
    }
}
