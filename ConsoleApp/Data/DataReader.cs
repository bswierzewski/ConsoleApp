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

        // This method prints information about the databases in the imported objects list
        public void PrintData()
        {
            // Loop through each database in the imported objects list
            foreach (var database in ImportedObjects.Where(x => x.IsDatabase))
            {
                // Print the name and number of tables for the database
                _printProvider.Print($"Database '{database.Name}' ({database.NumberOfChildren} tables)");

                // Call the PrintTables method to print information about the tables in the database
                PrintTables(database.Children);
            }
        }

        // This method prints information about the tables in a database
        private void PrintTables(IEnumerable<ImportedObject> tables)
        {
            // Loop through each table in the tables list
            foreach (var table in tables)
            {
                // Print the schema and name of the table, as well as the number of columns in the table
                _printProvider.Print($"\tTable '{table.Schema}.{table.Name}' ({table.NumberOfChildren} columns)");

                // Call the PrintColumns method to print information about the columns in the table
                PrintColumns(table.Children);
            }
        }

        // This method prints information about the columns in a table
        private void PrintColumns(IEnumerable<ImportedObject> columns)
        {
            // Loop through each column in the columns list
            foreach (var column in columns)
            {
                // Print the name of the column, its data type, and whether or not it accepts null values
                _printProvider.Print($"\t\tColumn '{column.Name}' with {column.DataType} data type {(column.IsNullable ? "accepts nulls" : "with no nulls")}");
            }
        }
    }
}