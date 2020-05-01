using System;
using System.Collections.Generic;
using System.Text;

namespace Search
{
    enum Directions
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        START
    }
    class Node
    {
        protected int x;
        protected int y;
        private Directions dir;
        private Node parent;

        public Node Parent { get { return parent; } }
        public Directions Dir { get { return dir; } }

        public int X { get { return x; } }
        public int Y { get { return y; } }

        public string Path
        {
            get
            {
                string path = this.Name;
                Node p = parent;
                while (p is Node)
                {

                    path = string.Format("{0} -> {1}", p.Name, path);
                    p = p.parent;
                }
                return path;
            }
        }

        public string Name
        {
            get
            {
                return string.Format("{2}({0}, {1})", x, y, dir);
            }
        }

        public CellTypes Cell
        {
            get
            {
                return enviroment.GetCell(x, y);
            }
        }

        protected Enviroment enviroment;

        public List<Node> Children
        {
            get
            {
                List<Node> children = new List<Node>();

                // Up
                if (enviroment.GetCell(x, y - 1) != CellTypes.WALL) children.Add(new Node(x, y - 1, enviroment, Directions.UP, this));
                // Down
                if (enviroment.GetCell(x, y + 1) != CellTypes.WALL) children.Add(new Node(x, y + 1, enviroment, Directions.DOWN, this));
                // Left
                if (enviroment.GetCell(x - 1, y) != CellTypes.WALL) children.Add(new Node(x - 1, y, enviroment, Directions.LEFT, this));
                // Right
                if (enviroment.GetCell(x + 1, y) != CellTypes.WALL) children.Add(new Node(x + 1, y, enviroment, Directions.RIGHT, this));

                return children;
            }
        }

        public Node(int x, int y, Enviroment enviroment, Directions dir, Node parent)
        {
            this.x = x;
            this.y = y;
            this.enviroment = enviroment;
            this.dir = dir;
            this.parent = parent;
        }

        public bool EqualsPos(Node oNode)
        {
            if (x == oNode.x && y == oNode.y)
            {
                return true;
            }
            return false;
        }
    }
}
