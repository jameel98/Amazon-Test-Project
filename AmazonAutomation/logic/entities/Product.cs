namespace AmazonProject.Entities
{
    public class Product
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Url { get; set; }

        public Product(string name, string price, string url)
        {
            Name = name;
            Price = price;
            Url = url;
        }

        public override string ToString()
        {
            return $"{Name}, {Price}, {Url}";
        }
    }
}
