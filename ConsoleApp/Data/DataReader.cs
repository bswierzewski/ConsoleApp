using ConsoleApp.Interfaces;
using ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Data
{
    public class DataReader
    {
        // Constant value used to identify if a value in a csv is nullable
        private const string NullableValueInCSV = "1";
        // Import provider instance to get csv data from
        private readonly IImportProvider _importProvider;
        // Printer provider instance to print results to
        private readonly IPrintProvider _printProvider;
        // Logger instance to log errors
        private readonly ILogger _logger;

        // Collection of ImportedObjects that are processed from the csv data
        public List<ImportedObject> ImportedObjects { get; set; } = new List<ImportedObject>();

        public DataReader(IImportProvider importProvider,
            IPrintProvider printerProvider,
            ILogger logger)
        {
            _importProvider = importProvider;
            _printProvider = printerProvider;
            _logger = logger;
        }

        /// <summary>
        /// </summary>
        /// <param name="delimeter">Delimeter used in CSV</param>
        public void ImportData(string fileName, char delimeter = ';')
        {
            // Clear list
            ImportedObjects = new List<ImportedObject>();

            // Get imported csv lines from provider
            var importedLines = _importProvider.GetImportedLines(fileName);

            // Loop through each line in the imported lines
            foreach (var line in importedLines)
            {
                // If is line is empty skip
                if (string.IsNullOrEmpty(line))
                {
                    _logger.Error("Empty line skip iteration");
                    continue;
                }

                // Clean and split line into an array of values
                var values = line.Trim()
                    .Replace(" ", "")
                    .Replace(Environment.NewLine, "")
                    .Split(delimeter);

                // Check if the length of the values array is 7 to prevent IndexOutOfRange
                if (values.Length != 7)
                {
                    _logger.Error($"Parse error with line: {line}");
                    continue;
                }

                // Add the newly created ImportedObject to the ImportedObjects list
                ImportedObjects.Add(CreateImportedObject(values));
            }

            // Private method to assing children to thier parent
            AssingChildrenToObjects();
        }

        // Private method to assign children to objects based on parent values
        private void AssingChildrenToObjects()
        {
            // Loop through each ImportedObject
            foreach (var importedObject in ImportedObjects)
            {
                // Assign children to the ImportedObject based on parent name and type
                importedObject.Children = ImportedObjects.Where(x => x.Parent.Equals(importedObject)).ToList();
            }
        }

        private ImportedObject CreateImportedObject(string[] values)
        {
            // Create a new instance of ImportedObject and set its properties based on the values passed in the string array
            var importedObject = new ImportedObject
            {
                Type = values[0],
                Name = values[1],
                Schema = values[2],
                Parent = new ImportedObjectBaseClass
                {
                    Name = values[3],
                    Type = values[4],
                },
                DataType = values[5],
                IsNullable = values[6] == NullableValueInCSV
            };

            return importedObject;
        }

        /// <summary>
        /// Print records with database pattern
        /// </summary>
        public void PrintData()
        {
            // Loop through all the ImportedObjects and find the ones that are databases
            foreach (var database in ImportedObjects.Where(x => x.IsDatabase))
            {
                // Add a string to the results that represents the database information
                _printProvider.Print($"Database '{database.Name}' ({database.NumberOfChildren} tables)");
                // Add the results of GetTableResults to the results list
                PrintTables(database.Children);
            }
        }

        private void PrintTables(IEnumerable<ImportedObject> tables)
        {
            // Loop through all the ImportedObjects and find the ones that are tables
            foreach (var table in tables)
            {
                _printProvider.Print($"\tTable '{table.Schema}.{table.Name}' ({table.NumberOfChildren} columns)");

                // Add the results of GetColumnResults to the results list
                PrintColumns(table.Children);
            }
        }

        private void PrintColumns(IEnumerable<ImportedObject> columns)
        {
            foreach (var column in columns)
                _printProvider.Print($"\t\tColumn '{column.Name}' with {column.DataType} data type {(column.IsNullable ? "accepts nulls" : "with no nulls")}");
        }
    }
}
