namespace ZipUploader.Contracts.Models
{
    /// <summary>
    /// The request for the StoreJsonData operation.
    /// </summary>
    public class StoreJsonDataRequest
    {
        /// <summary>
        /// Gets or sets the cipher key.
        /// </summary>
        /// <value>
        /// The cipher key.
        /// </value>
        public string CipherKey { get; set; }

        /// <summary>
        /// Gets or sets the json.
        /// </summary>
        /// <value>
        /// The json.
        /// </value>
        public string Json { get; set; }
    }
}
