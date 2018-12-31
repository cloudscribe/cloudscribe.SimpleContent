// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-22
// Last Modified:           2018-07-10
// 

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace cloudscribe.SimpleContent.Web.Templating
{
    public class DefaultModelFormParser : IParseModelFromForm
    {
        public DefaultModelFormParser(
            ILogger<DefaultModelFormParser> logger
            )
        {
            Log = logger;
        }

        protected ILogger Log { get; private set; }

        public string ParserName { get; } = "DefaultModelFormParser";

        public virtual object ParseModel(string modelType, IFormCollection form)
        {
            if (string.IsNullOrWhiteSpace(modelType))
            {
                Log.LogError("modelType was empty");
                return null;
            }
            if(form == null)
            {
                Log.LogError("form was null");
                return null;
            }

            try
            {
                var type = Type.GetType(modelType);
                var model = Activator.CreateInstance(type);
                var modelProps = model.GetType().GetProperties();
                foreach(var prop in modelProps)
                {
                    var formVal = form[prop.Name].ToString();
                    if(string.IsNullOrWhiteSpace(formVal))
                    { 
                        Log.LogDebug($"no form value found for {prop.Name}");
                    }
                    
                    try
                    {
                        if(prop.PropertyType == typeof(DateTime?) || prop.PropertyType == typeof(DateTime))
                        {
                            if (!string.IsNullOrWhiteSpace(formVal))
                            {
                                prop.SetValue(model, Convert.ToDateTime(formVal), null);
                            }
                                    
                        }
                        else if(prop.PropertyType == typeof(decimal?) || prop.PropertyType == typeof(decimal))
                        {
                            if (!string.IsNullOrWhiteSpace(formVal))
                            {
                                prop.SetValue(model, Convert.ToDecimal(formVal), null);
                            }    
                        }
                        else if (prop.PropertyType == typeof(int?) || prop.PropertyType == typeof(int))
                        {
                            if (!string.IsNullOrWhiteSpace(formVal))
                            {
                                prop.SetValue(model, Convert.ToInt32(formVal), null);
                            }     
                        }
                        else if (prop.PropertyType == typeof(long?) || prop.PropertyType == typeof(long))
                        {
                            if (!string.IsNullOrWhiteSpace(formVal))
                            {
                                prop.SetValue(model, Convert.ToInt64(formVal), null);
                            }       
                        }
                        else if (prop.PropertyType == typeof(double?) || prop.PropertyType == typeof(double))
                        {
                            if (!string.IsNullOrWhiteSpace(formVal))
                            {
                                prop.SetValue(model, Convert.ToDouble(formVal), null);
                            }     
                        }
                        else if (prop.PropertyType == typeof(bool?) || prop.PropertyType == typeof(bool))
                        {
                            if (!string.IsNullOrWhiteSpace(formVal))
                            {
                                prop.SetValue(model, Convert.ToBoolean(formVal), null);
                            }      
                        }
                        else if (prop.PropertyType == typeof(Guid?) || prop.PropertyType == typeof(Guid))
                        {
                            if (!string.IsNullOrWhiteSpace(formVal) && formVal.Length == 36)
                            {
                                prop.SetValue(model, new Guid(formVal), null);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(formVal))
                            {
                                prop.SetValue(model, Convert.ChangeType(formVal, prop.PropertyType), null);
                            } 
                            else
                            {
                                foreach(Type interfaceType in prop.PropertyType.GetInterfaces())
                                {
                                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IList<>))
                                    {
                                        Type itemType = prop.PropertyType.GetGenericArguments()[0];
                                        Log.LogDebug($"found Ilist {prop.Name}");
                                        var jsonVal = WebUtility.UrlDecode(form[prop.Name + "Json"].ToString());
                                        if (!string.IsNullOrWhiteSpace(jsonVal))
                                        {
                                            Log.LogDebug($"found json for {prop.Name}");
                                            var jsonSettings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include };
                                            var obj = JsonConvert.DeserializeObject(jsonVal, prop.PropertyType, jsonSettings);
                                            prop.SetValue(model, obj, null);

                                        }

                                        break;
                                    }
                                }
                            }
                        }

                            
                    }
                    catch(Exception ex)
                    {
                        Log.LogError($"failed to set property {prop.Name} using {formVal}. error:{ex.Message}:{ex.StackTrace}");
                    }
                        
                    Log.LogDebug($"value {formVal} found for {prop.Name}");
                    
                    
                }

                return model;

            }
            catch(Exception ex)
            {
                Log.LogError($"{ex.Message}:{ex.StackTrace}");
                return null;
            }
            

            
        }


    }
}
