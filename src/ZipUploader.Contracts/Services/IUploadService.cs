using System.Threading.Tasks;
using ZipUploader.Contracts.Models;

namespace ZipUploader.Contracts.Services
{
    /// <summary>
    /// Defines the business-level operations for the uploading services.
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// Processes the file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The HTTP content.</returns>
        Task<string> ProcessFile(ProcessFileRequest request);

        /// <summary>
        /// Stores the JSON data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A response of the operation.</returns>
        Task<int> StoreJsonData(StoreJsonDataRequest request);
    }
}
