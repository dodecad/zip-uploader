using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZipUploader.Contracts.Models;
using ZipUploader.Data.Entities;
using ZipUploader.Tests;

namespace ZipUploader.Domain.Tests.ZipUpload
{
    /// <summary>
    /// Tests related to the StoreJsonData operation of the ZipUploadService.
    /// </summary>
    [TestClass]
    public class ZipUploadServiceTest_StoreJsonData : ZipUploadServiceTest
    {
        #region Fields

        /// <summary>
        /// The expected number of calls to the SaveChangesAsync method.
        /// </summary>
        private int _callsToSaveChangesAsync;

        /// <summary>
        /// The request argument.
        /// </summary>
        private StoreJsonDataRequest _requestArgument;

        #endregion Fields

        /// <summary>
        /// Tests the operation under the specified circumstances.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task StoreJsonData_WhenRequestArgumentIsNull_ExpectArgumentNullException()
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
        /// Tests the operation under the specified circumstances.
        /// </summary>
        [TestMethod]
        public async Task StoreJsonData_UnderValidCircumstances_ExpectSuccess()
        {
            // Arrange:
            _callsToSaveChangesAsync = 1;
            Stub();

            // Act:
            await Act();

            // Assert:
            AssertCore();
        }

        /// <summary>
        /// Performs useful initialisation before each and every test is executed.
        /// </summary>
        [TestInitialize]
        public override void TestInitialise()
        {
            base.TestInitialise();

            _callsToSaveChangesAsync = 0;

            _requestArgument = new StoreJsonDataRequest
            {
                CipherKey = string.Empty,
                Json = "{ Contents: [ { Name: \"test\", Contents: [] } ] }"
            };
        }

        /// <summary>
        /// Provides common Asserts for all tests.
        /// </summary>
        protected override void AssertCore()
        {
            base.AssertCore();

            DbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Exactly(_callsToSaveChangesAsync));
        }

        /// <summary>
        /// Stubs the methods of the mocked dependencies.
        /// </summary>
        protected override void Stub()
        {
            base.Stub();

            var archiveContents = new List<ArchiveContent>
            {
                new ArchiveContent { ID = 1, IsFile = true, Name = "Test1" },
                new ArchiveContent { ID = 2, IsFile = true, Name = "Test2" },
                new ArchiveContent { ID = 3, IsFile = true, Name = "Test3" }
            };

            DbContext
                .Setup(context => context.ArchiveItems)
                .Returns(DbSetHelpers.GetQueryableMockDbSet(archiveContents));
        }

        /// <summary>
        /// Invokes the operation under test.
        /// </summary>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        private Task<int> Act()
        {
            return ZipUploadService.StoreJsonData(_requestArgument);
        }
    }
}
