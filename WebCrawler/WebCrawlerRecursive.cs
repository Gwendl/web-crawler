using System.Xml.Linq;

namespace WebCrawler
{
    public class WebCrawlerRecursive : IAmTheTest
    {
        public List<string> GetEmailsInPageAndChildPages(IWebBrowser browser, string url, int maximumDepth)
        {
            var html = browser.GetHtml(url);
            var xml = XDocument.Parse(html);
            var hrefAttributes = xml.Descendants("a").Attributes("href");
            var emails = GetEmails(hrefAttributes);
            if (maximumDepth <= 0) return emails.Distinct().ToList();
            var childPages = GetChildPages(hrefAttributes);
            var childPagesEmails = childPages.SelectMany(p => GetEmailsInPageAndChildPages(browser, p, maximumDepth - 1)).ToList();
            return emails.Concat(childPagesEmails).Distinct().ToList();
        }

        private string[] GetEmails(IEnumerable<XAttribute> hrefAttributes)
        {
            return hrefAttributes
                .Where(attr => attr.Value.StartsWith("mailto:"))
                .Select(attr => attr.Value.Replace("mailto:", ""))
                .ToArray();
        }

        private string[] GetChildPages(IEnumerable<XAttribute> hrefAttributes)
        {
            return hrefAttributes
                .Where(attr => !attr.Value.StartsWith("mailto:"))
                .Select(attr => attr.Value)
                .ToArray();
        }
    }
}
