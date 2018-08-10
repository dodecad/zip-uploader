namespace ZipUploader.Contracts.Models
{
    /// <summary>
    /// The request for the ProcessFile operation.
    /// </summary>
    public class ProcessFileRequest
    {
        /// <summary>
        /// Gets or sets the URL for accessing an API endpoint. 
        /// </summary>
        /// <value>
        /// The URL address.
        /// </value>
        public string ApiUrl { get; set; }
        
        /// <summary>
        /// Gets or sets the cipher key.
        /// </summary>
        /// <value>
        /// The cipher key.
        /// </value>
        public string CipherKey { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }
    }
}
