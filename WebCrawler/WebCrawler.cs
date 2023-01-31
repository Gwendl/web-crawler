using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class WebCrawler : IAmTheTest
    {
        public List<string> GetEmailsInPageAndChildPages(IWebBrowser browser, string url, int maximumDepth)
        {
            var client = new WebCrawlerClient(browser, url);
            var crawledEmails = client.CrawlForEmails(maximumDepth);
            return crawledEmails.Select(e => e.Email).ToList();
        }
    }
}
