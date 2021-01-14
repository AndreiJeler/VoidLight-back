using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;

namespace VoidLight.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IFileService _fileService;

        public FileController(IFileService service)
        {
            _fileService = service;
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            return Ok(await _fileService.UploadFileAsync(Request.Form.Files[0]));
        }
    }
}
