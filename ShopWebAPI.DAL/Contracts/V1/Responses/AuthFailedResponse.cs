﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShopWebAPI.DAL.Contracts.V1.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
