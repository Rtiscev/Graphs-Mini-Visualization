using AI_Graphs.Business_Logic.Prototype_Pattern;
using AI_Graphs.Graphs;

namespace AI_Graphs.PrototypePattern;

public class GraphPrototype : IGraphPrototype
{
    public int numVertices;
    public List<List<Node>> adjList;

    public GraphPrototype(int numVertices)
    {
        this.numVertices = numVertices;
        adjList = new List<List<Node>>(numVertices);

        for (int i = 0; i < numVertices; i++)
        {
            adjList.Add(new List<Node>());
        }
    }

    public void AddEdge(int source, int destination, int weight)
    {
        adjList[source].Add(new Node(destination, weight));
    }

    public IGraphPrototype Clone()
    {
        GraphPrototype clone = new GraphPrototype(this.numVertices);

        for (int i = 0; i < this.adjList.Count; i++)
        {
            foreach (Node node in this.adjList[i])
            {
                clone.adjList[i].Add(new Node(node.country, node.weight));
            }
        }

        return clone;
    }

    public Graph ToGraph()
    {
        Graph graph = new Graph(this.numVertices);

        for (int i = 0; i < this.adjList.Count; i++)
        {
            foreach (Node node in this.adjList[i])
            {
                graph.AddEdge(i, node.country, node.weight);
            }
        }

        return graph;
    }

    public static GraphPrototype FromGraph(Graph graph)
    {
        GraphPrototype prototype = new GraphPrototype(graph.numVertices);

        for (int i = 0; i < graph.adjList.Count; i++)
        {
            foreach (Node node in graph.adjList[i])
            {
                prototype.adjList[i].Add(new Node(node.country, node.weight));
            }
        }

        return prototype;
    }
}
