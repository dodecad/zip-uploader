using System.Collections.Generic;
using System.Linq;
using ZipUploader.Common.Helpers;

namespace ZipUploader.Common.Models
{
    /// <summary>
    /// Represents a Node.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// The cipher key.
        /// </summary>
        private readonly string _cipherKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="cipherKey">The cipher key.</param>
        public Node(string cipherKey)
        {
            _cipherKey = cipherKey;
        }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public IList<Node> Contents { get; set; } = new List<Node>();

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the decrypted name.
        /// </summary>
        /// <value>
        /// The decrypted name.
        /// </value>
        public string DecryptedName
        {
            get
            {
                if (SecureMode == false || string.IsNullOrEmpty(Name))
                    return string.Empty;

                return StringCipher.Decrypt(Name, _cipherKey);
            }
        }

        /// <summary>
        /// Gets the encrypted name.
        /// </summary>
        /// <value>
        /// The encrypted name.
        /// </value>
        public string EncryptedName
        {
            get
            {
                if (SecureMode == false || string.IsNullOrEmpty(Name))
                    return string.Empty;

                return StringCipher.Encrypt(Name, _cipherKey);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a file.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a file; otherwise, <c>false</c>.
        /// </value>
        public bool IsFile
        {
            get
            {
                if (Contents == null)
                    return true;
                
                return Contents.Any() == false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether secure mode is used.
        /// </summary>
        /// <value>
        /// <c>true</c> if secure mode used; otherwise, <c>false</c>.
        /// </value>
        public bool SecureMode
        {
            get
            {
                return string.IsNullOrEmpty(_cipherKey) == false;
            }
        }
    }
}
