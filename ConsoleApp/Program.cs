using ConsoleApp.Data;
using ConsoleApp.Interfaces;
using ConsoleApp.Services;
using System;

namespace ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            ILogger _logger = new ConsoleLogger();

            try
            {
                // Set file name to "DataSource\data.csv" if no command-line argument is provided
                var fileName = @"DataSource\data.csv";
                if (args.Length == 1)
                    fileName = args[0];

                // Create a DataReader object with instances of the required services
                var reader = new DataReader(new FileImport(), new ConsolePrinter(), _logger);

                // Call the ImportData method to import the data from the specified file
                reader.ImportData(fileName);

                // Call the PrintData method to print the imported data
                reader.PrintDatabases();
            }
            catch (Exception ex)
            {
                // Log the exception details to the logger instance
                _logger.Error(ex);
            }

            // Wait for user input to keep the console window open
            Console.ReadLine();
        }
    }
}
