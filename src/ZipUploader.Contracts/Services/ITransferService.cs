using System.Threading.Tasks;
using ZipUploader.Contracts.Models;

namespace ZipUploader.Contracts.Services
{
    /// <summary>
    /// Defines the business-level operations for TransferService.
    /// </summary>
    public interface ITransferService
    {
        /// <summary>
        /// Sends the file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The HTTP content.</returns>
        Task<string> SendFile(SendFileRequest request);
    }
}
