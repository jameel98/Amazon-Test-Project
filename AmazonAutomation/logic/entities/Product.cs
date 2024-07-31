namespace AmazonProject.Entities
{
    public class Product
    {
        // properties with getters and setters
        public string Name { get; set; }
        public string Price { get; set; }
        public string Url { get; set; }

        /// <summary>
        /// constructor for product objec
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="url"></param>
        public Product(string name, string price, string url)
        {
            Name = name;
            Price = price;
            Url = url;
        }

        /// <summary>
        /// function used to write the product details on the file
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Url},{Price},{Name}";
        }
    }
}
