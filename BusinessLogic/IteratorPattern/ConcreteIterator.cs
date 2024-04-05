using AI_Graphs.Graphs;

namespace AI_Graphs.BusinessLogic.IteratorPattern
{
	public class AdjListIterator : IAdjListIterator
	{
		private readonly List<List<Node>> _adjList;
		private int _outerIndex = 0;
		private IEnumerator<Node> _innerEnumerator;

		public AdjListIterator(List<List<Node>> adjList)
		{
			_adjList = adjList;
			_innerEnumerator = _adjList.Count > 0 ? _adjList[0].GetEnumerator() : null;
		}

		public bool HasNext()
		{
			if (_innerEnumerator != null && _innerEnumerator.MoveNext())
			{
				return true;
			}

			// Check if there are more outer lists to iterate over
			_outerIndex++;
			if (_outerIndex < _adjList.Count)
			{
				_innerEnumerator = _adjList[_outerIndex].GetEnumerator();
				return _innerEnumerator.MoveNext();
			}

			return false;
		}

		public Node GetNext()
		{
			if (HasNext())
			{
				return _innerEnumerator.Current;
			}
			else
			{
				throw new InvalidOperationException("No more elements to iterate over");
			}
		}
	}

}
