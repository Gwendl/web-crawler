namespace WebCrawler.Tests
{
    public class WebBrowser : IWebBrowser
    {
        public string GetHtml(string url)
        {
            return File.ReadAllText(url);
        }
    }
}
