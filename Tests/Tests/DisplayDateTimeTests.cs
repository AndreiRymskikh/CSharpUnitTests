using Application.WorldTime;
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
            var worldTimeApi = new WorldTimeApi(null);
            var expectedOutput = "Test Label: Wednesday 20 November 2024 08:16:14";

            var consoleOutputReader = new ConsoleReader();
            worldTimeApi.DisplayDateTime("Test Label", dateTime);
            var result = consoleOutputReader.ConsoleOutput();

            Assert.AreEqual(expectedOutput, result);
        }

        //spies the value
        [TestMethod]
        public void CheckDateTimeFormatIsCorrect()
        {
            var dateTime = new DateTimeOffset(2024, 11, 20, 08, 16, 14, TimeSpan.FromHours(1));
            var worldTimeApi = new WorldTimeApi(null);
            var expectedDateTime = "Wednesday 20 November 2024 08:16:14";
            string actualDateTime = null;

            worldTimeApi.DisplayDateTime("Test Label", dateTime);

            var fieldInfo = typeof(WorldTimeApi).GetField(
                "dateTimeStr", 
                BindingFlags.NonPublic | 
                BindingFlags.Instance);

            if (fieldInfo != null)
            {
                actualDateTime = (string)fieldInfo.GetValue(worldTimeApi);
            } 
            else
            {
                throw new Exception("fieldInfo is null for the variable dateTimeStr");
            }

            Assert.AreEqual(expectedDateTime, actualDateTime);
        }
    }
}