using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VoidLight.Business.Services.Contracts
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}
