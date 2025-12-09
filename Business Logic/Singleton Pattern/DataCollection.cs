using AI_Graphs.FacadePattern;
using AI_Graphs.Graphs;
using AI_Graphs.Utils;
using System.Numerics;

namespace AI_Graphs.SingletonPattern
{
    // singleton
    public sealed class DataCollection
	{
		// variables to store frequently-used data for drawing
		public float Radius;
		public int Amount;
		public List<List<Node>> AdjList;
		public List<Color> ColorsList;
		public List<int> Paths;
		public List<MyPoint> NodesLocations;
		public List<Dictionary<int, Vector2>> TraceableLines;
		public Vector2 NewPosition = Vector2.Zero;
		public int FromIndex;

		//This variable is going to store the Singleton Instance
		private static DataCollection Instance = null;

		//To use the lock, we need to create one variable
		private static readonly object Instancelock = new object();

		//The following Static Method is going to return the Singleton Instance
		public static DataCollection GetInstance()
		{
			//This is thread-Safe
			if (Instance == null)
			{
				//As long as one thread locks the resource, no other thread can access the resource
				//As long as one thread enters into the Critical Section, 
				//no other threads are allowed to enter the critical section
				lock (Instancelock)
				{ //Critical Section Start
					Instance ??= new DataCollection();
				} //Critical Section End
				  //Once the thread releases the lock, the other thread allows entering into the critical section
				  //But only one thread is allowed to enter the critical section
			}

			//Return the Singleton Instance
			return Instance;
		}

		private DataCollection()
		{
			Radius = 30;
			NodesLocations = new();
			TraceableLines = new();
			Paths = null;
		}
	}
}
