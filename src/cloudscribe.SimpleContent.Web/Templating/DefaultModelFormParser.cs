using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.Logging;

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
                            prop.SetValue(model, Convert.ChangeType(formVal, prop.PropertyType), null);
                        }
                        catch(Exception ex)
                        {
                            Log.LogError($"failed to set property {prop.Name} using {formVal}. error:{ex.Message}:{ex.StackTrace}");
                        }

                        

                        //model.GetType().InvokeMember(
                        //    prop.Name,
                        //    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                        //   Type.DefaultBinder, 
                        //   model,
                        //   formVal
                        //   );

                        Log.LogDebug($"value {formVal} found for {prop.Name}");
                    }
                    else
                    {
                        Log.LogInformation($"no form value found for {prop.Name}");
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
