using LogParser.Enums;
using LogParser.Logic;

namespace LogParser
{
    public class Program
    {
        private readonly CommandExecutor _commandExecutor = new();
        const string help = "exit -> Exits the program\n" + 
                               "help -> Shows this page\n" +
                               "file <file path> -> Sets a csv file as an input file\n" +
                               "query <query> [> <output file path>] -> Executes provided query on input file and shows result. Can be redirected to file.\n" +
                               "=, !=, && and || operators are suported for queries. Example: query column1 = value1 && column2 != value2 || column3 = value3 > out.json\n" + 
                               "Order of operator execution: =, != -> && -> ||\n" +
                               "use_default -> For testing only. Sets input file to \"20220601182758.csv\"\n";


        public void Run()
        {
            string? input;
            ReturnCodes returnCode;
            Console.WriteLine("Log file parser. For help type \"help\"");
            while (true)
            {
                try
                {
                    Console.Write($"{_commandExecutor.GetFileName()} > ");
                    input = Console.ReadLine();
                    if (input == null)
                    {
                        Console.WriteLine("Input can't be parsed");
                    }
                    else
                    {
                        returnCode = _commandExecutor.Execute(input);
                        switch (returnCode)
                        {
                            case ReturnCodes.Exit:
                                return;
                            case ReturnCodes.FileNotFound:
                                Console.WriteLine($"File \"{Logic.LogParser.RemoveCommandPrefix(input).Trim()}\" does not exist");
                                break;
                            case ReturnCodes.FileNotSet:
                                Console.WriteLine("No input file has been set. Use command \"file <file path>\" to set it or use \"help\".");
                                break;
                            case ReturnCodes.FileSet:
                                Console.WriteLine("Input file has been set successfully");
                                break;
                            case ReturnCodes.WrongFileExtension:
                                Console.WriteLine("Input file has to be with extension \".csv\"");
                                break;
                            case ReturnCodes.CommandNotFound:
                                Console.WriteLine($"\"{input}\" is not a valid command, type \"help\" to see list of commands");
                                break;
                            case ReturnCodes.ColumnNotFound:
                                Console.WriteLine($"Column \"{_commandExecutor.GetBadColumnName()}\" not found.");
                                break;
                            case ReturnCodes.SuccessNoOutputFile:
                                Console.WriteLine($"{_commandExecutor.GetQueryResultAsJson()}");
                                break;
                            case ReturnCodes.Success:
                                Console.WriteLine("Query ran successfully");
                                break;
                            case ReturnCodes.Help:
                                Console.WriteLine(help);
                                break;
                            case ReturnCodes.IncorrectSyntax:
                                Console.WriteLine("Bad query syntax");
                                break;

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error happened. Error message: {ex.Message}");
                }
            }
        }


        public static void Main()
        {
            new Program().Run();
        }
    }
}
