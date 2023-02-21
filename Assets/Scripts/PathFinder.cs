using System.Collections.Generic;
using System.Linq;
using Vertex = System.Int32;

public class Graph
{
    public (List<Vertex>, List<Vertex>) Dijkstra(ref Graph graph, Vertex start)
    {
        const int inf = (int)1e9; // dirty
        var distance = new List<Vertex>(graph.Size());
        var previous = new List<Vertex>(graph.Size());
        var queue = new PriorityQueue<Vertex, int>();

        distance[start] = 0;
        for (Vertex vertex = 0; vertex < graph.Size(); ++vertex)
        {
            if (vertex != start)
            {
                distance[vertex] = inf;
            }
            queue.AddWithPriority(vertex, distance[vertex]);
        }

        while (!queue.Empty())
        {
            Vertex vertex = queue.ExctractMin();

            foreach (Edge edge in graph.GetNeighbors(vertex))
            {
                int alt_distance = distance[vertex] + edge.cost;

                if (alt_distance < distance[edge.to])
                {
                    distance[edge.to] = alt_distance;
                    previous[edge.to] = vertex;
                    queue.DecreasePriority(edge.to, alt_distance);
                }
            }
        }

        return (distance, previous);
    }

    public struct Edge
    {
        public Vertex to;
        public int cost { get; private set; }

        public Edge(Vertex to, int cost)
        {
            this.to = to;
            this.cost = cost;
        }

        public void SetCost (int c)
        {
            cost = c;
        }
    };

    List<List<Edge>> edges;
    int edge_num;

    public Graph(int size)
    {
        this.edges = new List<List<Edge>>(size);
    }

    public int Size()
    {
        return this.edges.Count;
    }

    public int EdgeNum()
    {
        return this.edge_num;
    }

    public void AddEdge(Vertex from, Vertex to, int cost)
    {
        this.edges[from].Add(new Edge(to, cost));
        ++this.edge_num;
    }

    public void AddEdgeWithoutDups(Vertex from, Vertex to, int cost)
    {
        for (int i = 0; i < edges[from].Count; ++i)
        {
            if (edges[from][i].to == to)
            {
                if (edges[from][i].cost > cost)
                {
                    edges[from][i].SetCost(cost);
                }
                return;
            }
        }
        AddEdge(from, to, cost);
    }

    public void DeleteEdge(Vertex from, int id)
    {
        Edge temp = this.edges[from][id];
        this.edges[from][id] = this.edges[from].Last();
        this.edges[from][this.edges[from].Count - 1] = temp;
        this.edges[from].RemoveAt(this.edges[from].Count - 1);
        --this.edge_num;
    }

    public void DeleteVertex(Vertex vertex)
    {
        this.edge_num -= edges[vertex].Count;
    }

    public List<Edge> GetNeighbors(Vertex vertex)
    {
        return this.edges[vertex];
    }
}