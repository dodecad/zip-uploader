using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ZipUploader.Common.Models;

namespace ZipUploader.Common.Helpers
{
    /// <summary>
    /// Node-specific helper methods.
    /// </summary>
    public static class NodeHelpers
    {
        private static readonly char[] _pathDelimiters = { '/', '\\' };

        public static string ToJson(IDictionary<string, string> items)
        {
            return ToJson(items, string.Empty);
        }

        public static string ToJson(IDictionary<string, string> items, string cipherKey)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var root = new Node();

            foreach (var item in items)
            {
                var parent = root;

                if (string.IsNullOrEmpty(item.Value) == false)
                {
                    var child = null as Node;
                    var splitted = item.Value.Split(_pathDelimiters, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var part in splitted)
                    {
                        var name = part.Trim();

                        child = parent.Contents
                            .ToList()
                            .Find(n => n.Name == name);

                        if (child == null)
                        {
                            child = new Node(cipherKey) { Name = name };
                            parent.Contents.Add(child);
                        }

                        parent = child;
                    }
                }

                parent.Contents.Add(new Node(cipherKey) { Name = item.Key });
            }

            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new NodeConverter(cipherKey) },
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(root, settings);
        }

        public static IEnumerable<Node> ParseJson(string json, string cipherKey)
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new NodeConverter(cipherKey) },
                Formatting = Formatting.Indented
            };

            var root = JsonConvert.DeserializeObject<Node>(json, settings);
            var nodes = GetAllNodes(root);
            return nodes;

            IList<Node> GetAllNodes(Node node)
            {
                var result = new List<Node> { node };

                foreach (var child in node.Contents)
                    result.AddRange(GetAllNodes(child));

                return result;
            }
        }
    }
}
