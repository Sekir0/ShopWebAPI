using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShopWebAPI.DAL.Domain
{
    public class Cart
    {
        public Guid CartId { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
