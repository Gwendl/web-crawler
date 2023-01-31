using System.Xml.Linq;

namespace WebCrawler
{
    internal class WebCrawlerClient
    {
        private Stack<TargetWebPage> Targets;
        private readonly TargetWebPage RootTarget;
        private readonly IWebBrowser WebBrowser;
        private readonly HashSet<string> VisitedPages;
        private readonly Dictionary<string, XDocument> XmlCache;
        private readonly Dictionary<string, XElement[]> LinksCache;

        public WebCrawlerClient(IWebBrowser browser, string url)
        {
            WebBrowser = browser;
            Targets = new();
            VisitedPages = new();
            XmlCache = new();
            LinksCache = new();
            RootTarget = new TargetWebPage(url, 0);
        }

        public List<CrawledEmail> CrawlForEmails(int maximumDepth)
        {
            List<CrawledEmail> crawledEmails = new();
            Targets.Push(RootTarget);
            while(Targets.Count > 0)
            {
                var target = Targets.Pop();
                var emails = GetEmails(target);
                crawledEmails.AddRange(emails);
                if (target.Depth >= maximumDepth)
                    continue;
                var childPages = GetChildPages(target);
                var targetChildPages = childPages.Where(p => !VisitedPages.Contains(p.Url));
                foreach(var page in targetChildPages)
                    Targets.Push(page);
            }
            return crawledEmails.Distinct().ToList();
        }

        private XDocument GetXML(TargetWebPage page)
        {
            if (XmlCache.ContainsKey(page.Url))
                return XmlCache[page.Url];
            return XDocument.Parse(WebBrowser.GetHtml(page.Url));
        }

        private XElement[] GetLinks(TargetWebPage page)
        {
            if (LinksCache.ContainsKey(page.Url))
                return LinksCache[page.Url];
            return GetXML(page)
                .Descendants("a")
                .ToArray();
        }

        private TargetWebPage[] GetChildPages(TargetWebPage page)
        {
            return GetLinks(page)
                .Select(l => l.Attribute("href"))
                .Where(attr => attr != null)
                .Cast<XAttribute>()
                .Where(attr => !attr.Value.StartsWith("mailto:"))
                .Select(attr => new TargetWebPage(attr.Value, page.Depth + 1))
                .ToArray();
        }

        private CrawledEmail[] GetEmails(TargetWebPage page)
        {
            return GetLinks(page)
                .Select(l => l.Attribute("href"))
                .Where(attr => attr != null)
                .Cast<XAttribute>()
                .Where(attr => attr.Value.StartsWith("mailto:"))
                .Select(attr => attr.Value.Replace("mailto:", ""))
                .Select(address => new CrawledEmail(address))
                .ToArray();
        }
    }
}
