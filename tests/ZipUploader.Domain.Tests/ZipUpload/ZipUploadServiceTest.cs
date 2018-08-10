using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ZipUploader.Contracts.Services;
using ZipUploader.Data.Abstractions;
using ZipUploader.Domain.Services;
using ZipUploader.Tests;

namespace ZipUploader.Domain.Tests.ZipUpload
{
    /// <summary>
    /// Base class for ZipUploadService tests.
    /// </summary>
    [TestClass]
    public abstract class ZipUploadServiceTest : UnitTestContainer
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipUploadServiceTest"/> class.
        /// </summary>
        public ZipUploadServiceTest()
        {
            DbContext = new Mock<IDbContext>();
            TransferService = new Mock<ITransferService>();

            ZipUploadService = new ZipUploadService(
                DbContext.Object,
                TransferService.Object);
        }

        #endregion Public Constructors

        #region Protected Properties

        /// <summary>
        /// Gets or sets the db context.
        /// </summary>
        /// <value>The db context.</value>
        protected Mock<IDbContext> DbContext { get; set; }

        /// <summary>
        /// Gets or sets the transfer service.
        /// </summary>
        /// <value>The transfer service.</value>
        protected Mock<ITransferService> TransferService { get; set; }

        /// <summary>
        /// Gets or sets the upload service.
        /// </summary>
        /// <value>The upload service.</value>
        protected IUploadService ZipUploadService { get; set; }

        #endregion Protected Properties
    }
}
