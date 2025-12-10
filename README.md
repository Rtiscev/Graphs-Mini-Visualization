# Graphs Mini Visualization

Graph visualization with algorithm comparison in a .NET MAUI app.

## Description

This project is a .NET MAUI application that displays graphs (nodes and edges) and enables visual comparison of results from different algorithms applied to them, such as node distribution strategies or traversal methods. The goal is to provide a small, educational tool for better understanding how algorithms behave on graph data structures.

## Features

- Static visualization of graphs with nodes and edges generated in the application.
- Comparison between algorithms (e.g., node distribution or traversal methods) by running them on the same graph.
- **Benchmark feature** to measure and compare performance metrics (execution time, visited nodes, path length) across all implemented pathfinding algorithms.
- Clear layer organization: business logic, models, view-models, and views (MVVM).

## Project Structure

- `Business Logic/` – Contains logic for generating, modifying, and analyzing graphs, including distribution and processing algorithms.
- `Models/` – Models for graph, node, edge, and other entities used in the application.
- `ViewModels/` – View-models that bind the models to the MAUI graphical interface (MVVM pattern).
- `Views/` – MAUI pages and controls that draw the graphs and display relevant information.
- `Resources/` – Shared resources (styles, images, etc.).
- `Platforms/` – Platform-specific configurations for Android, Windows, and other MAUI targets.

## Getting Started

1. Install .NET SDK and .NET MAUI support in Visual Studio or another compatible IDE.
2. Clone the repository:
   ```bash
   git clone https://github.com/Rtiscev/Graphs-Mini-Visualization.git
   ```
3. Open the solution:
   ```text
   AI_Graphs.sln
   ```
4. Restore packages and run the application on the desired platform (Windows, Android, etc.).

## Usage

- Open the application and load or generate a graph from the interface.
- Choose an available algorithm and run it on the same graph to observe differences in node distribution or structure traversal.
- Use the **Benchmark Mode** to run all pathfinding algorithms simultaneously and compare their performance metrics.
- Repeat with other algorithms to visually compare the results.

## Algorithms

### Graph Layout Algorithms

- **Random distribution vs. force-directed distribution** – compares how nodes are positioned in the plane.

### Pathfinding & Traversal Algorithms

- **Breadth-First Search (BFS)** – Unweighted graph traversal that explores nodes level by level. Guarantees shortest path in unweighted graphs.

- **Depth-First Search (DFS)** – Explores as far as possible along each branch before backtracking. Does not guarantee shortest path but useful for connectivity and cycle detection.

- **A\* Search** – Heuristic-based pathfinding that extends Dijkstra by using estimated distance to goal (heuristic function). Often faster than Dijkstra while still finding optimal path with an admissible heuristic.

- **Uniform Cost Search (Dijkstra)** – Single-source shortest path algorithm for weighted graphs with non-negative edge weights. Explores nodes in order of increasing path cost, guaranteeing the optimal path.

- **Greedy Best-First Search** – Heuristic-based search that always expands the node that appears closest to the goal. Faster than A\* but does not guarantee an optimal path.

- **Bidirectional Search** – Searches simultaneously from both start and goal nodes, potentially reducing search time. Meets in the middle when the two search frontiers intersect.

## Benchmark Feature

The application includes a benchmarking system to objectively compare pathfinding algorithm performance:

### Metrics Collected

- **Execution Time** – Total time taken to find the path (in milliseconds)
- **Visited Nodes** – Number of nodes explored during the search
- **Path Length** – Total length/cost of the discovered path
- **Success Rate** – Whether a valid path was found

### How to Use

1. Configure the graph with start and end nodes
2. Select algorithms to include in the benchmark
3. Run the benchmark from the UI
4. View the comparison table showing metrics side-by-side

This feature helps understand trade-offs between algorithms under different graph conditions (sparse vs. dense, weighted vs. unweighted).

## Technologies

- .NET MAUI
- C#
- MVVM

## Future Plans

- Adding more layout and traversal algorithms
- Features for saving/loading graphs from files
- Improving visualization (colors, legends, node label display)
- Export benchmark results to CSV/JSON
