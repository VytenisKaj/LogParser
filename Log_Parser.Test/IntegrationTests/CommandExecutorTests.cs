

using LogParser.Enums;
using LogParser.Logic;
using Xunit;

namespace Log_Parser.Test.IntegrationTests
{
    public class CommandExecutorTests
    {
        const string pathToTestCsv = "./IntegrationTests/TestFiles/testCsv.csv";
        const string pathToTestNotCsvFile = "./IntegrationTests/TestFiles/badFile.txt";

        [Fact]
        public void GetFileName_TestCsvFileSet_ReturnFileName()
        {
            // Arrange
            var expected = "testCsv.csv";
            var _commandExecutor = new CommandExecutor();
            _commandExecutor.Execute($"file {pathToTestCsv}");

            // Act
            var actual = _commandExecutor.GetFileName();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Execute_FileSetButColumnDoesNotExist_ReturnReturnCodesColumnNotFound()
        {
            // Arrange
            var expected = ReturnCodes.ColumnNotFound;
            var _commandExecutor = new CommandExecutor();
            _commandExecutor.Execute($"file {pathToTestCsv}");

            // Act
            var actual = _commandExecutor.Execute("query badColumnName = something");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetBadColumnName_FileSetButColumnDoesNotExist_ReturnBadColumnName()
        {
            // Arrange
            var _commandExecutor = new CommandExecutor();
            var badColumn = "badColumnName";
            var expected = badColumn;
            _commandExecutor.Execute($"file {pathToTestCsv}");
            _commandExecutor.Execute($"query {badColumn} = something");

            // Act
            var actual = _commandExecutor.GetBadColumnName();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Execute_SettingFileWithWrongExtension_ReturnReturnCodesWrongFileExtension()
        {
            // Arrange
            var _commandExecutor = new CommandExecutor();
            var expected = ReturnCodes.WrongFileExtension;
            
            // Act
            var actual = _commandExecutor.Execute($"file {pathToTestNotCsvFile}"); ;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Execute_SettingFileWithBadPath_ReturnReturnCodesFileNotFound()
        {
            // Arrange
            var _commandExecutor = new CommandExecutor();
            var expected = ReturnCodes.FileNotFound;

            // Act
            var actual = _commandExecutor.Execute($"file bad/file/path"); ;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("query col1=something")]
        [InlineData("query col1=")]
        [InlineData("query col1 =      something    ")]
        [InlineData("query col1!=something")]
        [InlineData("query col1=something&&col2=something")]
        [InlineData("query col1=something&&col2=something||col1!=something")]
        [InlineData("query col1 = something && col2 = something || col1 != something")]
        [InlineData("query col1 = something && col2 =  || col1 != something")]
        [InlineData("QUERY col1=something")]
        public void Execute_ValidQueryWithNoOuputFile_ReturnSuccessNoOutputFile(string query)
        {
            // Arrange
            var _commandExecutor = new CommandExecutor();
            _commandExecutor.Execute($"file {pathToTestCsv}");
            var expected = ReturnCodes.SuccessNoOutputFile;

            // Act
            var actual = _commandExecutor.Execute(query); ;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("query col1=something > out.json")]
        [InlineData("query col1=>out.json")]
        [InlineData("query col1 =      something    >                out.json")]
        [InlineData("query col1=something&&col2=something>out.json")]
        [InlineData("query col1=something&&col2=something||col1!=something> out.json")]
        [InlineData("query col1 = something && col2 = something || col1 != something >out.json")]
        public void Execute_ValidQueryWithFile_ReturnSuccess(string query)
        {
            // Arrange
            var _commandExecutor = new CommandExecutor();
            _commandExecutor.Execute($"file {pathToTestCsv}");
            var expected = ReturnCodes.Success;

            // Act
            var actual = _commandExecutor.Execute(query); ;

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}
