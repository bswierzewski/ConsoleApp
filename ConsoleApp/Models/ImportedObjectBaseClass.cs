using System;

namespace ConsoleApp.Models
{
    public class ImportedObjectBaseClass
    {
        public string Name { get; set; }
        public string Type { get; set; }
        
        public bool Equals(ImportedObjectBaseClass importedObject)
         => importedObject != null
            && string.Equals(Name, importedObject.Name, StringComparison.InvariantCultureIgnoreCase)
            && string.Equals(Type, importedObject.Type, StringComparison.InvariantCultureIgnoreCase);
    }
}
