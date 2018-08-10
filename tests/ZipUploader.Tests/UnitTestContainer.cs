using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZipUploader.Tests
{
    /// <summary>
    /// Base class for all unit tests.
    /// </summary>
    [TestClass]
    public abstract class UnitTestContainer
    {
        #region Public Methods

        /// <summary>
        /// Performs useful initialisation before each and every test is executed.
        /// </summary>
        public virtual void TestInitialise()
        {
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Provides common asserts for all tests.
        /// </summary>
        protected virtual void AssertCore()
        {
        }

        /// <summary>
        /// Stubs the methods of the mocked dependencies.
        /// </summary>
        protected virtual void Stub()
        {
        }

        #endregion Protected Methods
    }
}
