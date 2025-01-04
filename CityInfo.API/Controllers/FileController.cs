using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensioncontentTypeProvider;

        public FileController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensioncontentTypeProvider = fileExtensionContentTypeProvider ?? throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("{fileName}")]
        public ActionResult GetFile(string fileName)
        {
            //var file = System.IO.File.OpenRead($"./Files/{fileName}");
            //return File(file, "application/octet-stream");
            var pathToFile = $"./Files/{fileName}";
            if(!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }
            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, "application/octet-stream", Path.GetFileName(pathToFile));
        }
    }
}
