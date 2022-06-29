using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SmallWorldPhenomenon
{
    class Normal
    {
        static Dictionary<string, Dictionary<string, List<string>>> edges = new Dictionary<string, Dictionary<string, List<string>>>();
        static StreamWriter o = new StreamWriter("output.txt");

        private static void ReadingQueriesFile(string filePath)
        {
            Stopwatch sw = Stopwatch.StartNew();
            foreach (var line in File.ReadAllLines(filePath))
            {
                string[] lineContent = line.Split('/');
                algorithm(lineContent[0], lineContent[1]);

            }
            sw.Stop();
            Console.Write("\nExcution Time = " + sw.Elapsed.TotalSeconds + " Seconds\n");
            Console.Write("\nExcution Time = " + sw.Elapsed.TotalMinutes + " Minutes\n");
            o.Close();
        }

        private static void ReadingTestFile(string filePath)
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                string[] lineContent = line.Split('/');
                for (int i = 1; i < lineContent.Length; i++)
                    FillDictionary(lineContent, i);
            }
        }

        private static void FillDictionary(string[] lineContent, int i)
        {
            edges.TryAdd(lineContent[i], new Dictionary<string, List<string>>());
            for (int j = 1; j < lineContent.Length; j++)
            {
                List<string> commonMovies = new List<string>();

                if (i != j)
                {
                    if (!edges[lineContent[i]].ContainsKey(lineContent[j]))
                    {
                        commonMovies.Add(lineContent[0]);
                        edges[lineContent[i]].Add(lineContent[j], commonMovies);
                    }
                    else
                    {
                        commonMovies = edges[lineContent[i]][lineContent[j]];
                        commonMovies.Add(lineContent[0]);
                        edges[lineContent[i]][lineContent[j]] = commonMovies;
                    }
                }
            }
        }

        public static void algorithm(string source, string distination)
        {
            if (edges[source].ContainsKey(distination))
            {
                o.Write(source + "/" + distination + "\nDoS = " + 1 + ", RS = " + edges[source][distination].Count 
                         + "\nCHAIN OF ACTORS: " + source + " -> " + distination + "\nCHAIN OF MOVIES: => " 
                         + edges[source][distination][0]);
                o.WriteLine('\n');
                return;
            }
            Dictionary<string, string> parent = new Dictionary<string, string>();
            Dictionary<string, int> level = new Dictionary<string, int>();
            Dictionary<string, int> costFromSource = new Dictionary<string, int>();

            Queue<string> Q = new Queue<string>();
            List<string> path = new List<string>();
            List<string> movies = new List<string>();


            foreach (string node in edges.Keys)
            {
                parent[node] = null;
                level[node] = -1;
                costFromSource[node] = 0;
            }

            level[source] = 0;
            costFromSource[source] = 0;
            Q.Enqueue(source);

            while (Q.Count != 0)
            {
                string curr = Q.Dequeue();
                foreach (string adj in edges[curr].Keys)
                {
                    if (level[adj] == level[curr])
                        continue;
                    else if (level[adj] == -1)
                    {
                        parent[adj] = curr;
                        level[adj] = level[curr] + 1;
                        costFromSource[adj] = costFromSource[curr] + edges[adj][curr].Count;
                        if (adj == distination)
                            break;
                        Q.Enqueue(adj);
                    }
                    else if (level[adj] > level[curr] && costFromSource[curr] + edges[adj][curr].Count > costFromSource[adj])
                    {
                        parent[adj] = curr;
                        level[adj] = level[curr] + 1;
                        costFromSource[adj] = costFromSource[curr] + edges[adj][curr].Count;
                    }
                }
            }

            string d = distination;

            while (distination != null)
            {
                path.Add(distination);
                string p = parent[distination];
                if (p != null)
                {
                    movies.Add(edges[distination][parent[distination]][0]);
                }
                distination = parent[distination];
            }
            o.Write(source + "/" + d + "\nDoS = " + level[d] + ", RS = " + costFromSource[d] + "\nCHAIN OF ACTORS: ");
            
            int i = path.Count - 1;
            while (i >= 0)
            {
                o.Write(path[i]);
                if (i != 0)
                    o.Write(" -> ");
                i--;
            }

            o.Write("\nCHAIN OF MOVIES: ");
            for (int k = movies.Count - 1; k > -1; k--)
            {
                o.Write(movies[k]);
                if (k != 0)
                    o.Write(" => ");
                else
                    o.Write(" =>");
            }
            o.WriteLine('\n');
        }

        public static void Start()
        {
            Console.WriteLine("Small-world Phenomenon:\n[1] Sample test cases\n[2] Complete tests\n");
            Console.Write("\nEnter your choice [1-2]: ");
            char choice = (char)Console.ReadLine()[0];
            switch (choice)
            {
                case '1':
                    ReadingTestFile(@"TestCases\Sample\movies1.txt");
                    ReadingQueriesFile(@"TestCases\Sample\queries1.txt");
                    break;

                case '2':
                    Console.WriteLine("\nComplete tests:\n[1]small\n[2]medium\n[3]large\n[4]extreme\n");
                    Console.Write("\nEnter your choice [1-4]: ");
                    char completeChoice = (char)Console.ReadLine()[0];
                    switch (completeChoice)
                    {
                        case '1':
                            Console.WriteLine("\nSmall Complete tests:\n[1]Case1\n[2]Case2\n");
                            Console.Write("\nEnter your choice [1-2]: ");
                            char completeSmall = (char)Console.ReadLine()[0];
                            switch (completeSmall)
                            {
                                case '1':
                                    ReadingTestFile(@"Testcases\Complete\small\Case1\Movies193.txt");
                                    ReadingQueriesFile(@"TestCases\Complete\small\Case1\queries110.txt");
                                    break;
                                case '2':
                                    ReadingTestFile(@"Testcases\Complete\small\Case2\Movies187.txt");
                                    ReadingQueriesFile(@"TestCases\Complete\small\Case2\queries50.txt");
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case '2':
                            Console.WriteLine("\nMedium Complete tests:\n[1]Case1\n[2]Case2\n");
                            Console.Write("\nEnter your choice [1-2]: ");
                            char completeMedium = (char)Console.ReadLine()[0];
                            switch (completeMedium)
                            {
                                case '1':
                                    ReadingTestFile(@"Testcases\Complete\medium\Case1\Movies967.txt");
                                    Console.WriteLine("\nMedium Complete tests:\n[1]Query 85\n[2]Query 4000\n");
                                    Console.Write("\nEnter your choice [1-2]: ");
                                    char queryMediumCase1 = (char)Console.ReadLine()[0];
                                    if (queryMediumCase1 == '1')
                                        ReadingQueriesFile(@"TestCases\Complete\medium\Case1\queries85.txt");
                                    else if (queryMediumCase1 == '2')
                                        ReadingQueriesFile(@"TestCases\Complete\medium\Case1\queries4000.txt");
                                    break;
                                case '2':
                                    ReadingTestFile(@"Testcases\Complete\medium\Case2\Movies4736.txt");
                                    Console.WriteLine("\nMedium Complete tests:\n[1]Query 110\n[2]Query 2000\n");
                                    Console.Write("\nEnter your choice [1-2]: ");
                                    char queryMediumCase2 = (char)Console.ReadLine()[0];
                                    if (queryMediumCase2 == '1')
                                        ReadingQueriesFile(@"TestCases\Complete\medium\Case2\queries110.txt");
                                    else if (queryMediumCase2 == '2')
                                        ReadingQueriesFile(@"TestCases\Complete\medium\Case2\queries2000.txt");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case '3':
                            ReadingTestFile(@"TestCases\Complete\large\Movies14129.txt");
                            Console.WriteLine("\nLarge Complete tests:\n[1]Query 26\n[2]Query 600\n");
                            Console.Write("\nEnter your choice [1-2]: ");
                            char completeLarge = (char)Console.ReadLine()[0];
                            switch (completeLarge)
                            {
                                case '1':
                                    ReadingQueriesFile(@"TestCases\Complete\large\queries26.txt");
                                    break;
                                case '2':
                                    ReadingQueriesFile(@"TestCases\Complete\large\queries600.txt");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case '4':
                            ReadingTestFile(@"TestCases\Complete\extreme\Movies122806.txt");
                            Console.WriteLine("\nExtreme Complete tests:\n[1]Query 22\n[2]Query 200\n");
                            Console.Write("\nEnter your choice [1-2]: ");
                            char completeExtreme = (char)Console.ReadLine()[0];
                            switch (completeExtreme)
                            {
                                case '1':
                                    ReadingQueriesFile(@"TestCases\Complete\extreme\queries22.txt");
                                    break;
                                case '2':
                                    ReadingQueriesFile(@"TestCases\Complete\extreme\queries200.txt");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
