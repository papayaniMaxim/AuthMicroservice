using System;
using Microsoft.AspNetCore.Mvc;

namespace AuthMicroservice.Controllers
{
    public class HelloController : Controller
    {
        [HttpGet]
        [Route("api/hello")]
        public IActionResult Get()
        {
            return Ok("Hello, World!");
        }
    }
}

