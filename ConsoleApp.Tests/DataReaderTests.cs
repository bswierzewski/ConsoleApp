using ConsoleApp.Data;
using ConsoleApp.Interfaces;
using ConsoleApp.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace ConsoleApp.Tests
{
    public class DataReaderTests
    {
        private Mock<IPrintProvider> _printProvider;
        private Mock<IImportProvider> _importProvider;
        private Mock<ILogger> _logger;

        private DataReader _dataReader;

        [SetUp]
        public void SetUp()
        {
            _printProvider = new Mock<IPrintProvider>();
            _importProvider = new Mock<IImportProvider>();
            _logger = new Mock<ILogger>();

            _dataReader = new DataReader(_importProvider.Object,
                _printProvider.Object,
                _logger.Object);
        }

        [Test]
        public void ImportData_ValidData_ImportedObjectsArePopulated()
        {
            // Arrange
            var fileName = "file.csv";
            var expectedImportedObjects = new List<ImportedObject>
            {
                new ImportedObject
                {
                    Type = "DB",
                    Name = "DB1",
                    Schema = "dbo",
                    Parent = new ImportedObjectBaseClass(),
                    DataType = "NVARCHAR",
                    IsNullable = true
                },
                new ImportedObject
                {
                    Type = "TBL",
                    Name = "TBL1",
                    Schema = "dbo",
                    Parent = new ImportedObjectBaseClass
                    {
                        Name = "DB1",
                        Type = "DB"
                    },
                    DataType = "INT",
                    IsNullable = false
                }
            };

            // Act
            _dataReader.ImportData(fileName);

            // Assert
            Assert.AreEqual(expectedImportedObjects.Count, _dataReader.ImportedObjects.Count);
            for (int i = 0; i < expectedImportedObjects.Count; i++)
            {
                Assert.AreEqual(expectedImportedObjects[i].Type, _dataReader.ImportedObjects[i].Type);
                Assert.AreEqual(expectedImportedObjects[i].Name, _dataReader.ImportedObjects[i].Name);
                Assert.AreEqual(expectedImportedObjects[i].Schema, _dataReader.ImportedObjects[i].Schema);
                Assert.AreEqual(expectedImportedObjects[i].Parent.Name, _dataReader.ImportedObjects[i].Parent.Name);
                Assert.AreEqual(expectedImportedObjects[i].Parent.Type, _dataReader.ImportedObjects[i].Parent.Type);
                Assert.AreEqual(expectedImportedObjects[i].DataType, _dataReader.ImportedObjects[i].DataType);
                Assert.AreEqual(expectedImportedObjects[i].IsNullable, _dataReader.ImportedObjects[i].IsNullable);
            }
        }

        [Test]
        public void PrintData_ShouldPrintCorrectInformationForDatabasesAndTables()
        {
            // Arrange
            var importedObjects = new List<ImportedObject>
            {
                new ImportedObject
                {
                    Type = "DB",
                    Name = "TestDB",
                    Children = new List<ImportedObject>
                    {
                        new ImportedObject
                        {
                            Type = "Table",
                            Schema = "dbo",
                            Name = "TestTable",
                            Children = new List<ImportedObject>
                            {
                                new ImportedObject
                                {
                                    Type = "Column",
                                },
                                new ImportedObject
                                {
                                    Type = "Column",
                                }
                            }
                        }
                    }
                }
            };

            _dataReader.ImportedObjects = importedObjects;

            // Act
            _dataReader.PrintData();

            // Assert
            _printProvider.Verify(p => p.Print("Database 'TestDB' (1 tables)"));
            _printProvider.Verify(p => p.Print("\tTable 'dbo.TestTable' (2 columns)"));
        } 
    }
}
