namespace WebCrawler
{
    internal record CrawledEmail
    {
        public readonly string Email;

        public CrawledEmail(string email)
        {
            Email = email;
        }
    }
}
