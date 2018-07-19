using cloudscribe.SimpleContent.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class JsonModelSerializer : IModelSerializer
    {
        public JsonModelSerializer()
        {

        }

        public string Name { get; } = "Json";

        public string Serialize(string typeName, object obj)
        {
            //var type = Type.GetType(typeName);

            return JsonConvert.SerializeObject(
                obj,
                Formatting.None,
                new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Include
                    //, DateTimeZoneHandling = DateTimeZoneHandling.Utc  
                }
                );
        }

        public object Deserialize(string typeName, string serializedObject)
        {
            var type = Type.GetType(typeName);
            if (string.IsNullOrWhiteSpace(serializedObject)) throw new ArgumentException("must pass in a string");
            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include
                //,DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            return JsonConvert.DeserializeObject(serializedObject, type, settings);
        }
    }
}
