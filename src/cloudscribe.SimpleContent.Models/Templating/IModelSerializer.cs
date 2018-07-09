using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.SimpleContent.Models
{
    public interface IModelSerializer
    {
        string Serialize(string typeName, object obj);
        object Deserialize(string typeName, string serializedObject);
        string Name { get; }
    }
}
