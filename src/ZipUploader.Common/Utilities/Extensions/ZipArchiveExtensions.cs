using System.IO.Compression;

namespace ZipUploader.Common.Extensions
{
    /// <summary>
    /// Class to add useful class extension methods for ZipArchive.
    /// </summary>
    public static class ZipArchiveExtensions
    {
        /// <summary>
        /// Determines whether the entry is a folder.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>
        /// <c>true</c> if the specified entry is folder; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFolder(this ZipArchiveEntry entry)
        {
            return entry.FullName.EndsWith("/");
        }
    }
}
