namespace WebCrawler
{
    internal record TargetWebPage
    {
        public readonly string Url;
        public readonly int Depth;

        public TargetWebPage(string url, int depth)
        {
            Url = url;
            Depth = depth;
        }
    }
}
