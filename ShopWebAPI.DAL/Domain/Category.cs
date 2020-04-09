using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ShopWebAPI.DAL.Domain
{
    public class Category
    {
        [Key]
        public string Name { get; set; }

        public string CreatorId { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public IdentityUser CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
