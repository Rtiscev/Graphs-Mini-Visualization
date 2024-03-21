using AI_Graphs.Graphs;
using AI_Graphs.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;

namespace AI_Graphs.ViewModels
{
	public partial class MainPageViewModel : ObservableObject
	{
		public event EventHandler<StringEventArgs> WorkCompleted;
		public event EventHandler<StringEventArgs> DisplayMsg;

		[ObservableProperty]
		ObservableCollection<string> methods;
		[ObservableProperty]
		ObservableCollection<string> cities;
		[ObservableProperty]
		ObservableCollection<PathModel> paths;
		[ObservableProperty]
		ObservableCollection<CountryAndID> countriesNodes;

		[ObservableProperty]
		GraphicsDrawable canvas;
		[ObservableProperty]
		int selectedIndex;
		[ObservableProperty]
		string selectedMethod;
		[ObservableProperty]
		string selectedCity1;
		[ObservableProperty]
		string selectedCity2;
		[ObservableProperty]
		bool isNotInitialized;
		[ObservableProperty]
		bool isLocked;
		[ObservableProperty]
		double nodesSize;

		enum SearchAlgorithms
		{
			Depth_First_Search,
			Breath_First_Search,
			A_Star,
			Greedy_Best_First_Search,
			Uniform_Cost,
			Bidirectional_Search
		}

		Dictionary<int, int> heuristicDistances;
		Dictionary<int, string> citiesIndex;
		Graphs.Graph graph;
		public MainPageViewModel()
		{
			Canvas = new();
			Canvas.IsInitialized = false;
			IsNotInitialized = true;
			IsLocked = false;
			CountriesNodes = new();
			Cities = new();
			Methods = new() { "Depth First Search", "Breath First Search", "A Star", "Greedy Best First Search", "Uniform Cost", "Bidirectional Search" };
			citiesIndex = new();
			Paths = new();
		}


		[RelayCommand]
		void ReDraw()
		{
			Canvas.ChangeRadius((float)NodesSize);
			WorkCompleted?.Invoke(this, new(""));
		}

		[RelayCommand]
		void RandomizeNodes()
		{
			Canvas.IsRandom = true;
			WorkCompleted?.Invoke(this, new(""));
		}

