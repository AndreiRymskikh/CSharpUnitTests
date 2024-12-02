namespace Tests.Tests
{
    [TestClass]
    public class GetTimeDifferenceTests
    {
        [TestMethod]
        public void GetTimeDifferenceReturnlsCorrectTimeDifferenceUkLocation()
        {
            var ukDateTime = new DateTimeOffset(2024, 11, 20, 08, 16, 14, TimeSpan.FromHours(0));
            var canadaDateTime = new DateTimeOffset(2024, 11, 20, 03, 16, 14, TimeSpan.FromHours(-5));
            var program = new Program(null);

            var consoleOutputReader = new ConsoleReader();
            program.DisplayTimeDifference("UK", ukDateTime.DateTime, canadaDateTime.DateTime);
            var result = consoleOutputReader.ConsoleOutput();

            var expectedOutput = "You are 5h ahead of Canada";
            Assert.AreEqual(expectedOutput, result);
        }

        [TestMethod]
        public void GetTimeDifferenceReturnlsCorrectTimeDifferenceCanadaLocation()
        {
            var ukDateTime = new DateTimeOffset(2024, 11, 20, 08, 16, 14, TimeSpan.FromHours(0));
            var canadaDateTime = new DateTimeOffset(2024, 11, 20, 03, 16, 14, TimeSpan.FromHours(-5));
            var program = new Program(null);

            var consoleOutputReader = new ConsoleReader();
            program.DisplayTimeDifference("Canada", ukDateTime.DateTime, canadaDateTime.DateTime);
            var result = consoleOutputReader.ConsoleOutput();

            var expectedOutput = "You are 5h behind UK";
            Assert.AreEqual(expectedOutput, result);
        }

        [TestMethod]
        public void GetTimeDifferenceReturnlsCorrectMessageWrongLocation()
        {
            var ukDateTime = new DateTimeOffset(2024, 11, 20, 08, 16, 14, TimeSpan.FromHours(0));
            var canadaDateTime = new DateTimeOffset(2024, 11, 20, 03, 16, 14, TimeSpan.FromHours(-5));
            var program = new Program(null);

            var consoleOutputReader = new ConsoleReader();
            program.DisplayTimeDifference("WrongLocation", ukDateTime.DateTime, canadaDateTime.DateTime);
            var result = consoleOutputReader.ConsoleOutput();

            var expectedOutput = "Wrong location value. It can be UK or Canada";
            Assert.AreEqual(expectedOutput, result);
        }
    }
}