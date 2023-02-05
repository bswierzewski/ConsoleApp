using ConsoleApp.Interfaces;
using System;

namespace ConsoleApp.Services
{
    public class ConsolePrinter : IPrintProvider
    {
        // Print text use WriteLine
        public void Print(string text)
            => Console.WriteLine(text);
    }
}
