using ConsoleApp.Data;
using ConsoleApp.Interfaces;
using Moq;
using NUnit.Framework;

namespace ConsoleApp.Tests
{
    public class DataReaderTests
    {
        private Mock<IPrintProvider> _printProvider;
        private Mock<IImportProvider> _importProvider;
        private Mock<ILogger> _logger;

        public DataReader DataReader { get; set; }

        [SetUp]
        public void SetUp()
        {
            _printProvider = new Mock<IPrintProvider>();
            _importProvider = new Mock<IImportProvider>();
            _logger = new Mock<ILogger>();

            DataReader = new DataReader(_importProvider.Object,
                _printProvider.Object,
                _logger.Object);
        }

        [Test]
        public void ImportData()
        {
        }

        [Test]
        public void PrintData()
        {
        }
    }
}
