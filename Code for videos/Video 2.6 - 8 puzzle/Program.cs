using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_2._6___8_puzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Video 2.6");
            Console.WriteLine();

            string state = PuzzleSolver.GenerateRandomSolvableState();

            Console.WriteLine("Solving " + PuzzleSolver.PrintableState(state));

            Stopwatch s = new Stopwatch();
            s.Start();
            List<string> path = PuzzleSolver.DepthFirstSearch(state);
            s.Stop();

            Console.WriteLine("Depth-first search took " + s.ElapsedMilliseconds + " ms");
            Console.WriteLine("Depth-first search path contains " + path.Count + " states");
            Console.WriteLine();

            s.Reset();
            s.Start();
            path = PuzzleSolver.BreadthFirstSearch(state);
            s.Stop();

            Console.WriteLine("Breadth-first search took " + s.ElapsedMilliseconds + " ms");
            Console.WriteLine("Breadth-first search path contains " + path.Count + " states");
            Console.WriteLine();

            Console.WriteLine("Printing solution");
            foreach (string p in path)
            {
                Console.WriteLine(PuzzleSolver.PrintableState(p));
            }

            while (true) ;
        }
    }
}
