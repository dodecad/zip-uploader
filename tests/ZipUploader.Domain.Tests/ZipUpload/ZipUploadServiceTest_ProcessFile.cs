using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using ZipUploader.Contracts.Models;

namespace ZipUploader.Domain.Tests.ZipUpload
{
    /// <summary>
    /// Tests related to the ProcessFile operation of the ZipUploadService.
    /// </summary>
    [TestClass]
    public class ZipUploadServiceTest_ProcessFile : ZipUploadServiceTest
    {
        #region Fields

        /// <summary>
        /// The expected number of calls to the SendFile method.
        /// </summary>
        private int _callsToSendFile;

        /// <summary>
        /// The HTTP content.
        /// </summary>
        private Task<string> _httpContent;

        /// <summary>
        /// The request argument.
        /// </summary>
        private ProcessFileRequest _requestArgument;

        #endregion Fields

        /// <summary>
        /// Tests the operation under the specified circumstances.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ProcessFile_WhenRequestArgumentIsNull_ExpectArgumentNullException()
        {
            // Arrange:
            _requestArgument = null;
            Stub();

            try
            {
                // Act:
                await Act();
            }
            catch (ArgumentNullException exception)
            {
                // Assert:
                Assert.AreEqual("request", exception.ParamName);
                AssertCore();
                throw;
            }
        }

        /// <summary>
        /// Performs useful initialisation before each and every test is executed.
        /// </summary>
        [TestInitialize]
        public override void TestInitialise()
        {
            base.TestInitialise();

            _callsToSendFile = 0;

            _requestArgument = new ProcessFileRequest
            {
                ApiUrl = "https://test/api/upload",
                CipherKey = "test_key",
                FilePath = @"C:\Test\Directory",
                Password = "password",
                Username = "username"
            };
        }

        /// <summary>
        /// Provides common Asserts for all tests.
        /// </summary>
        protected override void AssertCore()
        {
            base.AssertCore();

            TransferService.Verify(service => service.SendFile(It.IsAny<SendFileRequest>()),
                Times.Exactly(_callsToSendFile));
        }
        
        /// <summary>
        /// Stubs the methods of the mocked dependencies.
        /// </summary>
        protected override void Stub()
        {
            base.Stub();

            TransferService
                .Setup(service => service.SendFile(It.IsAny<SendFileRequest>()))
                .Returns(_httpContent);
        }

        /// <summary>
        /// Invokes the operation under test.
        /// </summary>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        private Task<string> Act()
        {
            return ZipUploadService.ProcessFile(_requestArgument);
        }
    }
}
