using ConsoleApp.Interfaces;

namespace ConsoleApp.Services
{
    public class EmptyLogger : ILogger
    {
        // Mock error message
        public void Error(string message) { }
    }
}
