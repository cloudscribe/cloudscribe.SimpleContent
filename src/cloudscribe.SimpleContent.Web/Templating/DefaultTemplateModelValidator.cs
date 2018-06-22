// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-22
// Last Modified:           2018-06-22
// 

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContent.Web.Templating
{
    public class DefaultTemplateModelValidator : IValidateTemplateModel
    {

        public string ValidatorName { get; } = "DefaultTemplateModelValidator";

        public bool IsValid(object model, ValidationContext validationContext, List<ValidationResult> results)
        {
            return Validator.TryValidateObject(model, validationContext, results);
        }

    }
}
