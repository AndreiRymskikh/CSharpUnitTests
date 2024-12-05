using System.Reflection;

namespace Tests.Tests
{
    [TestClass]
    public class DisplayDateTimeTests
    {
        //catches console output and checkes value
        [TestMethod]
        public void DisplayDateTimeOutputCorrectFormat()
        {
            var dateTime = new DateTimeOffset(2024, 11, 20, 08, 16, 14, TimeSpan.FromHours(1));
            var program = new Program(null);
            var expectedOutput = "Test Label: Wednesday 20 November 2024 08:16:14";

            var consoleOutputReader = new ConsoleReader();
            program.DisplayDateTime("Test Label", dateTime);
            var result = consoleOutputReader.ConsoleOutput();

            Assert.AreEqual(expectedOutput, result);
        }

        //spies the value
        [TestMethod]
        public void CheckDateTimeFormatIsCorrect()
        {
            var dateTime = new DateTimeOffset(2024, 11, 20, 08, 16, 14, TimeSpan.FromHours(1));
            var program = new Program(null);
            var expectedDateTime = "Wednesday 20 November 2024 08:16:14";
            string actualDateTime = null;

            program.DisplayDateTime("Test Label", dateTime);

            var fieldInfo = typeof(Program).GetField(
                "dateTimeStr", 
                BindingFlags.NonPublic | 
                BindingFlags.Instance);

            if (fieldInfo != null)
            {
                actualDateTime = (string)fieldInfo.GetValue(program);
            } 
            else
            {
                throw new Exception("fieldInfo is null for the variable dateTimeStr");
            }

            Assert.AreEqual(expectedDateTime, actualDateTime);
        }
    }
}