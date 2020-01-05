using System;

namespace cloudscribe.MetaWeblog.Models
{
    public class MetaWeblogException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetaWeblogException"/> class. 
        /// Constructor to load properties
        /// </summary>
        /// <param name="code">
        /// Fault code to be returned in Fault Response
        /// </param>
        /// <param name="message">
        /// Message to be returned in Fault Response
        /// </param>
        public MetaWeblogException(string code, string message): base(message)
        {
            this.Code = code;
        }


        /// <summary>
        ///     Gets code is actually for Fault Code.  It will be passed back in the 
        ///     response along with the error message.
        /// </summary>
        public string Code { get; private set; }
    }
}
