using System.ComponentModel.DataAnnotations;

namespace ZipUploader.Data.Entities
{
    /// <summary>
    /// Represents an ArchiveContent entity.
    /// </summary>
    public class ArchiveContent
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a file.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a file; otherwise, <c>false</c>.
        /// </value>
        public bool IsFile { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}
