using System;

namespace Serialization.Sample
{
    [Serializable]
    public class ParentExampleClass
    {
        public DateTime DateProperty { get; set; }
        public NestedExampleClass ClassProperty { get; set; }
    }
}