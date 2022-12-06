
using LogParser.Models;

namespace LogParser.Logic
{
    internal interface IFileWriter
    {
        public void Write(string target, QueryResult data);
    }
}
