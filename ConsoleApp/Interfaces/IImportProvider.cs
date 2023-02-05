using System.Collections.Generic;

namespace ConsoleApp.Interfaces
{
    public interface IImportProvider
    {
        /// <summary>
        /// Get imported lines base on fileName
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IEnumerable<string> GetImportedLines(string fileName); 
    }
}
