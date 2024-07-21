using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Solving
{
    class Neighbour
    {
        public Node Node { get; private set; }
        public int Weight { get; private set; }

        public Neighbour(Node Node, int Weight)
        {
  
            this.Node = Node;
            this.Weight = Weight;
        }
    }
}
