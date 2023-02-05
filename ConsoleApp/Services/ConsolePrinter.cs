using ConsoleApp.Interfaces;
using System;

namespace ConsoleApp.Services
{
    /// <summary>
    /// Console implementation IPrintProvider
    /// </summary>
    public class ConsolePrinter : IPrintProvider
    {
        public void Print(string text)
            => Console.WriteLine(text);
    }
}
