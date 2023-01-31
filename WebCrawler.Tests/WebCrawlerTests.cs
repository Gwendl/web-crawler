using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WebCrawler.Tests
{
    public class WebCrawlerTests
    {
        private readonly IAmTheTest WebCrawler;
        private readonly IWebBrowser Browser;

        public WebCrawlerTests()
        {
            WebCrawler = new WebCrawler();
            Browser = new WebBrowser();
        }

        [Fact]
        public void TestDepth0()
        {
            var emails = WebCrawler.GetEmailsInPageAndChildPages(Browser, "./pages/index.html", 0);
            Assert.Equal(emails, new List<string>() { "nullepart@mozilla.org" });
        }

        [Fact]
        public void TestDepth1()
        {
            var expectedEmails = new List<string>() { "nullepart@mozilla.org", "ailleurs@mozilla.org" };
            var actualEmails = WebCrawler.GetEmailsInPageAndChildPages(Browser, "./pages/index.html", 1);
            Assert.Equal(expectedEmails.OrderBy(e => e), actualEmails.OrderBy(e => e));
        }

        [Fact]
        public void TestDepth2()
        {
            var expectedEmails = new List<string>() { "nullepart@mozilla.org", "ailleurs@mozilla.org", "loin@mozilla.org" };
            var actualEmails = WebCrawler.GetEmailsInPageAndChildPages(Browser, "./pages/index.html", 2);
            Assert.Equal(expectedEmails.OrderBy(e => e), actualEmails.OrderBy(e => e));
        }
    }
}