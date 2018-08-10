using System;
using System.Linq;
using System.Threading.Tasks;
using ZipUploader.Common.Core;
using ZipUploader.Common.Helpers;
using ZipUploader.Contracts.Models;
using ZipUploader.Contracts.Services;
using ZipUploader.Data.Abstractions;
using ZipUploader.Data.Entities;

namespace ZipUploader.Domain.Services
{
    /// <summary>
    /// Provides the business-level operations for ZipUploadService.
    /// </summary>
    public class ZipUploadService : IUploadService
    {
        private readonly IDbContext _context;
        private readonly ITransferService _transferService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipUploadService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="transferService">The transfer service.</param>
        public ZipUploadService(
            IDbContext context,
            ITransferService transferService)
        {
            _context = context;
            _transferService = transferService;
        }

        /// <summary>
        /// Processes the file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The HTTP content.</returns>
        async Task<string> IUploadService.ProcessFile(ProcessFileRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.FilePath))
                throw new ArgumentException(nameof(request.FilePath));

            var items = ZipArchiveHelpers.ListArchiveItems(request.FilePath);
            var json = NodeHelpers.ToJson(items, request.CipherKey);

            return await _transferService.SendFile(new SendFileRequest
            {
                ContentType = ApplicationConstants.JsonContentType,
                Password = request.Password,
                StringContent = json,
                Url = request.ApiUrl,
                UserName = request.Username
            });
        }

        /// <summary>
        /// Stores the JSON data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A response of the operation.</returns>
        async Task<int> IUploadService.StoreJsonData(StoreJsonDataRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var nodes = NodeHelpers.ParseJson(request.Json, request.CipherKey);

            var files = nodes
                .Where(node => node.IsFile)
                .Select(node => new ArchiveContent
                {
                    IsFile = node.IsFile,
                    Name = node.DecryptedName
                });

            await _context.ArchiveItems.AddRangeAsync(files);
            return await _context.SaveChangesAsync();
        }
    }
}
