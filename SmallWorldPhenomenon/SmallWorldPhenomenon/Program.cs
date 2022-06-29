using System;
using System.IO;

namespace SmallWorldPhenomenon
{
    class Program
    {
        public static void checkingAnswers(string answerPath, string outputPath)
        {
             
        }
        static void Main(string[] args)
        {
            Console.WriteLine("\nSmall-world Phenomenon:\n[1] Normal Solution\n[2] Optimized Solution\n");
            Console.Write("\nEnter your choice [1-2]: ");
            char choice = (char)Console.ReadLine()[0];
            if (choice == '1')
                Normal.Start();
            else if (choice == '2')
                Optimized.Start();
            else
                return;
            checkingAnswers(@"Testcases\Complete\extreme\Solutions\queries22 - Solution.txt", "output.txt");
        }
    }
}
