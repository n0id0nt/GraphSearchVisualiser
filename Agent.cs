using System;
using System.Collections.Generic;
using System.Text;
using SFML.Window;
using SFML.Graphics;


namespace Search
{
    class Agent : Node
    {
        private Search searchMethod;

        public Agent(int x, int y, Enviroment enviroment) : base(x, y, enviroment, Directions.START, null)
        {
        }

        public bool setSearch(string method)
        {
            switch (method)
            {
                case "DFS":
                    searchMethod = new DFS(this);
                    break;
                case "BFS":
                    searchMethod = new BFS(this);
                    break;
                case "GBFS":
                    searchMethod = new GBFS(this, enviroment);
                    break;
                case "AS":
                    searchMethod = new AS(this, enviroment);
                    break;
                case "MDS":
                    searchMethod = new MDS(this, enviroment);
                    break;
                case "CUS2":
                    return false;
                default:
                    return false;
            }
            return true;
        }

        public string Search(string method)
        {
            if (setSearch(method))
            {
                string result = string.Format("{2}, nodes checked = {1},\n{0}", searchMethod.RunSearch(), searchMethod.CheckedNodes.Count, method);

                searchMethod.CheckedNodes = new List<Node>(); // reset the list of nodes

                return result;
            }
            else
            {
                return method + " is an invalid search method";
            }
        }

        private bool ContainsNode(List<Node> nodes, Node node)
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

        public void Update()
        {
            if (searchMethod is Search)
                searchMethod.Update();
        }

        public void Draw(int cellSize, RenderWindow window)
        {
            RectangleShape rect = new RectangleShape(new SFML.System.Vector2f(cellSize - 8, cellSize - 8));
            rect.FillColor = Color.Red;
            rect.Position = new SFML.System.Vector2f(x * cellSize + 4, y * cellSize + 4);
            window.Draw(rect);

            if (searchMethod is Search)
                searchMethod.Draw(cellSize, window);
        }
    }
}
