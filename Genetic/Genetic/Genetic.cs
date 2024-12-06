using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Drawing;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Genetic
{
    public class Path
    {
        List<int> vertices;
        public Path() { vertices = new List<int>(); }
        public Path(int[] vertices) 
        {
            this.vertices = new List<int>(vertices.Length);
            for (int i = 0; i < vertices.Length; i++)
            {
                this.vertices.Add(vertices[i]);
            }
        }
        public Path(List<int> vertices) { this.vertices = vertices; }
        public int Size() { return vertices.Count; }
        public int this[int index]
        {
            get { return vertices[index]; }
            set { vertices[index] = value; }
        }
        public void AddVert(int x) 
        {
            vertices.Add(x);
        }
        public override string ToString()
        {
            string res = vertices[0].ToString();
            for (int i = 1; i < vertices.Count; i++)
            {
                res += " ";
                res += vertices[i].ToString();
            }
            return res;
        }
        public bool Equals(Path other)
        {
            if (other == null) return false;
            for (int i = 0; i < vertices.Count;i++)
            {
                if (other.vertices[i] != vertices[i]) 
                    return false;
            }
            return true;
        }

    }
    public class Population
    {
        List<Path> population;
        public Population(List<Path> population)
        {
            this.population = population;
        }
        public Population()
        {
            this.population = new List<Path>();
        }
        public void AddGen(Path path)
        {
            this.population.Add(path);
        }
        public int GetSize() { return this.population.Count; }
        public Path this[int index]
        {
            get { return this.population[index]; }
            set { this.population[index] = value; }
        }
    }
    public class Genetic
    {
        Population pop;
        double[,] graph;
        int graph_size;
        public Genetic(double[,] graph)
        {
            pop = new Population();
            this.graph = graph;
            graph_size = graph.GetLength(0);
        }
        public Genetic() 
        {
            this.pop = new Population();
            graph = new double[0,0];
            graph_size = 0;
        }
        public Path Calculate(CancellationToken cts, int max_pop = 100, int gen_count = 100)
        {
            Random rnd = new Random();
            int start_pop = max_pop / 2;
            for (int i = 0; i < start_pop; i++) 
            {
                pop.AddGen(this.GenRndPath());
            }
            for(int i = 0; i < gen_count; i++)
            {
                //if (Console.KeyAvailable)
                //{
                //    Console.ReadKey(true);
                //    break;
                //}
                var bestPath = this.GetBestPath();
                //var new_pop = new Population();
                //new_pop.AddGen(bestPath);
                ConcurrentBag<Path> new_pop = new ConcurrentBag<Path>();
                new_pop.Add(bestPath);
                Parallel.For(0, max_pop, j =>
                {
                    var parnt1 = pop[rnd.Next(pop.GetSize())];
                    var parnt2 = pop[rnd.Next(pop.GetSize())];
                    while (parnt1.Equals(parnt2))
                    {
                        parnt2 = pop[rnd.Next(pop.GetSize())];
                    }
                    var child1 = Crossover(parnt1, parnt2);
                    var child2 = Crossover(parnt2, parnt1);
                    new_pop.Add(Mutate(child1));
                    new_pop.Add(Mutate(child2));
                    Console.WriteLine(i);
                });
                List<Path> tmp = new List<Path>(new_pop);
                var new_pop_ = new Population(tmp);
                pop = Survive(new_pop_, max_pop);
                if (cts.IsCancellationRequested)
                {
                    return GetBestPath();
                }

            }
            return GetBestPath();

        }
        public Population Survive(Population population, int max_pop)
        {
            List<double> fitness = new List<double>();
            for (int i = 0; i < population.GetSize(); ++i)
            {
                fitness.Add(Metric(population[i]));
            }
            var result = new Population();
            while(result.GetSize() < max_pop)
            {
                int index = fitness.IndexOf(fitness.Max());
                fitness[index] = 0;
                result.AddGen(population[index]);
            }
            return result;
        }

        public double Metric(Path path)
        {
            if (path == null) return 0;
            double result = 0;
            for (int i = 0; i < graph_size - 1; ++i)
            {
                result += graph[path[i], path[i + 1]];
            }
            return 1 / result;
        }
        public Path GetBestPath()
        {
            var path = pop[0];
            double max = Metric(path);
            for (int i = 1; i < pop.GetSize(); ++i)
            {
                if (Metric(pop[i]) > max) {
                    max = Metric(pop[i]);
                    path = pop[i];
                }
            }
            if (path == null) return GetBestPath();
            return path;

        }
        public Path GenRndPath()
        {
            Random rnd = new Random();
            List<int> path = new List<int>();
            while (path.Count < graph_size)
            {
                var tmp = rnd.Next(graph_size);
                if (!path.Contains(tmp))
                {
                    path.Add(tmp);
                }
            }
            if (new Path(path) == null) return GenRndPath();
            return new Path(path);
        }
        public Path Mutate(Path path)
        {
            Random rnd = new Random();
            int i = rnd.Next(path.Size());
            int j = rnd.Next(path.Size());
            int tmp = path[i];
            path[i] = path[j];
            path[j] = tmp;
            return path;
        }

        public Path Crossover(Path path1, Path path2) 
        {
            int N = path1.Size();
            Random rnd = new Random();
            int index = rnd.Next(path1.Size());
            List<int> child = new List<int>();
            for (int i = 0; i < N; i++)
            {
                child.Add(path1[i]);
            }
            int j = 0;
            while (child.Count < N)
            {
                if (!child.Contains(path2[j]))
                {
                    child.Add(path2[j]);
                }
                j++;
            }
            return new Path(child);
        }
    }
}