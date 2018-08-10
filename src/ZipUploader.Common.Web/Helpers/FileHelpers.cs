using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ZipUploader.Common.Web.Helpers
{
    /// <summary>
    /// Web-based file-oriented helper methods.
    /// </summary>
    public static class FileHelpers
    {
        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="modelState">The validation information.</param>
        /// <returns>The file path.</returns>
        public static async Task<string> SaveFile(IFormFile file, ModelStateDictionary modelState)
        {
            const string emptyFileErrorMessage = "The file is empty.";
            const string destFolder = "wwwroot";

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            if (file.Length == 0)
            {
                modelState?.AddModelError(file.Name, emptyFileErrorMessage);
            }
            else
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), destFolder, file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                    await file.CopyToAsync(stream);

                return path;
            }

            return string.Empty;
        }
    }
}
