using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Search
{
    static class SortSelectMenu
    {

        static private int BUTTON_HEIGHT = 25;

        static private string[] sorts = {
            "DFS",
            "BFS",
            "GBFS",
            "AS",
            "IDDFS",
            "RBFS"
        };

        static public int SelectSort(int gridWidth, SFML.System.Vector2i mousePos)
        {
            if (mousePos.X < gridWidth)
            {
                return -1;
            }
            return mousePos.Y / BUTTON_HEIGHT;
            
        }

        static public void DrawMenu(int xOffset, RenderWindow window)
        {
            Font font = new Font("fonts\\arial.ttf");

            // draw search types
            for (int i = 0; i < sorts.Length; i++)
            {
                Text lable = new Text(sorts[i], font, (uint)(BUTTON_HEIGHT - 2));
                lable.FillColor = Color.White;
                lable.Position = new SFML.System.Vector2f(xOffset + 5, i * BUTTON_HEIGHT);

                window.Draw(lable);
            }
        }
    }
}
