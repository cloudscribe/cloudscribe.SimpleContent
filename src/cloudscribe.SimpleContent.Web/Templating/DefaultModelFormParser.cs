// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-22
// Last Modified:           2018-06-22
// 

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

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
                    if(!string.IsNullOrWhiteSpace(formVal))
                    {
                        try
                        {
                            if(prop.PropertyType == typeof(DateTime?) || prop.PropertyType == typeof(DateTime))
                            {
                                prop.SetValue(model, Convert.ToDateTime(formVal), null);
                            }
                            else if(prop.PropertyType == typeof(decimal?))
                            {
                                prop.SetValue(model, Convert.ToDecimal(formVal), null);
                            }
                            else if (prop.PropertyType == typeof(int?))
                            {
                                prop.SetValue(model, Convert.ToInt32(formVal), null);
                            }
                            else if (prop.PropertyType == typeof(long?))
                            {
                                prop.SetValue(model, Convert.ToInt64(formVal), null);
                            }
                            else if (prop.PropertyType == typeof(double?))
                            {
                                prop.SetValue(model, Convert.ToDouble(formVal), null);
                            }
                            else if (prop.PropertyType == typeof(bool?))
                            {
                                prop.SetValue(model, Convert.ToBoolean(formVal), null);
                            }
                            else
                            {
                                prop.SetValue(model, Convert.ChangeType(formVal, prop.PropertyType), null);
                            }

                            
                        }
                        catch(Exception ex)
                        {
                            Log.LogError($"failed to set property {prop.Name} using {formVal}. error:{ex.Message}:{ex.StackTrace}");
                        }
                        
                        Log.LogDebug($"value {formVal} found for {prop.Name}");
                    }
                    else
                    {
                        Log.LogDebug($"no form value found for {prop.Name}");
                    }
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
