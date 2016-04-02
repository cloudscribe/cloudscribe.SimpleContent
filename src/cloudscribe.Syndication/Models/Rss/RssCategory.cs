using System.Collections.ObjectModel;

namespace cloudscribe.Syndication.Models.Rss
{
    /// <summary>
    /// Represents a category or tag to which a <see cref="RssFeed"/> or <see cref="RssItem"/> belongs.
    /// </summary>
    public class RssCategory
    {

        public RssCategory()
        {
            
        }

        public RssCategory(string value)
        {
            this.Value = value;
        }

        public RssCategory(string value, string domain) : this(value)
        {
            this.Domain = domain;
        }

        public RssCategory(Collection<string> value)
        {
            Guard.ArgumentNotNull(value, "value");

            //------------------------------------------------------------
            //	Build slash-delimited string for supplied taxonomy hierarchy
            //------------------------------------------------------------
            if (value.Count > 0)
            {
                string[] hierarchy = new string[value.Count];
                value.CopyTo(hierarchy, 0);

                this.Value = string.Join("/", hierarchy);
            }
        }

        public RssCategory(Collection<string> value, string domain) : this(value)
        {
            this.Domain = domain;
        }

        private string categoryDomain = string.Empty;
        /// <summary>
        /// Gets or sets a string that identifies the taxonomy in which the category is placed.
        /// </summary>
        /// <value>A string that identifies the taxonomy in which the category is placed. The default value is an empty string.</value>
        public string Domain
        {
            get
            {
                return categoryDomain;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    categoryDomain = string.Empty;
                }
                else
                {
                    categoryDomain = value.Trim();
                }
            }
        }

        private string categoryValue = string.Empty;

        /// <summary>
        /// Gets or sets a slash-delimited string that identifies a hierarchical position in the taxonomy.
        /// </summary>
        /// <value>A slash-delimited string that identifies a hierarchical position in the taxonomy. The default value is an empty string.</value>
        /// <remarks>
        ///     If the category represents a tag or is the root hierarchical position in the taxonomy, no slash-delimiter is necessary.
        /// </remarks>
        public string Value
        {
            get
            {
                return categoryValue;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    categoryValue = string.Empty;
                }
                else
                {
                    categoryValue = value.Trim();
                }
            }
        }
       

    }
}
