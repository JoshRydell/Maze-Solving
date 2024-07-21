using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Maze_Solving
{
    class Program
    {
        static void Main(string[] args)
        {
            string mazeName;
            int algorithm;
            string savePath;
            string readPath;

            Console.WriteLine("Enter maze name to solve: ");
            mazeName = Console.ReadLine();
            Console.WriteLine();
            readPath = Directory.GetCurrentDirectory() + "\\" + mazeName + ".bmp"; 

            SolveMaze sm = new SolveMaze(readPath);

            Console.WriteLine("Apply: ");
            Console.WriteLine("(1) Dijkstra");
            Console.WriteLine("(2) A*");
            Console.WriteLine("(3) Depth First");
            algorithm = Convert.ToInt32(Console.ReadLine());

            if(algorithm == 1)
            {
                savePath = Directory.GetCurrentDirectory() + "\\" + mazeName + " solved with Dijkstra.bmp";
                sm.Dijkstra(savePath);
            }
            else if(algorithm == 2)
            {
                savePath = Directory.GetCurrentDirectory() + "\\" + mazeName + " solved with A Star.bmp";
                sm.A_Star(savePath);

            }
            else if (algorithm == 3)
            {
                savePath = Directory.GetCurrentDirectory() + "\\" + mazeName + " solved with Depth First traversal.bmp";
                sm.DepthFirst(savePath);
            }

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
    }
}
