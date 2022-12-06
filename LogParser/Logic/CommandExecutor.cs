using LogParser.Enums;
using System.Text.Json;

namespace LogParser.Logic
{
    public class CommandExecutor
    {
        private readonly LogParser _parser = new();

        public string GetFileName()
        {
            string fileName = _parser.FilePath;
            if (fileName == String.Empty)
            {
                return "No file set";
            }
            return Path.GetFileName(fileName);
        }

        public string GetBadColumnName()
        {
            return _parser.BadColumn;
        }

        public string GetQueryResultAsJson()
        {
            return JsonSerializer.Serialize(_parser.QueryResult, new JsonSerializerOptions { WriteIndented = true });
        }


        public ReturnCodes Execute(string command)
        {

            if (command.ToLower() == "exit")
            {
                return ReturnCodes.Exit;
            }
            if (command.ToLower() == "help")
            {
                return ReturnCodes.Help;
            }
            if(command.ToLower().StartsWith("query "))
            {
                return _parser.Parse(command);
            }
            if(command.ToLower().StartsWith("file "))
            {
                return _parser.SetFile(command);
            }
            if(command.ToLower() == "use_default")
            {
                return _parser.SetFile("file ./../../../Data/20220601182758.csv");
            }
                    
            return ReturnCodes.CommandNotFound;

        }
    }
}
