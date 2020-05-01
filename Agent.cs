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

        /// <summary>
        /// Sets the search method of the agent
        /// </summary>
        /// <param name="method">The new search method</param>
        /// <returns>Whether the method was successfully changed</returns>
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
                case "IDDFS":
                    searchMethod = new IDDFS(this);
                    break;
                case "RBFS":
                    searchMethod = new RBFS(this, enviroment);
                    break;
                default:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// sets a search method and preforms that search
        /// </summary>
        /// <param name="method"></param>
        /// <returns>The path of the search method </returns>
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

        /// <summary>
        /// runs the update function to step through the search methods visualisation
        /// </summary>
        public void Update()
        {
            if (searchMethod is Search)
                searchMethod.Update();
        }

        /// <summary>
        /// draws self to the screen with the search method
        /// </summary>
        /// <param name="cellSize"></param>
        /// <param name="window"></param>
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
