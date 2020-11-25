namespace RTX3000_notifier.Model
{
    public interface IWebsite
    {
        public string Url { get; set; }

        public Stock GetStock();

        public string GetProductUrl(Videocard card);
    }
}
