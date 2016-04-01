using System;

namespace cloudscribe.Syndication.Models.Rss
{
    /// <summary>
    /// Represents a form to submit a text query to a <see cref="RssFeed">feed's</see> publisher over the Common Gateway Interface (CGI).
    /// </summary>
    public class RssTextInput
    {
        public RssTextInput()
        {
            
        }

        private string textInputDescription = string.Empty;
        /// <summary>
        /// Gets or sets character data that provides a human-readable label explaining this form's purpose.
        /// </summary>
        /// <value>Character data that provides a human-readable label explaining this form's purpose.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        public string Description
        {
            get
            {
                return textInputDescription;
            }

            set
            {
                Guard.ArgumentNotNullOrEmptyString(value, "value");
                textInputDescription = value.Trim();
            }
        }

        private Uri textInputLink;
        /// <summary>
        /// Gets or sets the URL of the CGI script that handles the query.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents the URL of the CGI script that handles the query.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri Link
        {
            get
            {
                return textInputLink;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                textInputLink = value;
            }
        }

        private string textInputName = string.Empty;
        /// <summary>
        /// Gets or sets the name of the form component that contains the query.
        /// </summary>
        /// <value>The name of the form component that contains the query.</value>
        /// <remarks>
        ///     The value of this property <b>must</b> begin with a letter and contain only these characters: 
        ///     the letters A to Z in either case, numeric digits, colons (":"), hyphens ("-"), periods (".") and underscores ("_").
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        public string Name
        {
            get
            {
                return textInputName;
            }

            set
            {
                Guard.ArgumentNotNullOrEmptyString(value, "value");
                textInputName = value.Trim();
            }
        }

        private string textInputTitle = string.Empty;
        /// <summary>
        /// Gets or sets a value that labels the button used to submit the query.
        /// </summary>
        /// <value>A string value that labels the button used to submit the query.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        public string Title
        {
            get
            {
                return textInputTitle;
            }

            set
            {
                Guard.ArgumentNotNullOrEmptyString(value, "value");
                textInputTitle = value.Trim();
            }
        }


    }
}
