using System;
using Genetic;

public class Program
{
    static void Main(string[] args)
    {
        double[,] graph =
        {
            {0, 1, 100, 100, 100, 100 },
            {1, 0, 1, 100, 100, 100 },
            {100, 1, 0, 1, 100, 100 },
            {100, 100, 1, 0, 1, 100 },
            {100, 100, 100, 1, 0, 150 },
            {100, 100, 100, 100, 150, 0 }
        };
        Console.WriteLine("write number of cities(if want to use default write \"0\"");
        int cities_num = Int32.Parse(Console.ReadLine());
        if(cities_num == 0 )
        {
            Genetic.Genetic gen = new Genetic.Genetic(graph);
            Console.WriteLine(gen.Calculate(50, 5));
        }
        else {
            Console.WriteLine("write graph number <-> Line:");
            double[,] roads = new double[cities_num, cities_num];
            for (int i = 0; i <  cities_num; i++)
            {
                for (int j = 0; j < cities_num; j++)
                {
                    roads[i, j] = double.Parse(Console.ReadLine());
                }
            }
            Console.WriteLine("write maximum population size:");
            int max_pop = Int32.Parse(Console.ReadLine());
            Console.WriteLine("write maximum generation count:");
            int gen_count = Int32.Parse(Console.ReadLine());
            Genetic.Genetic gen = new Genetic.Genetic(roads);
            Console.WriteLine(gen.Calculate(max_pop, gen_count));
        }
        
    }
} 