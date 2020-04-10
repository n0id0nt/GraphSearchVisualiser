using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using SFML.Window;
using SFML.Graphics;

namespace Search
{
    /// <summary>
    /// Cell types represents the kind of cells that appear in the enviroment
    /// </summary>
    enum CellTypes
    {
        EMPTY,
        WALL,
        GOAL
    }
    /// <summary>
    /// The envroment of the program this houses the agent and the grid
    /// </summary>
    class Enviroment
    {
        // The grid 
        private CellTypes[,] grid;

        public int Width { get; }
        public int Height { get; }

        public Agent Agent { get; }

        /// <summary>
        /// Gets the CellType from the specifies location in the grid
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>The CellType</returns>
        public CellTypes GetCell(int x, int y)
        {
            // test if the coordinates are within the range of the grid size
            if (x < 0 || y < 0 || x >= grid.GetLength(0) || y >= grid.GetLength(1))
            {
                return CellTypes.WALL;
            }
            return grid[x, y];
        }

        public Enviroment(string file)
        {
            // pass in the text file
            StreamReader reader = new StreamReader(file);

            // extracts the value to an array 
            string[] size = Regex.Match(reader.ReadLine(), @"(?<=\[).+?(?=\])").Value.Split(',');
            // set the of the array to whats defined in the size array
            grid = new CellTypes[short.Parse(size[1]), short.Parse(size[0])];
            Width = short.Parse(size[1]);
            Height = short.Parse(size[0]);
            // set the values in array to be empty
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y] = CellTypes.EMPTY;
                }
            }

            // extract the agents initial coordinates 
            string[] agent = Regex.Match(reader.ReadLine(), @"(?<=\().+?(?=\))").Value.Split(',');
            // create the agent
            Agent = new Agent(short.Parse(agent[0]), short.Parse(agent[1]), this);

            // extract the goals from the file
            string[] goals = reader.ReadLine().Split('|');
            foreach (string goal in goals)
            {
                string[] g = Regex.Match(goal, @"(?<=\().+?(?=\))").Value.Split(',');
                grid[short.Parse(g[0]), short.Parse(g[1])] = CellTypes.GOAL;
            }

            string wall;

            while ((wall = reader.ReadLine()) != null)
            {
                string[] w = Regex.Match(wall, @"(?<=\().+?(?=\))").Value.Split(',');
                for (int y = 0; y < short.Parse(w[3]); y++)
                {
                    for (int x = 0; x < short.Parse(w[2]); x++)
                    {
                        grid[short.Parse(w[0]) + x, short.Parse(w[1]) + y] = CellTypes.WALL;
                    }
                }
            }
        }

        /// <summary>
        /// Draws the envrioment grid to the top left corner
        /// </summary>
        /// <param name="cellSize">The size of each cell</param>
        public void DrawGrid(int cellSize, RenderWindow window)
        {
            DrawGrid(cellSize, 0, 0, window);
        }
        /// <summary>
        /// Draws the envrioment grid
        /// </summary>
        /// <param name="cellSize">The size of each cell</param>
        /// <param name="xOffset">The offset in the X dimention</param>
        /// <param name="yOffset">The offset in the Y dimention</param>
        public void DrawGrid(int cellSize, int xOffset, int yOffset, RenderWindow window)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    Color color;
                    switch (grid[x, y])
                    {
                        case CellTypes.EMPTY:
                            color = Color.White;
                            break;
                        case CellTypes.GOAL:
                            color = Color.Green;
                            break;
                        case CellTypes.WALL:
                            color = new Color(100, 100, 100);
                            break;
                        default:
                            color = Color.White;
                            break;
                    }
                    RectangleShape rect = new RectangleShape(new SFML.System.Vector2f(cellSize - 2, cellSize - 2));
                    rect.FillColor = color;
                    rect.Position = new SFML.System.Vector2f(x * cellSize + 1 + xOffset, y * cellSize + 1 + yOffset);
                    //SwinGame.FillRectangle(color, x * cellSize + 1 + xOffset, y * cellSize + 1 + yOffset, cellSize - 2, cellSize - 2);
                    window.Draw(rect);
                }
            }
        }
    }
}
