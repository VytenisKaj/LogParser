using LogParser.Models;
using System.Text.Json;

namespace LogParser.Logic
{
    internal class JsonFileWriter : IFileWriter
    {
        public void Write(string targetPath, QueryResult data)
        {
            File.WriteAllText(targetPath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
