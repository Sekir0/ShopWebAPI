using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWebAPI.DAL.Contracts.V1.Requests
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
