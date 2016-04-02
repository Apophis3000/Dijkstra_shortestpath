using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dijkstra_shortestpath
{
    public class Vertex
    {
        private int nodeId;
        public Vertex(int id)
        {
            nodeId = id;
        }

        public int NodeId
        {
            get { return nodeId; }
        }
    }
    public class Edge
    {
        private int vertex1, vertex2;
        private int weight;

        public Edge(int v1, int v2, int w)
        {
            vertex1 = v1;
            vertex2 = v2;
            weight = w;
        }

        public int Weight
        {
            get { return weight; }
        }
        public int Vertex1
        {
            get { return vertex1; }
        }

        public int Vertex2
        { 
            get { return vertex2; }
        }
    }

    public class Graph
    {
        private const int infinity = 999999999;
        private List<Vertex> vertices;
        private List<Edge> edges;
        public Graph(List<Vertex> v, List<Edge> e)
        {
            vertices = v;
            edges = e;
        }
        
        public List<Vertex> Vertices
        {
            get { return vertices; }
        }

        public Vertex GetVertex(int i)
        {
            foreach (Vertex v in vertices)
            {
                if (v.NodeId == i)
                    return v;
            }
            return null;
        }

        public Vertex GetDistance(int i)
        {
            return vertices.First(x => x.NodeId == i);
        }

        public List<Vertex> GetNeighbors(int nodeId)
        {
            List<Vertex> neighbors = new List<Vertex>();
            foreach (Edge e in edges)
            {
                if (e.Vertex1 == nodeId)
                {
                    
                    neighbors.Add(vertices.First(x => x.NodeId == e.Vertex2));
                }
            }
            return neighbors;
        }

        public int GetDistanceBetween(int node1, int node2)
        {
            foreach (Edge e in edges)
            {
                if (e.Vertex1 == node1 && e.Vertex2 == node2)
                    return e.Weight;
            }
            return 0;
        }

        public Dictionary<int, int> Dijkstra(int src)
        {
            Dictionary<int, int> distances = new Dictionary<int, int>();
            Dictionary<int, int> previous = new Dictionary<int, int>();
            List<Vertex> unvisited = new List<Vertex>();

            foreach (Vertex v in vertices)
            {
                unvisited.Add(v);
                distances.Add(v.NodeId, infinity);
                previous.Add(v.NodeId, -1);
            }

            distances[src] = 0;

            while (unvisited.Count > 0)
            {                
                int id = src;
                if (!unvisited.Contains(GetVertex(id)))
                    id = 1;
                while (!unvisited.Contains(GetVertex(id)))
                    id++;
                int prev = distances[id];

                foreach (Vertex v in unvisited)
                {
                    if (distances[v.NodeId] < prev)
                        prev = distances[v.NodeId];
                }

                unvisited.Remove(GetVertex(id));

                if (distances[id] == infinity)
                    break;

                int alt;
                foreach (Vertex v in GetNeighbors(id))
                {
                    alt = distances[id] + GetDistanceBetween(id, v.NodeId);
                    if (alt < distances[v.NodeId])
                    {
                        distances[v.NodeId] = alt;
                        previous[v.NodeId] = id;
                    }
                }

            }

            for (int i = 1; i <= distances.Count; i++)
            {
                Console.WriteLine("Kürzester Pfad von {0} zu {1} {2} : {3}", src, i, getPath(previous, i), distances[i]);
            }

            return previous;
        }

        private string getPath(Dictionary<int, int> dist, int i)
        {
            if (!dist.ContainsKey(i))
                return String.Empty;
            return getPath(dist, dist.First(x => x.Value == dist[i]).Value) + String.Format("-> {0} ", i);
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            List<Vertex> vertices = new List<Vertex>();
            for (int i = 0; i < 25; i++)
            {
                vertices.Add(new Vertex(i+1));
            }          

            List<Edge> edges = new List<Edge>();

            int max = vertices.Count;

            foreach (Vertex v in vertices)
            {
                if (v.NodeId + 1 < max)
                {
                    Vertex v2 = vertices.First(x => x.NodeId == v.NodeId+1);
                    edges.Add(new Edge(v.NodeId, v2.NodeId, 1));
                }

                if (v.NodeId + 5 < max)
                {
                    Vertex v3 = vertices.First(x => x.NodeId == v.NodeId+5);
                    edges.Add(new Edge(v.NodeId, v3.NodeId, 1));
                }
            }
            
            Graph g = new Graph(vertices, edges);
            var graph = g.Dijkstra(1);
        }
    }
}
