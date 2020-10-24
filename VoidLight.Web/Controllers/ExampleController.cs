using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data.Business;

namespace VoidLight.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        private readonly IExampleService _exampleService;
        public ExampleController(IExampleService service)
        {
            _exampleService = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            return Ok(_exampleService.GetAll());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddExample(ExampleDto dto)
        {
            var example = await _exampleService.AddExample(dto);
            return Ok(example);
        }
    }
}