		[RelayCommand]
		void GraphInit()
		{
			// process file
			string rootPath = Environment.ProcessPath;
			for (int oo = 0; oo < 6; oo++)
			{
				rootPath = Directory.GetParent(rootPath).FullName;
			}

			string content;
			using (StreamReader reader = new(System.IO.Path.Combine(rootPath, "Input\\map.csv")))
			{
				content = reader.ReadToEnd();
			}
			var splitStr = content.Split('\n');

			var _cities = splitStr[0].Split(',');
			int i = 0;
			foreach (var city in _cities)
			{
				var removeSlash = city.Split("\r");
				citiesIndex.Add(i++, removeSlash[0]);
			}

			// Convert dictionary entries to Country objects and add them to the list
			foreach (var keyValuePair in citiesIndex)
			{
				CountryAndID country = new(keyValuePair.Value, keyValuePair.Key);
				CountriesNodes.Add(country);
			}

			// add nodes and weights
			graph = new(citiesIndex.Count);

			for (int j = 1; j < splitStr.Length; j++)
			{
				var splitSecond = splitStr[j].Split(',');
				for (int k = 0; k < splitSecond.Length; k++)
				{
					if (int.Parse(splitSecond[k]) > 0)
					{
						graph.AddEdge(j - 1, k, int.Parse(splitSecond[k]));
					}
				}
			}

			foreach (var city in citiesIndex)
			{
				Cities.Add(city.Value);
			}

			heuristicDistances = new()
			{
				{  citiesIndex.FirstOrDefault(x => x.Value == "Arad").Key, 366 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Bucharest").Key, 0 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Craiova").Key, 160 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Drobita").Key, 242 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Eforie").Key, 161 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Fagaras").Key, 176 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Giurgiu").Key, 77 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Hirsova").Key, 151 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Iasi").Key, 226 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Lugoj").Key, 244 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Mehedia").Key, 241 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Neamt").Key, 234 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Oradea").Key, 380 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Pitesti").Key, 100 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "RM").Key, 193 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Sibiu").Key, 253 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Timisoara").Key, 329 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Urziceni").Key, 80 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Vaslui").Key, 199 },
				{  citiesIndex.FirstOrDefault(x => x.Value == "Zerind").Key, 374 }
			};

			IsLocked = true;
			IsNotInitialized = false;
			Canvas.IsInitialized = true;
			Canvas.Amount = graph.numVertices;
			Canvas.AdjList = graph.adjList;
			WorkCompleted?.Invoke(this, new(""));
		}

		[RelayCommand]
		void ChangeColors()
		{
			Canvas.IsRandom = false;
			WorkCompleted?.Invoke(this, new(""));
		}

		[RelayCommand]
		async Task FindPath()
		{
			Paths.Clear();

			var startC = citiesIndex.FirstOrDefault(x => x.Value == SelectedCity1).Key;
			var endC = citiesIndex.FirstOrDefault(x => x.Value == SelectedCity2).Key;
			List<int> path = new();
			if (SelectedIndex == (int)SearchAlgorithms.Depth_First_Search)
			{
				path = DFS.Search(graph, startC, endC);
			}
			else if (SelectedIndex == (int)SearchAlgorithms.Breath_First_Search)
			{
				path = BFS.Search(graph, startC, endC);
			}
			else if (SelectedIndex == (int)SearchAlgorithms.A_Star)
			{
				var graph2 = new List<List<AStar.Node2>>();
				foreach (var ff in graph.adjList)
				{
					var minigraph2 = new List<AStar.Node2>();
					foreach (var gg in ff)
					{
						minigraph2.Add(new AStar.Node2() { Country = gg.country, Weight = gg.weight, Id = default, TotalCost = default });
					}
					graph2.Add(minigraph2);
				}
				path = AStar.FindPath(graph2, heuristicDistances, startC, endC);
			}
			else if (SelectedIndex == (int)SearchAlgorithms.Uniform_Cost)
			{
				//var graph4 = new List<List<Node9>>();
				//foreach (var ff in graph.adjList)
				//{
				//	var minigraph4 = new List<Node9>();
				//	foreach (var gg in ff)
				//	{
				//		minigraph4.Add(new Node9(gg.country, gg.weight));
				//	}
				//	graph4.Add(minigraph4);
				//}

				IGraphAdapter graphAdapter = new GraphAdapter(graph);
				UniformCostSearch ucs = new();
				path = ucs.FindPath(graphAdapter.GetAdjacencyList(), startC, endC);
			}
			else if (SelectedIndex == (int)SearchAlgorithms.Greedy_Best_First_Search)
			{
				path = GreedyBestFirstSearch.FindPath(graph.adjList, heuristicDistances, startC, endC);
			}
			else if (SelectedIndex == (int)SearchAlgorithms.Bidirectional_Search)
			{
				path = BidirectionalSearch.BiDirSearch(graph, startC, endC);
			}
			//var path = BFS.Search(graph, startC, endC);

			Canvas.Paths = null;
			Canvas.TraceableLines = new();
			if (path != null)
			{
				for (int i = 0; i < path.Count - 1; i++)
				{
					Paths.Add(new(i + 1, citiesIndex[path[i]], path[i], 1));
					// get current city 
					Vector2 fromCity = new(Canvas.NodesLocations[path[i]].X, Canvas.NodesLocations[path[i]].Y);
					Vector2 toCity = new(Canvas.NodesLocations[path[i + 1]].X, Canvas.NodesLocations[path[i + 1]].Y);
					// and city that it goes to

					float lerpFactor = 0;
					while (lerpFactor <= 1.0f)
					{
						lerpFactor += 0.01f;
						Canvas.IsTraceable = true;
						Canvas.FromIndex = path[i];
						Canvas.NewPosition = Vector2.Lerp(fromCity, toCity, lerpFactor);
						IsLocked = false;
						WorkCompleted?.Invoke(this, new(""));
						await Task.Delay(10);
					}
				}

				Paths.Add(new(path.Count, citiesIndex[path[^1]], path[^1], 0));
				IsLocked = true;
				IsNotInitialized = false;
				// add the last item
				Canvas.Paths = path;
			}
			else
			{
				DisplayMsg?.Invoke(this, new(""));
				IsLocked = true;
				IsNotInitialized = false;
			}
		}
	}
}