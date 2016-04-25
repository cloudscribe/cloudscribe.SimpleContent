// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2016-04-24
// 


using Newtonsoft.Json;
using System;

namespace NoDb
{
    public class StringSerializer<T> : IStringSerializer<T> where T : class
    {
        public StringSerializer()
        {

        }

        public string ExpectedFileExtension { get; } = ".json";

        public string Serialize(T obj)
        {
            return JsonConvert.SerializeObject(
                obj,
                Formatting.None,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include }
                );
        }

        public T Deserialize(string serializedObject, string key = "")
        {
            // for json we are not using the passed in key, we are assuming the json object has its id seriailized as part of it
            // however the xml format for MiniBlog/BlogEngine Posts does not put the id in the xml, the key is just the filename minus extension
            // so we have to pass it in for that scenario, ie it is used in PostXmlSerializer

            if (string.IsNullOrWhiteSpace(serializedObject)) throw new ArgumentException("must pass in a string");
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }


    }
}
