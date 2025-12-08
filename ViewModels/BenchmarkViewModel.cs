using AI_Graphs.FactoryMethodPattern;
using AI_Graphs.Graphs;
using AI_Graphs.Models;
using AI_Graphs.PrototypePattern;
using AI_Graphs.ProxyPattern;
using AI_Graphs.StrategyPattern;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AI_Graphs.ViewModels
{
    public partial class BenchmarkViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<string> cities;

        [ObservableProperty]
        ObservableCollection<BenchmarkResult> benchmarkResults;

        [ObservableProperty]
        string selectedCity1;

        [ObservableProperty]
        string selectedCity2;

        [ObservableProperty]
        bool isBenchmarking;

        [ObservableProperty]
        string benchmarkStatus;

        [ObservableProperty]
        bool hasResults;

        [ObservableProperty]
        bool isSortedByName;

        [ObservableProperty]
        bool isSortedByPath;

        [ObservableProperty]
        bool isSortedByTime;

        [ObservableProperty]
        string nameSortIcon;

        [ObservableProperty]
        string pathSortIcon;

        [ObservableProperty]
        string timeSortIcon;

        private PrototypeManager prototypeManager;
        private Dictionary<string, CachingSearchProxy> searchProxies;
        private Dictionary<int, string> citiesIndex;
        private Dictionary<int, int> heuristicDistances;
        private Graph graph;
        private List<BenchmarkResult> allResults;

        private bool nameAscending = true;
        private bool pathAscending = true;
        private bool timeAscending = true;

        public BenchmarkViewModel()
        {
            prototypeManager = new PrototypeManager();
            Cities = [];
            BenchmarkResults = [];
            citiesIndex = [];
            allResults = [];
            searchProxies = [];

            // Initialize sort icons
            NameSortIcon = "↑";
            PathSortIcon = "↑";
            TimeSortIcon = "↑";

            LoadGraphData();
        }

        private void LoadGraphData()
        {
            try
            {
                string rootPath = Environment.ProcessPath;
                for (int idx = 0; idx < 6; idx++)
                {
                    rootPath = Directory.GetParent(rootPath).FullName;
                }

                string content;
                using (StreamReader reader = new(System.IO.Path.Combine(rootPath, "Business Logic\\Input\\map.csv")))
                {
                    content = reader.ReadToEnd();
                }

                var splitStr = content.Split('\n');
                var _cities = splitStr[0].Split(',');

                int i = 0;
                foreach (var city in _cities)
                {
                    var removeSlash = city.Split("\r");
                    string cityName = removeSlash[0];
                    citiesIndex.Add(i++, cityName);
                    Cities.Add(cityName);
                }

                graph = new Graph(citiesIndex.Count);

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

                heuristicDistances = new()
                {
                    { citiesIndex.FirstOrDefault(x => x.Value == "Arad").Key, 366 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Bucharest").Key, 0 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Craiova").Key, 160 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Drobita").Key, 242 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Eforie").Key, 161 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Fagaras").Key, 176 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Giurgiu").Key, 77 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Hirsova").Key, 151 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Iasi").Key, 226 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Lugoj").Key, 244 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Mehedia").Key, 241 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Neamt").Key, 234 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Oradea").Key, 380 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Pitesti").Key, 100 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "RM").Key, 193 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Sibiu").Key, 253 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Timisoara").Key, 329 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Urziceni").Key, 80 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Vaslui").Key, 199 },
                    { citiesIndex.FirstOrDefault(x => x.Value == "Zerind").Key, 374 }
                };

                SelectedCity1 = "Arad";
                SelectedCity2 = "Bucharest";

                BenchmarkStatus = "Ready to benchmark. Select cities and click 'Run Benchmark'";
            }
            catch (Exception ex)
            {
                BenchmarkStatus = $"Error loading data: {ex.Message}";
            }
        }

        [RelayCommand]
        async Task BenchmarkAll()
        {
            if (graph == null)
            {
                BenchmarkStatus = "Graph not loaded!";
                return;
            }

            if (string.IsNullOrEmpty(SelectedCity1) || string.IsNullOrEmpty(SelectedCity2))
            {
                BenchmarkStatus = "Please select both cities!";
                return;
            }

            BenchmarkResults.Clear();
            allResults.Clear();
            IsBenchmarking = true;
            HasResults = false;

            // Reset sort states
            IsSortedByName = false;
            IsSortedByPath = false;
            IsSortedByTime = false;

            var start = citiesIndex.FirstOrDefault(x => x.Value == SelectedCity1).Key;
            var end = citiesIndex.FirstOrDefault(x => x.Value == SelectedCity2).Key;

            GraphPrototype prototype = GraphPrototype.FromGraph(graph);
            prototypeManager.AddPrototype("benchmark_graph", prototype);

            var tasks = new List<Task>
            {
                RunBenchmarkWithFactory(new DFSFactory(), start, end),
                RunBenchmarkWithFactory(new BFSFactory(), start, end),
                RunBenchmarkWithFactory(new AStarFactory(), start, end),
                RunBenchmarkWithFactory(new GreedyFactory(), start, end),
                RunBenchmarkWithFactory(new UniformCostFactory(), start, end),
                RunBenchmarkWithFactory(new BidirectionalFactory(), start, end)
            };

            await Task.WhenAll(tasks);

            IsBenchmarking = false;
            HasResults = true;
            BenchmarkStatus = $"✓ Benchmark completed! ({BenchmarkResults.Count} algorithms tested)";
        }

        private async Task RunBenchmarkWithFactory(SearchAlgorithmFactory factory, int start, int end)
        {
            string name = factory.GetAlgorithmName();
            BenchmarkStatus = $"Running {name}...";

            if (!searchProxies.TryGetValue(name, out CachingSearchProxy proxy))
            {
                ISearchStrategy strategy = factory.CreateAlgorithm();
                proxy = new CachingSearchProxy(strategy, name);
                searchProxies[name] = proxy;
            }

            List<int> path = await Task.Run(() => proxy.FindPath(graph, start, end, heuristicDistances));
            long time = proxy.GetExecutionTime();

            var result = new BenchmarkResult(
                name,
                path?.Count ?? 0,
                time,
                path != null ? "✓" : "✗"
            );

            MainThread.BeginInvokeOnMainThread(() =>
            {
                allResults.Add(result);
                BenchmarkResults.Add(result);
            });
        }

        [RelayCommand]
        void SortByName()
        {
            IsSortedByName = true;
            IsSortedByPath = false;
            IsSortedByTime = false;

            var sorted = nameAscending
                ? allResults.OrderBy(x => x.AlgorithmName).ToList()
                : allResults.OrderByDescending(x => x.AlgorithmName).ToList();

            nameAscending = !nameAscending;
            NameSortIcon = nameAscending ? "↑" : "↓";

            BenchmarkResults.Clear();
            foreach (var result in sorted)
            {
                BenchmarkResults.Add(result);
            }
        }

        [RelayCommand]
        void SortByPath()
        {
            IsSortedByName = false;
            IsSortedByPath = true;
            IsSortedByTime = false;

            var sorted = pathAscending
                ? allResults.OrderBy(x => x.PathLength).ToList()
                : allResults.OrderByDescending(x => x.PathLength).ToList();

            pathAscending = !pathAscending;
            PathSortIcon = pathAscending ? "↑" : "↓";

            BenchmarkResults.Clear();
            foreach (var result in sorted)
            {
                BenchmarkResults.Add(result);
            }
        }

        [RelayCommand]
        void SortByTime()
        {
            IsSortedByName = false;
            IsSortedByPath = false;
            IsSortedByTime = true;

            var sorted = timeAscending
                ? allResults.OrderBy(x => x.ExecutionTimeMs).ToList()
                : allResults.OrderByDescending(x => x.ExecutionTimeMs).ToList();

            timeAscending = !timeAscending;
            TimeSortIcon = timeAscending ? "↑" : "↓";

            BenchmarkResults.Clear();
            foreach (var result in sorted)
            {
                BenchmarkResults.Add(result);
            }
        }
    }
}
