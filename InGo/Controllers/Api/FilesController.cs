using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InGo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InGo.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileName _fileName;

        public FilesController(FileName fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Adds new file to server.
        /// </summary>
        /// <param name="file">File.</param>
        /// <returns>Url of the file.</returns>
        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            string path = _fileName.GetFileName(file);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(path.Replace("wwwroot", ""));
        }
    }
}