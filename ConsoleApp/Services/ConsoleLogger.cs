using ConsoleApp.Interfaces;
using System;

namespace ConsoleApp.Services
{
    public class ConsoleLogger : ILogger
    {
        // Display custom error message in console
        public void Error(string message)
            => Console.WriteLine($"ERROR: {message}");
        
        // Display exception message in console
        public void Error(Exception exception)
            => Console.WriteLine(exception.ToString());
    }
}
