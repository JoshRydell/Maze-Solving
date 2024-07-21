using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace Maze_Solving
{
    class SolveMaze
    {
        
        private List<Node> graph;
        string mazePath;
        private int tileWidth;
        public SolveMaze(string mazePath)
        {
            this.mazePath = mazePath;
            Bitmap img = new Bitmap(mazePath);
            bool[,] maze;
            Node[,] nodeMap;
            Size mazeSize = new Size();
            tileWidth = 0;
            graph = new List<Node>();


            for (int i = 0; i < img.Width; i++)
            {

                if (img.GetPixel(i, 0) == Color.FromArgb(255, 255, 255, 255))
                {
                    tileWidth++;
                }
            }
            mazeSize.Width = img.Width / tileWidth;
            mazeSize.Height = img.Height / tileWidth;
            maze = new bool[mazeSize.Width, mazeSize.Height];
            nodeMap = new Node[mazeSize.Width, mazeSize.Height];

            for (int y = 0; y < mazeSize.Height; y++)
            {
                for (int x = 0; x < mazeSize.Width; x++)
                {

                    if (img.GetPixel(x * tileWidth, y * tileWidth) == Color.FromArgb(255, 255, 255, 255))
                    {
                        maze[x, y] = true;
                    }
                    else
                    {
                        maze[x, y] = false;
                    }
                }
            }

            for (int x = 0; x < mazeSize.Width; x++)
            {
                if (maze[x, 0])
                {
                    Node newNode = new Node(x, 0, graph.Count);
                    graph.Add(newNode);
                    nodeMap[x, 0] = newNode;
                }
            }
            for (int y = 1; y < mazeSize.Height - 1; y++)
            {
                for (int x = 1; x < mazeSize.Width - 1; x++)
                {

                    if (maze[x, y])
                    {
                        bool verticalPath = maze[x, y + 1] && maze[x, y - 1];
                        bool horizontalPath = maze[x - 1, y] && maze[x + 1, y];
                        bool tJunction = verticalPath && (maze[x - 1, y] ^ maze[x + 1, y]) || horizontalPath && (maze[x, y - 1] ^ maze[x, y + 1]);
                        if (!(verticalPath ^ horizontalPath) || tJunction)
                        {

                            Node newNode = new Node(x, y, graph.Count);
                            nodeMap[x, y] = newNode;
                            graph.Add(newNode);

                            int i = x - 1;
                            while (i >= 0 && nodeMap[i, y] == null && maze[i, y])
                            {
                                i--;
                            }
                            if (maze[i, y] && nodeMap[i, y] != null)
                            {
                                int weight = x - i;
                                newNode.AddNeighbour(nodeMap[i, y], weight);

                            }

                            i = y - 1;
                            while (i >= 0 && nodeMap[x, i] == null && maze[x, i])
                            {
                                i--;
                            }
                            if (maze[x, i] && nodeMap[x, i] != null)
                            {
                                int weight = y - i;
                                newNode.AddNeighbour(nodeMap[x, i], weight);
                            }

                        }


                    }
                }
                
            }

            for (int x = 0; x < mazeSize.Width; x++)
            {
                if (maze[x, mazeSize.Height - 1])
                {
                    Node newNode = new Node(x, mazeSize.Height - 1, graph.Count);
                    graph.Add(newNode);
                    nodeMap[x, mazeSize.Height - 1] = newNode;
                    int i = mazeSize.Height - 2;
                    while (i >= 0 && nodeMap[x, i] == null && maze[x, i])
                    {
                        i--;
                    }
                    if (maze[x, i] && nodeMap[x, i] != null)
                    {
                        int weight = mazeSize.Height - (i + i);
                        newNode.AddNeighbour(nodeMap[x, i], weight);

                    }
                }
            }

            Console.WriteLine("Graph generated");
            Console.WriteLine("Number of nodes: " + graph.Count.ToString());
            Console.WriteLine();

        }

        public void Dijkstra(string savePath)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            Node currentNode = graph[0];
            currentNode.travelWeight = 0;
            Heap h = new Heap(graph.ToArray());
            currentNode = h.PopMin();

            int finalNodeNum = graph.Count - 1;

            while (finalNodeNum != currentNode.nodeNumber)
            {
                currentNode.visited = true;
                for (int i = 0; i < currentNode.Neighbours.Count; i++)
                {
                    if (!currentNode.Neighbours[i].Node.visited && (currentNode.Neighbours[i].Node.travelWeight == -1 || currentNode.Neighbours[i].Weight + currentNode.travelWeight < currentNode.Neighbours[i].Node.travelWeight))
                    {
                        currentNode.Neighbours[i].Node.parentNode = currentNode.nodeNumber;
                        h.decreaseKey(currentNode.Neighbours[i].Node.nodeNumber, currentNode.Neighbours[i].Weight + currentNode.travelWeight);
                    }
                }

                currentNode = h.PopMin();


            }
            sp.Stop();
            Console.WriteLine();
            Console.WriteLine("Dijkstra completed");
            Console.WriteLine("Time taken: " + sp.ElapsedMilliseconds.ToString() + " milliseconds");
            Draw(savePath);


        }

        private void Draw(string savePath)
        {
            Node currentNode = graph[graph.Count - 1];
            Bitmap solved = new Bitmap(mazePath);
            Graphics g = Graphics.FromImage(solved);
            Brush pathColour = Brushes.Red;
            Brush terminalColour = Brushes.Green;
            while (currentNode.nodeNumber != 0)
            {

                Node parentNode = graph[currentNode.parentNode];
                int[] Vector = new int[2] { parentNode.X - currentNode.X, parentNode.Y - currentNode.Y };
                Vector[0] /= Vector[0] == 0 ? 1 : Math.Abs(Vector[0]);
                Vector[1] /= Vector[1] == 0 ? 1 : Math.Abs(Vector[1]);
                int i = 0;
                while (!(i * Vector[0] + currentNode.X == parentNode.X && i * Vector[1] + currentNode.Y == parentNode.Y))
                {
                    g.FillRectangle(pathColour, (i * Vector[0] + currentNode.X) * tileWidth, (i * Vector[1] + currentNode.Y) * tileWidth, tileWidth, tileWidth);
                    i++;
                }

                currentNode = graph[currentNode.parentNode];
            }


            g.FillRectangle(terminalColour, currentNode.X * tileWidth, currentNode.Y * tileWidth, tileWidth, tileWidth);
            g.FillRectangle(terminalColour, graph[graph.Count - 1].X * tileWidth, graph[graph.Count - 1].Y * tileWidth, tileWidth, tileWidth);
            solved.Save(savePath);

            for (int i = 0; i < graph.Count; i++)
            {
                graph[i].Reset();
            }
        }

        public void A_Star(string savePath)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            Heap heap = new Heap(graph.Count);
            graph[0].travelWeight = 0;
            heap.AddNode(graph[0]);
            Node currentNode = heap.PopMin();
            int finalNodeNum = graph.Count - 1;


            while (finalNodeNum != currentNode.nodeNumber)
            {
                currentNode.visited = true;
                for (int i = 0; i < currentNode.Neighbours.Count; i++)
                {
                    currentNode.Neighbours[i].Node.heuristic = Math.Abs(currentNode.Neighbours[i].Node.X - graph[finalNodeNum].X) + Math.Abs(currentNode.Neighbours[i].Node.Y - graph[finalNodeNum].Y);
                    if (!currentNode.Neighbours[i].Node.visited && (currentNode.Neighbours[i].Node.travelWeight == -1 || currentNode.Neighbours[i].Weight + currentNode.Neighbours[i].Node.heuristic + currentNode.travelWeight < currentNode.Neighbours[i].Node.travelWeight))
                    {
                        currentNode.Neighbours[i].Node.parentNode = currentNode.nodeNumber;
                        currentNode.Neighbours[i].Node.travelWeight = currentNode.Neighbours[i].Weight + currentNode.travelWeight + currentNode.Neighbours[i].Node.heuristic;
                        heap.AddNode(currentNode.Neighbours[i].Node);
                    }
                }

                //int smallestTravelWeight = -1;
                //int nextNode = -1;
                //for (int i = 0; i < graph.Count; i++)
                //{
                //    if (!graph[i].visited && (smallestTravelWeight == -1 || (smallestTravelWeight > graph[i].travelWeight && graph[i].travelWeight != -1)))
                //    {
                //        smallestTravelWeight = graph[i].travelWeight;
                //        nextNode = i;
                //    }
                //}

                currentNode = heap.PopMin();
            }
            sp.Stop();
            Console.WriteLine();
            Console.WriteLine("A* completed");
            Console.WriteLine("Time taken: " + sp.ElapsedMilliseconds.ToString() + " milliseconds");
            Draw(savePath);

        }

        public void DepthFirst(string savePath)
        {
            DepthFirstRecurse(graph[0]);
            Draw(savePath);

            Console.WriteLine("Depth First traversal completed");
        }

        private void DepthFirstRecurse(Node node)
        {
            node.visited = true;
            for(int i = 0; i < node.Neighbours.Count;i++)
            {
                if(!node.Neighbours[i].Node.visited)
                {
                    node.Neighbours[i].Node.parentNode = node.nodeNumber;
                    DepthFirstRecurse(node.Neighbours[i].Node);
                }
            }
        }
    }
}
