using System.Collections.Generic;

namespace ConsoleApp.Interfaces
{ 
    public interface IImportProvider
    {
        // Gets a collection of lines imported from the specified file name.
        IEnumerable<string> GetImportedLines(string fileName); 
    }
}
