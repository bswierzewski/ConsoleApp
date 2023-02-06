using System;
using System.Collections.Generic;

namespace ConsoleApp.Models
{
    public class ImportedObject : ImportedObjectBaseClass
    {
        public string Schema { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public ImportedObjectBaseClass Parent { get; set; }
        public ICollection<ImportedObject> Children { get; set; } = new List<ImportedObject>();
        public int NumberOfChildren => Children.Count;

        /// <summary>
        /// This method returns a boolean value indicating whether the current object is of type "database" or not.
        /// The comparison is case-insensitive and culture-invariant.
        /// </summary>
        public bool IsDatabase
            => string.Equals(Type, "database", StringComparison.InvariantCultureIgnoreCase);

    }
}
