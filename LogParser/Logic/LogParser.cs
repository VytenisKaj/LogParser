using LogParser.Enums;
using LogParser.Models;
using System.Text.RegularExpressions;

namespace LogParser.Logic
{
    internal class LogParser
    {
        private readonly IFileWriter _writer = new JsonFileWriter();

        public string FilePath { get; private set; } = String.Empty;

        private string[]? CsvColumnNames { get; set; } = Array.Empty<string>();

        public string BadColumn { get; private set; } = string.Empty;

        public QueryResult QueryResult { get; private set; } = new QueryResult();

        public ReturnCodes Parse(string command)
        {
            if(FilePath == String.Empty)
            {
                return ReturnCodes.FileNotSet;
            }

            var splitOutputFileFromQuery = RemoveCommandPrefix(command).Split('>');

            string query = splitOutputFileFromQuery[0].Trim();
            string? outputFile = splitOutputFileFromQuery.Length == 2 ? splitOutputFileFromQuery[1].Trim() : null;

            QueryResult = new QueryResult();

            string fullEntry = String.Empty;
            foreach (var line in File.ReadLines(FilePath))
            {
                fullEntry += line;
                if (fullEntry.Count(c => c == '\"') % 2 == 0)
                {
                    foreach (var queryWithoutOr in query.Split("||"))
                    {
                        var andQueries = queryWithoutOr.Split("&&");
                        if (!ColumnsExist(andQueries))
                        {
                            return ReturnCodes.ColumnNotFound;
                        }
                        CheckEntry(fullEntry, andQueries);
                    }
                    fullEntry = String.Empty;
                }
            }

            QueryResult.Query = query;
            QueryResult.ResultCount = QueryResult.Results.Count;

            if (outputFile != null)
            {
                _writer.Write(outputFile, QueryResult);
                return ReturnCodes.Success;
            }
            return ReturnCodes.SuccessNoOutputFile;
        }

        public ReturnCodes SetFile(string command)
        {
            string filePath = RemoveCommandPrefix(command).Trim();
            if (File.Exists(filePath))
            {
                if(Path.GetExtension(filePath) != ".csv")
                {
                    return ReturnCodes.WrongFileExtension;
                }
                FilePath = filePath;
                SetCsvColumnNames();
                return ReturnCodes.FileSet;
            }
            return ReturnCodes.FileNotFound;
        }

        public static string RemoveCommandPrefix(string command)
        {
            return command[(command.IndexOf(' ') + 1)..];
        }

        private void SetCsvColumnNames() => CsvColumnNames = File.ReadLines(FilePath).FirstOrDefault()?.Split(',').Select(column => column.Trim()).ToArray();

        private bool ColumnsExist(string[] andQueries)
        {
            foreach (var andQuery in andQueries)
            {
                string columnToCheck = andQuery.Contains("!=") ? andQuery.Split("!=")[0].Trim() : andQuery.Split("=")[0].Trim();
                if (!Array.Exists(CsvColumnNames, column => column == columnToCheck))
                {
                    BadColumn = columnToCheck;
                    return false;
                }
            }
            return true;
        }

        private bool EntryPassedQueries(string[] entryAsElements, string[] queries)
        {
            foreach(string query in queries)
            {
                string[] columnAndValue = query.Contains("!=") ? query.Split("!=") : query.Split("=");
                string column = columnAndValue[0].Trim();
                string value = columnAndValue[1].Trim();
                bool queryResult = query.Contains("!=") ? !EntryContainsValueInColumn(entryAsElements, column, value) : EntryContainsValueInColumn(entryAsElements, column, value);
                if (!queryResult)
                {
                    return false;
                }
            }
            return true;
        }

        private bool EntryContainsValueInColumn(string[] entryAsElements, string column, string value)
        {
            return entryAsElements[Array.IndexOf(CsvColumnNames, column)].Contains(value.Trim());
        }

        private void CheckEntry(string entry, string[] queries)
        {
            var entryElements = Regex.Split(entry, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            if (ArraysContainSameValues(entryElements, CsvColumnNames))
            {
                return;
            }
            if (EntryPassedQueries(entryElements, queries))
            {
                AddEntryToQueryResults(entryElements);
            }
        }

        private bool ArraysContainSameValues(string[] array1, string[] array2)
        {
            return array1.OrderBy(x => x).SequenceEqual(array2.OrderBy(x => x));
        }

        private void AddEntryToQueryResults(string[] entryElements)
        {
            var queryResultAsDictionary = new Dictionary<string, string>();
            for (int i = 0; i < CsvColumnNames?.Length; i++)
            {
                queryResultAsDictionary.Add(CsvColumnNames[i], entryElements[i]);
            }
            QueryResult.Results.Add(queryResultAsDictionary);
        }
       
    }
}
