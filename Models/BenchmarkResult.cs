namespace AI_Graphs.Models
{
    public class BenchmarkResult
    {
        public string AlgorithmName { get; set; }
        public int PathLength { get; set; }
        public long ExecutionTimeMs { get; set; }
        public string Status { get; set; }
        public string StatusColor => Status == "✓" ? "LightGreen" : "Red";

        public BenchmarkResult(string algorithm, int pathLength, long time, string status = "✓")
        {
            AlgorithmName = algorithm;
            PathLength = pathLength;
            ExecutionTimeMs = time;
            Status = status;
        }
    }
}
