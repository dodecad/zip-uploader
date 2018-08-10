using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ZipUploader.Common.Web.Helpers;
using ZipUploader.Contracts.Models;
using ZipUploader.Contracts.Services;
using ZipUploader.UI.Web.ControlPanel.Models;

namespace ZipUploader.UI.Web.ControlPanel.Pages.ZipUpload
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IUploadService _uploadService;

        public IndexModel(
            IConfiguration configuration,
            IUploadService uploadService)
        {
            _configuration = configuration;
            _uploadService = uploadService;
        }

        [BindProperty]
        public FileUpload FileUpload { get; set; }

        public string Message { get; private set; }

        public void OnGet()
        {
            Message = "Please fill out the form below:";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            const string apiUrlKey = "ApiUrl";
            const string cipherSecretKey = "CipherKey";
            const string redirectPage = "../Index";

            if (ModelState.IsValid == false)
                return Page();

            var path = await FileHelpers.SaveFile(FileUpload.File, ModelState);

            // Perform a second check to catch SaveFile method violations:
            if (ModelState.IsValid == false)
                return Page();

            var apiUrl = _configuration.GetValue<string>(apiUrlKey);
            var cipherKey = _configuration.GetValue<string>(cipherSecretKey);

            await _uploadService.ProcessFile(new ProcessFileRequest
            {
                ApiUrl = apiUrl,
                CipherKey = cipherKey,
                FilePath = path,
                Password = FileUpload.Password,
                Username = FileUpload.Username
            });

            return RedirectToPage(redirectPage);
        }
    }
}
