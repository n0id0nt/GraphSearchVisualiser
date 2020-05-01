using System;
using SFML;
using SFML.Window;
using SFML.Graphics;

namespace Search
{
    class Program
    {
        static public byte FRAMERATE = 10;
        static public bool PAUSED = false;
        public const int CELL_SIZE = 30;
        public const int MENU_WIDTH = 140;

        static Enviroment enviroment;

        static void OnClick(object sender, EventArgs e)
        {
            SFML.System.Vector2i mousePos = Mouse.GetPosition((RenderWindow)sender);
            switch (SortSelectMenu.SelectSort(enviroment.Width * CELL_SIZE, mousePos))
            {
                case 0:
                    enviroment.Agent.setSearch("DFS");
                    break;
                case 1:
                    enviroment.Agent.setSearch("BFS");
                    break;
                case 2:
                    enviroment.Agent.setSearch("GBFS");
                    break;
                case 3:
                    enviroment.Agent.setSearch("AS");
                    break;
                case 4:
                    enviroment.Agent.setSearch("IDDFS");
                    break;
                case 5:
                    enviroment.Agent.setSearch("RBFS");
                    break;
                default:
                    break;
            }
        }

        static void OnKeyRealeased(object sender, KeyEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            if (e.Code == Keyboard.Key.RBracket)
            {
                FRAMERATE += 2;
                if (FRAMERATE > 200)
                    FRAMERATE = 200;

                window.SetFramerateLimit(FRAMERATE);
            }
            else if (e.Code == Keyboard.Key.LBracket)
            {
                FRAMERATE -= 2;
                if (FRAMERATE < 2)
                    FRAMERATE = 2;
                window.SetFramerateLimit(FRAMERATE);
            }
            else if (e.Code == Keyboard.Key.P)
            {
                PAUSED = !PAUSED;
            }
        }

        static void Main(string[] args)
        {
            string file = args[0];
            string method = args[1];

            try
            {
                enviroment = new Enviroment(file);
            }
            catch
            {
                Console.WriteLine("The file provided is not valid");
                return;
            }

            if (method == "GUI")
            {
                RenderWindow window = new RenderWindow(new VideoMode((uint)(enviroment.Width * CELL_SIZE) + MENU_WIDTH, (uint)(enviroment.Height * CELL_SIZE)), "Robot Navigation", Styles.Close);

                window.Closed += (s, a) => window.Close();
                window.MouseButtonReleased += new EventHandler<MouseButtonEventArgs>(OnClick);
                window.KeyReleased += new EventHandler<KeyEventArgs>(OnKeyRealeased);

                window.SetFramerateLimit(FRAMERATE);

                RectangleShape rect = new RectangleShape(new SFML.System.Vector2f(500f, 300f));
                rect.FillColor = new Color(237, 220, 26);
                rect.Position = new SFML.System.Vector2f(20, 20);              

                while (window.IsOpen)
                {
                    window.DispatchEvents();
                    
                    if (!PAUSED)
                        enviroment.Agent.Update();

                    window.Clear(Color.Black);

                    enviroment.DrawGrid(CELL_SIZE, window);
                    enviroment.Agent.Draw(CELL_SIZE, window);
                    SortSelectMenu.DrawMenu(enviroment.Width * CELL_SIZE, window);

                    window.Display();
                }
            }
            else
            {
                string path = enviroment.Agent.Search(method);
                Console.WriteLine("{0}, {1}", file, path);

            }
        }
    }
}
