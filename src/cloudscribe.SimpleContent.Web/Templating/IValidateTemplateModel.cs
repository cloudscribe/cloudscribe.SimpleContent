using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContent.Web.Templating
{
    public interface IValidateTemplateModel
    {
        string ValidatorName { get; }
        bool IsValid(object model, ValidationContext validationContext, List<ValidationResult> results);
    }
}
