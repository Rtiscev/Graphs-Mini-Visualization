using AI_Graphs.Graphs;

namespace AI_Graphs.AdapterPattern
{
	// ITarget
	public interface IGraph2Adapter
	{
		List<List<AStar.Node2>> GetAdjacencyList();
	}
}
