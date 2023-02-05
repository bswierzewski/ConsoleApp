using ConsoleApp.Interfaces;

namespace ConsoleApp.Services
{
    /// <summary>
    /// Empty logger implementation ILogger
    public class EmptyLogger : ILogger
    {
        public void Error(string message) { }
    }
}
