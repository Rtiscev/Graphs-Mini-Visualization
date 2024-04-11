using AI_Graphs.Graphs;

namespace AI_Graphs.BusinessLogic.IteratorPattern
{
	// Iterator Interface
	// This is going to be an interface defining the operations for accessing and traversing elements in a sequence.
	public interface IAdjListIterator
	{
		// get current
		// move to next
		bool HasNext();
		Node GetNext();
	}
}
