using ConsoleApp.Data;
using ConsoleApp.Interfaces;
using ConsoleApp.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

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
            var csv = new List<string>
            {
                "Type;Name;Schema;ParentName;ParentType;DataType;IsNullable",
                "DB;DB1;dbo;;;NVARCHAR;",
                "",
                "TBL;TBL1;dbo;DB1;DB;INT;1",
                "DB;DB1;dbo;;;NVARCHAR;1;2;",
                "DB;DB1;"
            };

            _importProvider.Setup(x => x.GetImportedLines(It.IsAny<string>())).Returns(csv);

            // Act
            _dataReader.ImportData("");

            // Assert

            // Check log error
            _logger.Verify(p => p.Error("Line 2: Empty line, skip this line"));
            _logger.Verify(p => p.Error("Line 4: Parse error with values: DB;DB1;dbo;;;NVARCHAR;1;2;"));
            _logger.Verify(p => p.Error("Line 5: Parse error with values: DB;DB1;"));

            // Verify correct object mappings
            Assert.AreEqual(3, _dataReader.ImportedObjects.Count);

            var firstLine = _dataReader.ImportedObjects[0];

            Assert.AreEqual("Type", firstLine.Type);
            Assert.AreEqual("Name", firstLine.Name);
            Assert.AreEqual("Schema", firstLine.Schema);
            Assert.AreEqual("ParentName", firstLine.Parent.Name);
            Assert.AreEqual("ParentType", firstLine.Parent.Type);
            Assert.AreEqual("DataType", firstLine.DataType);
            Assert.AreEqual(false, firstLine.IsNullable);

            var db = _dataReader.ImportedObjects[1];

            Assert.AreEqual("DB", db.Type);
            Assert.AreEqual("DB1", db.Name);
            Assert.AreEqual("dbo", db.Schema);
            Assert.AreEqual("", db.Parent.Name);
            Assert.AreEqual("", db.Parent.Type);
            Assert.AreEqual("NVARCHAR", db.DataType);
            Assert.AreEqual(false, db.IsNullable);

            var children = _dataReader.ImportedObjects[1].Children.FirstOrDefault();

            Assert.AreEqual("TBL", children.Type);
            Assert.AreEqual("TBL1", children.Name);
            Assert.AreEqual("dbo", children.Schema);
            Assert.AreEqual("DB1", children.Parent.Name);
            Assert.AreEqual("DB", children.Parent.Type);
            Assert.AreEqual("INT", children.DataType);
            Assert.AreEqual(true, children.IsNullable);
        }

        [Test]
        public void ImportData_EmptyImportedLines_ShouldReturnAndLogError()
        {
            // Arrange
            _importProvider.Setup(x => x.GetImportedLines(It.IsAny<string>())).Returns(new List<string>());

            // Act
            _dataReader.ImportData("");

            // Assert
            _logger.Verify(p => p.Error("Imported lines is empty"));
        }

        [TestCase("value1, value2 , value3")]
        [TestCase("value1,value2,value3")]
        [TestCase("  value1,    value2 ,         value3\r\n")]
        [TestCase("value1\r\n,value2\r\n,value3\r\n")]
        public void PrepareValues_ShouldSplitValues(string line)
        {
            // Arrange
            char delimeter = ',';
            string[] expectedResult = new string[] { "value1", "value2", "value3" };

            // Act
            string[] result = _dataReader.PrepareValues(line, delimeter);

            // Assert
            CollectionAssert.AreEqual(expectedResult, result);
        }

        [Test]
        public void PrintData_ShouldPrintCorrectInformationForDatabasesAndTables()
        {
            // Arrange
            var importedObjects = new List<ImportedObject>
            {
                new ImportedObject
                {
                    Type = "DATABASE",
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
                                    Name = "TestColumn1",
                                    DataType = "INT",
                                    IsNullable = true,
                                },
                                new ImportedObject
                                {
                                    Name= "TestColumn2",
                                    DataType = "DOUBLE",
                                    IsNullable = false,
                                }
                            }
                        }
                    }
                }
            };

            _dataReader.ImportedObjects = importedObjects;

            // Act
            _dataReader.PrintDatabases();

            // Assert
            _printProvider.Verify(p => p.Print("Database 'TestDB' (1 tables)"));
            _printProvider.Verify(p => p.Print("\tTable 'dbo.TestTable' (2 columns)"));
            _printProvider.Verify(p => p.Print("\t\tColumn 'TestColumn1' with INT data type accepts nulls"));
            _printProvider.Verify(p => p.Print("\t\tColumn 'TestColumn2' with DOUBLE data type with no nulls"));
        }
    }
}
