namespace Tests.Tests
{
    [TestClass]
    public class DisplayDateTimeTests
    {

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
    }
}