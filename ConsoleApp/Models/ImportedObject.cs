using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool IsDatabase
            => string.Equals(Type, "database", StringComparison.InvariantCultureIgnoreCase);

    }
}
