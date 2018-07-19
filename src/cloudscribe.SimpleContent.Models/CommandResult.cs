using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public class CommandResult<T> where T : class
    {
        public CommandResult(T resultType, bool succeeded, List<string> errors)
        {
            Value = resultType;
            Succeeded = succeeded;
            ErrorMessages = errors;

        }

        public T Value { get; private set; }
        public bool Succeeded { get; private set; }
        public List<string> ErrorMessages { get; private set; }


    }
}
