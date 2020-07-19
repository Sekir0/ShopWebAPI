using System;
using System.Collections.Generic;
using System.Text;

namespace ShopWebApi.Contracts.V1.Responses.Cart
{
    public class CartResponse
    {
        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }
    }
}
