using Microsoft.AspNetCore.Mvc;
using ShopWebAPI.Contracts.V1;
using System;
using System.Collections.Generic;


namespace ShopWebAPI.Controllers.v1
{
    public class TestController : Controller
    {
        private List<Test> _test;

        public TestController()
        {
            _test = new List<Test>();
            for (var i = 0; i < 5; i++)
            {
                _test.Add(new Test { Id = Guid.NewGuid().ToString() });
            }
        }


        [HttpGet(ApiRoutes.GetAll)]
        public IActionResult Get()
        {
            return Ok(_test);
        }
    }
}
