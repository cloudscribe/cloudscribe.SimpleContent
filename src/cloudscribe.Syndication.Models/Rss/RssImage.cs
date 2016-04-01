using System;

namespace cloudscribe.Syndication.Models.Rss
{
    /// <summary>
    /// Represents a graphical logo for an <see cref="RssFeed"/>.
    /// </summary>
    public class RssImage
    {
        public RssImage()
        {
            
        }

        public RssImage(Uri link, string title, Uri url)
        {
            this.Link = link;
            this.Title = title;
            this.Url = url;
        }

        private string imageDescription = string.Empty;
        /// <summary>
        /// Gets or sets character data that provides a human-readable characterization of the site linked to this image.
        /// </summary>
        /// <value>Character data that provides a human-readable characterization of the site linked to this image.</value>
        /// <remarks>
        ///     The value of this property <i>should</i> be suitable for use as the <i>title</i> attribute of the <b>a</b> tag in an HTML rendering.
        /// </remarks>
        public string Description
        {
            get
            {
                return imageDescription;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    imageDescription = String.Empty;
                }
                else
                {
                    imageDescription = value.Trim();
                }
            }
        }

        private const int MAX_HEIGHT = 400;
        private const int MAX_WIDTH = 144;

        private int imageHeight = int.MinValue;
        /// <summary>
        /// Gets or sets the height of this image.
        /// </summary>
        /// <value>The height, in pixels, of this image. The default value is <see cref="Int32.MinValue"/>, which indicates no height was specified.</value>
        /// <remarks>
        ///     If no height is specified for the image, the image is assumed to be 31 pixels tall.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is greater than <i>400</i>.</exception>
        public int Height
        {
            get
            {
                return imageHeight;
            }

            set
            {
                Guard.ArgumentNotGreaterThan(value, "value", MAX_HEIGHT);
                imageHeight = value;
            }
        }

        private Uri imageLink;
        /// <summary>
        /// Gets or sets the URL of the web site represented by this image.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents the URL of the web site represented by this image.</value>
        /// <remarks>
        ///     The value of this property <i>should</i> be the same URL as the channel's <see cref="RssChannel.Link">link</see> property.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri Link
        {
            get
            {
                return imageLink;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                imageLink = value;
            }
        }

        private string imageTitle = string.Empty;
        /// <summary>
        /// Gets or sets character data that provides a human-readable description of this image.
        /// </summary>
        /// <value>Character data that provides a human-readable description of this image.</value>
        /// <remarks>
        ///     The value of this property <i>should</i> be the same as the channel's <see cref="RssChannel.Title">title</see> property 
        ///     and be suitable for use as the <i>alt</i> attribute of the <b>img</b> tag in an HTML rendering.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        public string Title
        {
            get
            {
                return imageTitle;
            }

            set
            {
                Guard.ArgumentNotNullOrEmptyString(value, "value");
                imageTitle = value.Trim();
            }
        }

        private Uri imageUrl;
        /// <summary>
        /// Gets or sets the URL of this image.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents the URL of this image.</value>
        /// <remarks>
        ///     The image <b>must</b> be in the <i>GIF</i>, <i>JPEG</i> or <i>PNG</i> formats.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri Url
        {
            get
            {
                return imageUrl;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                imageUrl = value;
            }
        }

        private int imageWidth = int.MinValue;

        /// <summary>
        /// Gets or sets the width of this image.
        /// </summary>
        /// <value>The width, in pixels, of this image. The default value is <see cref="Int32.MinValue"/>, which indicates no width was specified.</value>
        /// <remarks>
        ///     If no width is specified for the image, the image is assumed to be 88 pixels wide.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is greater than <i>144</i>.</exception>
        public int Width
        {
            get
            {
                return imageWidth;
            }

            set
            {
                Guard.ArgumentNotGreaterThan(value, "value", MAX_WIDTH);
                imageWidth = value;
            }
        }


    }
}
