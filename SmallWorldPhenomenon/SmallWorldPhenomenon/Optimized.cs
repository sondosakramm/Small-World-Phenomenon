using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SmallWorldPhenomenon
{
    class Optimized
    {
        static Dictionary<string, int> getActorIndex = new Dictionary<string, int>();
        static List<int> actorsAsIndex = new List<int>();
        static List<string> namesOfActors = new List<string>();
        static List<string> namesOfMovies = new List<string>();
        static List<Dictionary<int, KeyValuePair<int, int>>> adjacentsForEachActor = new List<Dictionary<int, KeyValuePair<int, int>>>();
        static StreamWriter o = new StreamWriter("output.txt"); 
        static int numOfActors = 0;
        static int numOfMovies = 0;

        private static void ReadingTestFile(string filePath)
        {
            using (var f = File.OpenRead(filePath))
            using (var reader = new StreamReader(f))
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    string[] lineContent = line.Split('/');
                    namesOfMovies.Add(lineContent[0]);
                    for (int i = 1; i < lineContent.Length; i++)
                    {
                        if (!getActorIndex.ContainsKey(lineContent[i]))
                        {
                            namesOfActors.Add(lineContent[i]);
                            getActorIndex.Add(lineContent[i], numOfActors);
                            actorsAsIndex.Add(numOfActors);
                            adjacentsForEachActor.Add(new Dictionary<int, KeyValuePair<int, int>>());
                            numOfActors++;
                        }
                    }
                    numOfMovies++;

                    for (int j = 1; j < lineContent.Length; j++)
                    {
                        int actorIndex = getActorIndex[lineContent[j]];
                        for (int k = 1; k < lineContent.Length; k++)
                        {
                            if (k != j)
                            {
                                int adjActorIndex = getActorIndex[lineContent[k]];

                                if (!adjacentsForEachActor[actorIndex].ContainsKey(adjActorIndex))
                                {
                                    adjacentsForEachActor[actorIndex][adjActorIndex] = new KeyValuePair<int, int>(numOfMovies - 1, 1);

                                }
                                else
                                {
                                    var pair = adjacentsForEachActor[actorIndex][adjActorIndex];
                                    adjacentsForEachActor[actorIndex][adjActorIndex] = new KeyValuePair<int, int>(pair.Key, pair.Value + 1);
                                }

                            }
                        }
                    }
                }
        }

        private static void ReadingQueriesFile(string filePath)
        {
            Stopwatch sw = Stopwatch.StartNew();
            using (var f = File.OpenRead(filePath))
            using (var reader = new StreamReader(f))
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    string[] lineContent = line.Split('/');
                    int sourceIndex = getActorIndex[lineContent[0]];
                    int destinationIndex = getActorIndex[lineContent[1]];
                    //Calling The Algorithm
                    BFSAlgorithm(sourceIndex, destinationIndex);
                }
            sw.Stop();
            Console.Write("\nExcution Time = " + sw.Elapsed.TotalSeconds + " Seconds\n");
            Console.Write("\nExcution Time = " + sw.Elapsed.TotalMinutes + " Minutes\n");
            o.Close();
        }

        private static void BFSAlgorithm(int source, int destination)
        {

            Queue<int> Q = new Queue<int>();
            List<string> path = new List<string>();
            List<string> movies = new List<string>();
            int numberOfNodes = actorsAsIndex.Count;

            int[] parent = new int[numberOfNodes];
            Array.Fill(parent, -10);
            int[] level = new int[numberOfNodes];
            Array.Fill(level, -1);
            int[] costFromSource = new int[numberOfNodes];
            Array.Fill(costFromSource, 0);
            level[source] = 0;
            parent[source] = -1;

            Q.Enqueue(source);
            while (Q.Count != 0)
            {
                int currNode = Q.Dequeue();

                if (level[destination] == level[currNode] && level[destination] != -1)
                    break;

                foreach (int adj in adjacentsForEachActor[currNode].Keys)
                {
                    int currLevelAdj = level[currNode] + 1;

                    if (level[adj] == level[currNode])
                        continue;

                    int currCost = adjacentsForEachActor[currNode][adj].Value;
                    int currTotalCost = costFromSource[currNode] + currCost;

                    if (parent[adj] == -10)
                    {
                        parent[adj] = currNode;
                        level[adj] = currLevelAdj;
                        costFromSource[adj] = currTotalCost;
                        if (adj == destination)
                            break;
                        Q.Enqueue(adj);
                    }
                    else if (level[adj] > level[currNode] && currTotalCost > costFromSource[adj])
                    {
                        parent[adj] = currNode;
                        level[adj] = currLevelAdj;
                        costFromSource[adj] = currTotalCost;
                    }
                }
            }

            int actualDestination = destination;

            while (destination != -1)
            {
                path.Add(namesOfActors[destination]);
                int p = parent[destination];
                if (p != -1)
                {
                    int movieNum = adjacentsForEachActor[destination][parent[destination]].Key;
                    movies.Add(namesOfMovies[movieNum]);
                }
                destination = parent[destination];
            }

            o.Write(namesOfActors[source] + "/" + namesOfActors[actualDestination] +
            "\nDoS = " + level[actualDestination] + ", RS = " + costFromSource[actualDestination] +
            "\nCHAIN OF ACTORS: ");

            int i = path.Count - 1;
            while (i >= 0)
            {
                o.Write(path[i]);
                if (i != 0)
                    o.Write(" -> ");
                i--;
            }

            o.Write("\nCHAIN OF MOVIES:  => ");
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
                    Console.WriteLine("\n Complete tests:\n[1] small\n[2] medium\n[3] large\n[4] extreme\n");
                    Console.Write("\nEnter your choice [1-4]: ");
                    char completeChoice = (char)Console.ReadLine()[0];
                    switch (completeChoice)
                    {
                        case '1':
                            Console.WriteLine("\nSmall Complete tests:\n[1] Case1\n[2] Case2\n");
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
                            Console.WriteLine("\nMedium Complete tests:\n[1] Case1\n[2] Case2\n");
                            Console.Write("\nEnter your choice [1-2]: ");
                            char completeMedium = (char)Console.ReadLine()[0];
                            switch (completeMedium)
                            {
                                case '1':
                                    ReadingTestFile(@"Testcases\Complete\medium\Case1\Movies967.txt");
                                    Console.WriteLine("\nMedium Complete tests:\n[1] Query 85\n[2] Query 4000\n");
                                    Console.Write("\nEnter your choice [1-2]: ");
                                    char queryMediumCase1 = (char)Console.ReadLine()[0];
                                    if (queryMediumCase1 == '1')
                                        ReadingQueriesFile(@"TestCases\Complete\medium\Case1\queries85.txt");
                                    else if (queryMediumCase1 == '2')
                                        ReadingQueriesFile(@"TestCases\Complete\medium\Case1\queries4000.txt");
                                    break;
                                case '2':
                                    ReadingTestFile(@"Testcases\Complete\medium\Case2\Movies4736.txt");
                                    Console.WriteLine("\nMedium Complete tests:\n[1] Query 110\n[2] Query 2000\n");
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
                            Console.WriteLine("\nLarge Complete tests:\n[1] Query 26\n[2] Query 600\n");
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
                            Console.WriteLine("\nExtreme Complete tests:\n[1] Query 22\n[2] Query 200\n");
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
            }
        }
    }
}
