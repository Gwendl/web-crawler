using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    public interface IWebBrowser
    {
        // Returns null if the url could not be visited.
        string GetHtml(string url);
    }
}
