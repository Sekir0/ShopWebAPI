using System;
using System.Collections.Generic;
using System.Text;

namespace ShopWebAPI.DAL.Models
{
    public class AuthenticatioResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
