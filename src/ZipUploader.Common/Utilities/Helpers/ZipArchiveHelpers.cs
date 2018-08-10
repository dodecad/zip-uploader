using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ZipUploader.Common.Extensions;

namespace ZipUploader.Common.Helpers
{
    /// <summary>
    /// ZipArchive-specific helper methods.
    /// </summary>
    public static class ZipArchiveHelpers
    {
        /// <summary>
        /// Lists the archived items.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The result as a dictionary.</returns>
        public static IDictionary<string, string> ListArchiveItems(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(nameof(path));

            var items = new Dictionary<string, string>();

            using (var archive = ZipFile.OpenRead(path))
            {
                foreach (var entry in archive.Entries)
                {
                    if (entry.IsFolder())
                        continue;

                    items.Add(Path.GetFileName(entry.FullName), Path.GetDirectoryName(entry.FullName));
                }
            }

            return items;
        }
    }
}
