
namespace LogParser.Models
{
    internal class QueryResult
    {
        public string Query { get; set; } = string.Empty;
        public int ResultCount { get; set; } = 0;
        public HashSet<Dictionary<string, string>> Results { get; set; } = new();
    }
}
