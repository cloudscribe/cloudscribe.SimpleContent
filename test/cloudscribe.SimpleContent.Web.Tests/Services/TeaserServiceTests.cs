using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xunit;
using Moq;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class TeaserServiceTests
    {
        private const string testText1 = "The quick brown fox jumps over the lazy dog.";

        private readonly TeaserService teaserService;
        private readonly Mock<IPost> postMock;
        private readonly Mock<IProjectSettings> projectSettingsMock;

        public TeaserServiceTests()
        {
            teaserService = new TeaserService();
            postMock = new Mock<IPost>();
            projectSettingsMock = new Mock<IProjectSettings>();
        }

        [Theory]
        [Description("If teaser override is specified, always show it regardless of other settings.")]
        [InlineData(AutoTeaserMode.Off)]
        [InlineData(AutoTeaserMode.On)]
        public void ShouldDisplayTeaser_TeaserOverrideSpecified_ReturnsTrue(AutoTeaserMode mode)
        {
            postMock.Setup(m => m.TeaserOverride).Returns("FOO BAR");
            postMock.Setup(m => m.SuppressAutoTeaser).Returns(true);
            projectSettingsMock.Setup(m => m.AutoTeaserMode).Returns(mode);

            var post = postMock.Object;
            var projectSettings = projectSettingsMock.Object;

            Assert.True(teaserService.ShouldDisplayTeaser(projectSettings, post));
        }

        [Theory]
        [Description("If teaser override is not specified and suppress auto teaser is true, never show the teaser.")]
        [InlineData(AutoTeaserMode.Off)]
        [InlineData(AutoTeaserMode.On)]
        public void ShouldDisplayTeaser_SuppressAutoTeaserIsTrue_ReturnsFalse(AutoTeaserMode mode)
        {
            postMock.Setup(m => m.TeaserOverride).Returns("");
            projectSettingsMock.Setup(m => m.AutoTeaserMode).Returns(mode);
            postMock.Setup(m => m.SuppressAutoTeaser).Returns(true);

            var post = postMock.Object;
            var projectSettings = projectSettingsMock.Object;

            Assert.False(teaserService.ShouldDisplayTeaser(projectSettings, post));
        }


        //TODO: this test has bad logic
        // should not display a teaser unless content length in words chars or string length is greater than projectSettings.TeaserTruncationLength
        //[Fact]
        //[Description("If teaser override is not specified and suppress auto teaser is false, go with the auto teaser mode setting.")]
        //public void ShouldDisplayTeaser_AutoTeaserModeOn_ReturnsTrue()
        //{
        //    postMock.Setup(m => m.TeaserOverride).Returns("");
        //    projectSettingsMock.Setup(m => m.AutoTeaserMode).Returns(AutoTeaserMode.On);
        //    postMock.Setup(m => m.SuppressAutoTeaser).Returns(false);

        //    var post = postMock.Object;
        //    var projectSettings = projectSettingsMock.Object;

        //    Assert.True(teaserService.ShouldDisplayTeaser(projectSettings, post));
        //}

        [Fact]
        [Description("If teaser override is not specified and suppress auto teaser is false, go with the auto teaser mode setting.")]
        public void ShouldDisplayTeaser_AutoTeaserModeOff_ReturnsFalse()
        {
            postMock.Setup(m => m.TeaserOverride).Returns("");
            projectSettingsMock.Setup(m => m.AutoTeaserMode).Returns(AutoTeaserMode.Off);
            postMock.Setup(m => m.SuppressAutoTeaser).Returns(false);

            var post = postMock.Object;
            var projectSettings = projectSettingsMock.Object;

            Assert.False(teaserService.ShouldDisplayTeaser(projectSettings, post));
        }

        [Fact]
        [Description("Length truncation mode (lifted from Humanizer) truncates to a fixed length, inclusive of whitespace.")]
        public void TruncatePost_LengthTruncationMode_TruncatesToFixedLength()
        {
            var result = teaserService.TruncatePost(TeaserTruncationMode.Length, testText1, 25);
            Assert.Equal("The quick brown fox ju...", result);
        }

        [Fact]
        [Description("Character truncation mode (lifted from Humanizer) truncates to a fixed number of characters, exclusive of whitespace.")]
        public void TruncatePost_CharacterTruncationMode_TruncatesToFixedLength()
        {
            var result = teaserService.TruncatePost(TeaserTruncationMode.Character, testText1, 25);
            Assert.Equal("The quick brown fox jumps o...", result);
        }

        [Fact]
        [Description("Word truncation mode (lifted from Humanizer) truncates to a fixed number of words.")]
        public void TruncatePost_WordTruncationMode_TruncatesToFixedLength()
        {
            var result = teaserService.TruncatePost(TeaserTruncationMode.Word, testText1, 7);
            Assert.Equal("The quick brown fox jumps over the...", result);
        }

        // this one fails because we now have logic to increase the truncation length until valid html results
        //[Fact]
        //[Description("If HTML tags would be cut off, make sure they are closed.")]
        //public void CreateTeaser_HtmlTagsWouldBeCutOff_TagsGetClosed()
        //{
        //    postMock.Setup(m => m.TeaserOverride).Returns("");
        //    postMock.Setup(m => m.SuppressAutoTeaser).Returns(false);
        //    projectSettingsMock.Setup(m => m.AutoTeaserMode).Returns(AutoTeaserMode.On);
        //    projectSettingsMock.Setup(m => m.TeaserTruncationMode).Returns(TeaserTruncationMode.Length);
        //    projectSettingsMock.Setup(m => m.TeaserTruncationLength).Returns(30);

        //    var post = postMock.Object;
        //    var projectSettings = projectSettingsMock.Object;

        //    var input = "<p>The quick <b>brown fox jumps over the lazy dog.</b></p>";
        //    var expected = "<p></p>The quick <b>brown fox j...</b>";
        //    var result = teaserService.CreateTeaser(projectSettings, post, input);
        //    Assert.Equal(expected, result);
        //}

        // Joe A comment 2018-01-22
        // using charcter trunction set at 400
        // this results in malformed html that breaks the read more link
        // double quotes changed manually here to single quotes
        // notice bad a link in output
        string badInputHtml = @"<h3 id=""markdown-is-cool"">Markdown is cool!</h3>
<p>Once you get to know it</p>
<pre><code>private MarkdownPipeline _mdPipeline = null;

        public string FilterHtml(IPage p)
        {
            if (p.ContentType == &quot; markdown & quot;)
        {
                if (_mdPipeline == null)
                {
                    _mdPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                }
                return Markdown.ToHtml(p.Content, _mdPipeline);
            }

            return filter.FilterHtml(
                p.Content,
                ProjectSettings.CdnUrl,
                ProjectSettings.LocalMediaVirtualPath);
        }
			
</code></pre>
<p>then add an image</p>
<p><a href=""/media/images/img_1349.jpg""><img src= ""/media/images/img_1349-ws.jpg"" alt= ""my pond"" /></a></p>";


        string badOutputHtml = @"<h3 id=""markdown-is-cool"">Markdown is cool!</h3>
<p>Once you get to know it</p>
<pre><code>private MarkdownPipeline _mdPipeline = null;

        public string FilterHtml(IPage p)
        {
            if (p.ContentType == &quot; markdown & quot;)
        {
                if (_mdPipeline == null)
                {
                    _mdPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                }
                return Markdown.ToHtml(p.Content, _mdPipeline);
            }

            return filter.FilterHtml(
                p.Content,
                ProjectSettings.CdnUrl,
                ProjectSettings.LocalMediaVirtualPath);
        }
			
</code></pre>
<p>then add an image</p>
<p></p><a href=""/media/imag...";

        [Fact]
        [Description("If HTML tags would be cut off, make sure they are closed.")]
        public void CreateTeaser_ShouldNotCreateBadOutput1()
        {
            postMock.Setup(m => m.TeaserOverride).Returns("");
            postMock.Setup(m => m.SuppressAutoTeaser).Returns(false);
            projectSettingsMock.Setup(m => m.AutoTeaserMode).Returns(AutoTeaserMode.On);
            projectSettingsMock.Setup(m => m.TeaserTruncationMode).Returns(TeaserTruncationMode.Character);
            projectSettingsMock.Setup(m => m.TeaserTruncationLength).Returns(400);

            var post = postMock.Object;
            var projectSettings = projectSettingsMock.Object;

            var input = badInputHtml;
            var expected = badOutputHtml + " href=\"\"></a href=\"/media/imag...>";
            var result = teaserService.CreateTeaser(projectSettings, post, input);
            bool bad = expected == result;
            Assert.False(bad);
        }

        



    }
}