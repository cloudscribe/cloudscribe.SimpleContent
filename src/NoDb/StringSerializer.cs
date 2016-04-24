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
        public string Serialize(T obj)
        {
            return JsonConvert.SerializeObject(
                obj,
                Formatting.None,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include }
                );
        }

        public T Deserialize(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) throw new ArgumentException("must pass in a string");
            return JsonConvert.DeserializeObject<T>(s);
        }


    }
}
