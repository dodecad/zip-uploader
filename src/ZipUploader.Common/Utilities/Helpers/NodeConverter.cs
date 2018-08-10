using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using ZipUploader.Common.Models;

namespace ZipUploader.Common.Helpers
{
    /// <summary>
    /// Node-specific JSON converter.
    /// </summary>
    public class NodeConverter : JsonConverter
    {
        /// <summary>
        /// The cipher key.
        /// </summary>
        private readonly string _cipherKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConverter"/> class.
        /// </summary>
        public NodeConverter()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConverter"/> class.
        /// </summary>
        /// <param name="cipherKey">The cipher key.</param>
        public NodeConverter(string cipherKey)
        {
            _cipherKey = cipherKey;
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

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Node);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            const string contents = "Contents";
            const string isFile = "IsFile";
            const string name = "Name";

            var node = (Node)value;
            var nodeName = node.Name;

            if (SecureMode && node.SecureMode)
                nodeName = node.EncryptedName;

            var obj = new JObject();

            if (string.IsNullOrWhiteSpace(nodeName) == false)
                obj.Add(name, nodeName);

            if (node.IsFile)
                obj.Add(isFile, true);
            else
                obj.Add(contents, JArray.FromObject(node.Contents, serializer));

            obj.WriteTo(writer);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var node = new Node(_cipherKey);

            // Populate the object properties:
            serializer.Populate(jObject.CreateReader(), node);

            return node;
        }
    }
}
