using System;

namespace ConsoleApp.Models
{
    public class ImportedObjectBaseClass
    {
        public string Name { get; set; }
        public string Type { get; set; }

        /// <summary>
        /// This method compares the current ImportedObjectBaseClass object with the given one to determine
        /// if they are equal or not.It returns a boolean value indicating the result of the comparison.
        /// The comparison is case-insensitive and culture-invariant.
        /// </summary>
        public bool Equals(ImportedObjectBaseClass importedObject)
         => importedObject != null
            && string.Equals(Name, importedObject.Name, StringComparison.InvariantCultureIgnoreCase)
            && string.Equals(Type, importedObject.Type, StringComparison.InvariantCultureIgnoreCase);
    }
}
