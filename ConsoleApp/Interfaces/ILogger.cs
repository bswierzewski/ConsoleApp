using System;

namespace ConsoleApp.Interfaces
{
    public interface ILogger
    {
        // Log error with custom message 
        void Error(string message);

        // Log error with message from exception
        void Error(Exception exception);
    }
}
