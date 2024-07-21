using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Solving
{
    class Node
    {
        public List<Neighbour> Neighbours { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int nodeNumber { get; private set; }

        public int heuristic;
        public int parentNode;
        public bool visited;
        public int travelWeight;

        public int TreeParent;
        public int Degree;
        public int LastSibling;
        public int NextSibling;
        public int TreeChild;
        public bool loser;

        public Node(int X, int Y, int nodeNumber)
        {
            travelWeight = -1;
            parentNode = -1;
            this.X = X;
            this.Y = Y;
            visited = false;
            Neighbours = new List<Neighbour>();
            this.nodeNumber = nodeNumber;

            loser = false;
            TreeParent = -1;
            TreeChild = -1;
            LastSibling = -1;
            NextSibling = -1;
        }

        public void AddNeighbour(Node Neighbour, int Weight)
        {
            Neighbours.Add(new Neighbour(Neighbour, Weight));
            Neighbour.Neighbours.Add(new Neighbour(this, Weight));
        }

        public void Reset()
        {
            travelWeight = -1;
            parentNode = -1;
            heuristic = 0;
        }

        

    }
}
