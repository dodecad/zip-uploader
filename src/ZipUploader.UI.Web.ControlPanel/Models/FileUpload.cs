using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ZipUploader.UI.Web.ControlPanel.Models
{
    /// <summary>
    /// The FileUpload ViewModel.
    /// </summary>
    public class FileUpload
    {
        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        [Required]
        [Display(Name = "File")]
        public IFormFile File { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
    }
}
