using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Solving
{
    class Heap
    {
        private Node[] Nodes;
        private int min;
        int freeSpace = 0;
        public Heap(Node[] Nodes)
        {
            min = -1;
            this.Nodes = Nodes;
            freeSpace = Nodes.Length;
            for(int i = 0; i < Nodes.Length;i++)
            {
                Nodes[i].NextSibling = (i + 1) % Nodes.Length;
                Nodes[i].LastSibling = (i - 1 + Nodes.Length) % Nodes.Length;
                if(min == -1 || Nodes[i].travelWeight > -1 && Nodes[min].travelWeight > Nodes[i].travelWeight)
                {
                    min = i;
                }
            }
        }

        public Heap(int size)
        {
            min = -1;
            Nodes = new Node[size];
        }


        public Node PopMin()
        {
            int oldMin = min;
            List<int> vertexToAdd = new List<int>();
            if (Nodes[oldMin].TreeChild != -1)
            {
                int addVertex = Nodes[oldMin].TreeChild;
                do
                {
                    vertexToAdd.Add(addVertex);
                    addVertex = Nodes[addVertex].NextSibling;
                } while (addVertex != Nodes[oldMin].TreeChild);
            }
            for(int i = 0; i < vertexToAdd.Count; i++)
            {
                AddChild(-1, vertexToAdd[i]);
            }
            Nodes[Nodes[oldMin].LastSibling].NextSibling = Nodes[oldMin].NextSibling;
            Nodes[Nodes[oldMin].NextSibling].LastSibling = Nodes[oldMin].LastSibling;
            

            int[] degree = new int[Nodes.Length];
            for (int i = 0; i < degree.Length; i++)
            {
                degree[i] = -1;
            }

            int startNode = Nodes[min].NextSibling;
            int currentNode = Nodes[min].NextSibling;
            

            do
            {
                int x = currentNode;
                while (degree[Nodes[x].Degree] != -1 && degree[Nodes[x].Degree] != currentNode)
                {
                    int u = degree[Nodes[x].Degree];
                    degree[Nodes[x].Degree] = -1;
                    x = Merge(x, u);
                    startNode = x;
                }
                degree[Nodes[x].Degree] = x;
                currentNode = Nodes[x].NextSibling;
            } while (currentNode != startNode);


            startNode = currentNode;
            min = -1;
            do
            {

                if (min == -1 || Nodes[currentNode].travelWeight != -1 && ( Nodes[min].travelWeight > Nodes[currentNode].travelWeight || Nodes[min].travelWeight == -1 ))
                {
                    min = currentNode;
                }
                currentNode = Nodes[currentNode].NextSibling;

            } while (currentNode != startNode);

            return Nodes[oldMin];
        }

        private void getDegree(int root)
        {
            Nodes[root].Degree = 0;
            if (Nodes[root].TreeChild != -1)
            {
                int nextNode = Nodes[root].TreeChild;
                do
                {
                    nextNode = Nodes[nextNode].NextSibling;
                    Nodes[root].Degree++;
                } while (nextNode != Nodes[root].TreeChild);
            }
        }

        public void decreaseKey(int index, int value)
        { 
            Nodes[index].travelWeight = value;
            if(Nodes[index].TreeParent != -1 && (Nodes[Nodes[index].TreeParent].travelWeight > Nodes[index].travelWeight || Nodes[Nodes[index].TreeParent].travelWeight == -1))
            {
                if(Nodes[Nodes[index].TreeParent].loser && Nodes[Nodes[index].TreeParent].TreeParent != -1)
                {
                    AddChild(-1, Nodes[index].TreeParent);
                    Nodes[Nodes[index].TreeParent].loser = !Nodes[Nodes[index].TreeParent].loser;
                }
                
                AddChild(-1, index);
            }

            if(min == -1 || Nodes[index].travelWeight < Nodes[min].travelWeight && Nodes[index].travelWeight != -1|| (Nodes[index].travelWeight != -1 && Nodes[min].travelWeight == -1))
            {
                min = index;
            }
        }

        private void AddChild(int Parent, int newChild)
        {
            if(Nodes[newChild].TreeParent != -1 && Nodes[newChild].NextSibling == newChild)
            {
                Nodes[Nodes[newChild].TreeParent].TreeChild = -1;
            }
            else if(Nodes[newChild].NextSibling != -1)
            {
                Nodes[Nodes[newChild].NextSibling].LastSibling = Nodes[newChild].LastSibling;
                Nodes[Nodes[newChild].LastSibling].NextSibling = Nodes[newChild].NextSibling;
                if(Nodes[newChild].TreeParent!= -1)
                {
                    Nodes[Nodes[newChild].TreeParent].TreeChild = Nodes[newChild].NextSibling;
                }
                
            }

            Nodes[newChild].TreeParent = Parent;

            int nextNode = Parent == -1 ? min : Nodes[Parent].TreeChild;
            {
                if (nextNode == -1)
                {
                    if(Parent != -1)
                    {
                        Nodes[Parent].TreeChild = newChild;
                    }
                    else
                    {
                        min = newChild;
                    }

                    Nodes[newChild].NextSibling = newChild;
                    Nodes[newChild].LastSibling = newChild;
                }
                else
                {
                    if (Parent != -1)
                    {
                        Nodes[Parent].TreeChild = newChild;

                    }
                    int temp = Nodes[nextNode].LastSibling;
                    Nodes[newChild].NextSibling = nextNode;
                    Nodes[Nodes[nextNode].LastSibling].NextSibling = newChild;
                    Nodes[nextNode].LastSibling = newChild;
                    Nodes[newChild].LastSibling = temp;
                    if (min == -1 || Nodes[min].travelWeight > Nodes[newChild].travelWeight && Nodes[newChild].travelWeight > -1)
                    {
                        min = newChild;
                    }
                    if(Parent == -1)
                    {
                        Nodes[newChild].loser = false;
                    }

                }
                if (Parent == -1)
                {
                    getDegree(newChild);
                }
                else
                {
                    getDegree(Parent);
                }
            }
        }

        public void AddNode(Node Node)
        {
            if(freeSpace >= Nodes.Length)
            {
                throw new Exception("Heap Full");
            }
            Nodes[freeSpace] = Node;
            AddChild(-1, freeSpace);
            freeSpace++;
        }
        private int Merge(int Vertex1, int Vertex2)
        {
            
            if(Nodes[Vertex1].travelWeight > Nodes[Vertex2].travelWeight)
            {
                int temp = Vertex1;
                Vertex1 = Vertex2;
                Vertex2 = temp;
            }
            if (Nodes[Vertex1].travelWeight < Nodes[Vertex2].travelWeight && Nodes[Vertex1].travelWeight != -1 )
            {
                AddChild(Vertex1, Vertex2);
                getDegree(Vertex1);
                return Vertex1;
            }
            else
            {
                AddChild(Vertex2, Vertex1);
                getDegree(Vertex2);
                return Vertex2;
            }
        }
    }

}
