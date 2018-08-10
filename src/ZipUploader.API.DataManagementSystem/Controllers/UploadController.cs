using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ZipUploader.Contracts.Models;
using ZipUploader.Contracts.Services;

namespace ZipUploader.API.DataManagementSystem.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUploadService _uploadService;

        public UploadController(
            IConfiguration configuration,
            IUploadService uploadService)
        {
            _configuration = configuration;
            _uploadService = uploadService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value)
        {
            const string cipherSecretKey = "CipherKey";

            // Get a decryption key from the configuration file:
            var cipherKey = _configuration.GetValue<string>(cipherSecretKey);

            var response = await _uploadService.StoreJsonData(new StoreJsonDataRequest
            {
                CipherKey = cipherKey,
                Json = value
            });

            return response == default(int)
                ? StatusCode(StatusCodes.Status500InternalServerError)
                : Ok();
        }
    }
}
