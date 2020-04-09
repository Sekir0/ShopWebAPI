using System.Collections.Generic;


namespace ShopWebAPI.Contracts.V1.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
        public IEnumerable<string> Categorys { get; set; }
    }
}
