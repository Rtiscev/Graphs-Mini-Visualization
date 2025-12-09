using AI_Graphs.DrawingMethods;
using AI_Graphs.Graphs;
using AI_Graphs.Models;
using AI_Graphs.SingletonPattern;
using AI_Graphs.StrategyPattern;
using AI_Graphs.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Windows.Input;

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
        [NotifyCanExecuteChangedFor(nameof(FindPathCommand))]
        int selectedIndex;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FindPathCommand))]
        string selectedMethod;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FindPathCommand))]
        string selectedCity1;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FindPathCommand))]
        string selectedCity2;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ReDrawCommand))]
        [NotifyCanExecuteChangedFor(nameof(RandomizeNodesCommand))]
        [NotifyCanExecuteChangedFor(nameof(ChangeColorsCommand))]
        [NotifyCanExecuteChangedFor(nameof(FindPathCommand))]
        bool isNotInitialized;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ReDrawCommand))]
        [NotifyCanExecuteChangedFor(nameof(RandomizeNodesCommand))]
        [NotifyCanExecuteChangedFor(nameof(ChangeColorsCommand))]
        [NotifyCanExecuteChangedFor(nameof(FindPathCommand))]
        bool isLocked;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ReDrawCommand))]
        double nodesSize;

        [ObservableProperty]
        string errorMessage;

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
        Graph graph;

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
            ErrorMessage = string.Empty;

            // Set default algorithm selection
            SelectedIndex = 0;

            canvas.SendBytesUp += Canvas_SendBytesUp;
        }

        private void Canvas_SendBytesUp(object sender, BusinessLogic.Utils.ByteArrayEventArgs e)
        {

        }

        // Helper method to safely invoke events
        private void RaiseWorkCompleted(string message = "")
        {
            WorkCompleted?.Invoke(this, new StringEventArgs(message));
        }

        // Helper method to safely display messages
        private void ShowError(string message)
        {
            ErrorMessage = message;
            DisplayMsg?.Invoke(this, new StringEventArgs(message));
        }

        // Validation: Can only redraw if graph is initialized and locked, and NodesSize is valid
        private bool CanReDraw() => IsLocked && !IsNotInitialized && NodesSize >= 30 && NodesSize <= 50;

        [RelayCommand(CanExecute = nameof(CanReDraw))]
        void ReDraw()
        {
            Canvas.ChangeRadius((float)NodesSize);
            RaiseWorkCompleted();
        }

        // Validation: Can only randomize if graph is initialized and locked
        private bool CanRandomizeNodes() => IsLocked && !IsNotInitialized;

        [RelayCommand(CanExecute = nameof(CanRandomizeNodes))]
        void RandomizeNodes()
        {
            Canvas.IsRandom = true;
            RaiseWorkCompleted();
        }

        // Validation: Can only initialize once (when not initialized)
        private bool CanGraphInit() => IsNotInitialized;

        [RelayCommand(CanExecute = nameof(CanGraphInit))]
        void GraphInit()
        {
            try
            {
                // process file
                string rootPath = Environment.ProcessPath;
                for (int oo = 0; oo < 6; oo++)
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
                DataCollection.GetInstance().Amount = graph.numVertices;
                DataCollection.GetInstance().AdjList = graph.adjList;
                ErrorMessage = string.Empty;
                RaiseWorkCompleted();
            }
            catch (Exception ex)
            {
                ShowError($"Error loading graph: {ex.Message}");
            }
        }

        // Validation: Can only change colors if graph is initialized and locked
        private bool CanChangeColors() => IsLocked && !IsNotInitialized;

        [RelayCommand(CanExecute = nameof(CanChangeColors))]
        void ChangeColors()
        {
            Canvas.IsRandom = false;
            RaiseWorkCompleted();
        }

        // Validation: Can only find path if all conditions are met
        private bool CanFindPath()
        {
            return IsLocked &&
                   !IsNotInitialized &&
                   !string.IsNullOrWhiteSpace(SelectedCity1) &&
                   !string.IsNullOrWhiteSpace(SelectedCity2) &&
                   SelectedCity1 != SelectedCity2 &&
                   IsValidAlgorithmSelected();
        }

        // Helper method to validate algorithm selection
        private bool IsValidAlgorithmSelected()
        {
            // Check if SelectedIndex is within valid enum range
            return SelectedIndex >= 0 &&
                   SelectedIndex < Enum.GetValues(typeof(SearchAlgorithms)).Length &&
                   !string.IsNullOrWhiteSpace(SelectedMethod);
        }

        [RelayCommand(CanExecute = nameof(CanFindPath))]
        async Task FindPath()
        {
            try
            {
                Paths.Clear();
                ErrorMessage = string.Empty;

                var startC = citiesIndex.FirstOrDefault(x => x.Value == SelectedCity1).Key;
                var endC = citiesIndex.FirstOrDefault(x => x.Value == SelectedCity2).Key;

                // Validate that cities exist in the index
                if (!citiesIndex.ContainsValue(SelectedCity1) || !citiesIndex.ContainsValue(SelectedCity2))
                {
                    ShowError("Invalid city selection");
                    return;
                }

                // Validate algorithm selection
                if (!IsValidAlgorithmSelected())
                {
                    ShowError("Please select a valid search algorithm");
                    return;
                }

                SearchContext ctx = new();

                // Use switch expression for cleaner code
                switch (SelectedIndex)
                {
                    case (int)SearchAlgorithms.Depth_First_Search:
                        ctx.SetSearchStrategy(new DFS_Strategy());
                        break;
                    case (int)SearchAlgorithms.Breath_First_Search:
                        ctx.SetSearchStrategy(new BFS_Strategy());
                        break;
                    case (int)SearchAlgorithms.A_Star:
                        ctx.SetSearchStrategy(new AStar_Strategy());
                        break;
                    case (int)SearchAlgorithms.Uniform_Cost:
                        ctx.SetSearchStrategy(new UniformCost_Strategy());
                        break;
                    case (int)SearchAlgorithms.Greedy_Best_First_Search:
                        ctx.SetSearchStrategy(new GreedyBestFirst_Strategy());
                        break;
                    case (int)SearchAlgorithms.Bidirectional_Search:
                        ctx.SetSearchStrategy(new Bidirectional_Strategy());
                        break;
                    default:
                        ShowError("Invalid algorithm selection");
                        return;
                }

                List<int> path = ctx.FindPath(graph, startC, endC, heuristicDistances);
                DataCollection.GetInstance().Paths = null;
                DataCollection.GetInstance().TraceableLines = new();

                if (path != null && path.Count > 0)
                {
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        // Validate path indices
                        if (path[i] < 0 || path[i] >= DataCollection.GetInstance().NodesLocations.Count ||
                            path[i + 1] < 0 || path[i + 1] >= DataCollection.GetInstance().NodesLocations.Count)
                        {
                            ShowError("Invalid path generated");
                            return;
                        }

                        Paths.Add(new(i + 1, citiesIndex[path[i]], path[i], 1));

                        // get current city 
                        Vector2 fromCity = new(DataCollection.GetInstance().NodesLocations[path[i]].X,
                                               DataCollection.GetInstance().NodesLocations[path[i]].Y);
                        Vector2 toCity = new(DataCollection.GetInstance().NodesLocations[path[i + 1]].X,
                                             DataCollection.GetInstance().NodesLocations[path[i + 1]].Y);

                        float lerpFactor = 0;
                        while (lerpFactor <= 1.0f)
                        {
                            lerpFactor += 0.01f;
                            Canvas.IsTraceable = true;
                            DataCollection.GetInstance().FromIndex = path[i];
                            DataCollection.GetInstance().NewPosition = Vector2.Lerp(fromCity, toCity, lerpFactor);
                            IsLocked = false;
                            RaiseWorkCompleted();
                            await Task.Delay(10);
                        }
                    }

                    Paths.Add(new(path.Count, citiesIndex[path[^1]], path[^1], 0));

                    // add the last item
                    DataCollection.GetInstance().Paths = path;
                }
                else
                {
                    ShowError("No path found between selected cities");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error finding path: {ex.Message}");
            }
            finally
            {
                IsLocked = true;
                IsNotInitialized = false;
            }
        }
    }
}
