using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ZipUploader.Common.Core;
using ZipUploader.Contracts.Models;
using ZipUploader.Contracts.Services;

namespace ZipUploader.Domain.Services
{
    /// <summary>
    /// Provides the business-level operations for TransferService.
    /// </summary>
    public class TransferService : ITransferService
    {
        /// <summary>
        /// Sends the file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The HTTP content.</returns>
        async Task<string> ITransferService.SendFile(SendFileRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Url))
                throw new ArgumentException(nameof(request.Url));

            var httpContent = new StringContent(request.StringContent, Encoding.UTF8, request.ContentType);

            using (var httpClient = new HttpClient())
            {
                var credentials = Encoding.ASCII.GetBytes($"{request.UserName}:{request.Password}");
                var authParameter = Convert.ToBase64String(credentials);
                var authHeader = new AuthenticationHeaderValue(ApplicationConstants.BasicAuthScheme, authParameter);

                // Set the Authorization header:
                httpClient.DefaultRequestHeaders.Authorization = authHeader;

                // Send a POST request:
                var httpResponse = await httpClient.PostAsync(request.Url, httpContent);

                // Get the content of the response:
                if (httpResponse.Content != null)
                    return await httpResponse.Content.ReadAsStringAsync();
            }

            return string.Empty;
        }
    }
}
