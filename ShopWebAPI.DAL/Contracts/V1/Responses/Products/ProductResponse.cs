using ShopWebAPI.DAL.Contracts.V1.Responses.Categorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWebAPI.DAL.Contracts.V1.Responses
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
        public string UserId { get; set; }
        public IEnumerable<CategoryResponse> Categorys { get; set; }
    }
}
