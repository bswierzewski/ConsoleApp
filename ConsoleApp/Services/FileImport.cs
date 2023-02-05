using ConsoleApp.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp.Services
{
    public class FileImport : IImportProvider
    {
        public IEnumerable<string> GetImportedLines(string fileName)
        {
            using (var streamReader = new StreamReader(fileName))
            {
                while (!streamReader.EndOfStream)
                {
                    yield return streamReader.ReadLine();
                }
            }
        }
    }
}
