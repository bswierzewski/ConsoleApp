using ConsoleApp.Data;
using ConsoleApp.Services;
using System;

namespace ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var fileName = "data.csv";

                if (args.Length > 0)
                    fileName = args[0];

                var reader = new DataReader(new FileImport(), new ConsolePrinter(), new EmptyLogger());

                reader.ImportData(fileName);
                reader.PrintData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }
    }
}
