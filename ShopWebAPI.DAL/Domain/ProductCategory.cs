using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShopWebAPI.DAL.Domain
{
    public class ProductCategory
    {
        [ForeignKey(nameof(CategoryName))]
        public virtual Category Category { get; set; }

        public string CategoryName { get; set; }

        public virtual Product Product { get; set; }

        public Guid ProductId { get; set; }
    }
}
