using LogParser.Enums;
using LogParser.Logic;
using Xunit;

namespace Log_Parser.Test.UnitTests
{
    public class CommandExecutorTests
    {

        CommandExecutor _commandExecutor = new();

        [Theory]
        [InlineData("exit")]
        [InlineData("Exit")]
        [InlineData("EXIT")]
        [InlineData("eXiT")]
        public void Execute_ExitCommandGiven_ReturnReturnCodesExit(string command)
        {
            // Arrange
            var expected = ReturnCodes.Exit;

            // Act
            var actual = _commandExecutor.Execute(command);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData("notValidCommand")]
        [InlineData("exit ")]
        [InlineData("help ")]
        public void Execute_NotValidCommandGiven_ReturnReturnCodesCommandNotFound(string command)
        {
            // Arrange
            var expected = ReturnCodes.CommandNotFound;

            // Act
            var actual = _commandExecutor.Execute(command);

            // Assert
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("help")]
        [InlineData("HELP")]
        [InlineData("Help")]
        [InlineData("HeLp")]
        public void Execute_HelpCommandGiven_ReturnReturnCodesHelp(string command)
        {
            // Arrange
            var expected = ReturnCodes.Help;

            // Act
            var actual = _commandExecutor.Execute(command);

            // Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void GetFileName_NoFileSetPreviously_ReturnFileNotSet()
        {
            var expected = "No file set";

            var actual = _commandExecutor.GetFileName();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Execute_NoFileSetPreviouslyAndQueryIsGiven_ReturnReturnCodesFileNotSet()
        {
            var expected = ReturnCodes.FileNotSet;

            var actual = _commandExecutor.Execute("query someQuery");

            Assert.Equal(expected, actual);
        }
    }
}