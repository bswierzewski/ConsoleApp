using ConsoleApp.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp.Services
{
    public class FileImport : IImportProvider
    {
        // Implements the GetImportedLines method to return a collection of lines imported from a file.
        public IEnumerable<string> GetImportedLines(string fileName)
        {
            // Use a StreamReader to read the lines from the file.
            using (var streamReader = new StreamReader(fileName))
            {
                // Read each line from the file until the end of the stream is reached.
                while (!streamReader.EndOfStream)
                {
                    // Yield each line as it is read from the file.
                    yield return streamReader.ReadLine();
                }
            }
        }
    }
}
