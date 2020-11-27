using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data.Business;
using VoidLight.Data.Business.Authentication;
using VoidLight.Data.Entities;
using VoidLight.Infrastructure.Common;
using VoidLight.Web.Infrastructure.Authorization;

namespace VoidLight.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private IGamePublisherService _gamePublisherService;

        public PublishersController(IGamePublisherService gamePublisherService)
        {
            _gamePublisherService = gamePublisherService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetPublishers(int id)
        {
            return Ok(_gamePublisherService.GetGamePublisher());
        }
    }
}
