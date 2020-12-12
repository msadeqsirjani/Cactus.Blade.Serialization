using Cactus.Blade.Guard;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Cactus.Blade.Serialization
{
    /// <summary>
    /// A JSON implementation of the <see cref="ISerializer"/> interface using <see cref="System.Text.Json.JsonSerializer"/>.
    /// </summary>
    public class DefaultJsonSerializer : ISerializer
    {
        private static readonly UTF8Encoding StreamEncoding = new UTF8Encoding(false, true);
        private const int StreamBufferSize = 1024;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultJsonSerializer"/> class.
        /// </summary>
        /// <param name="name">The name of the serializer, used to when selecting which serializer to use.</param>
        /// <param name="settings">Newtonsoft settings for the serializer.</param>
        public DefaultJsonSerializer(string name = "default", JsonSerializerSettings settings = null)
        {
            Name = name ?? "default";

            JsonSerializer =
                settings.IsNull()
                    ? JsonSerializer.CreateDefault()
                    : JsonSerializer.CreateDefault(settings);
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="JsonSerializer"/> used when serializing.
        /// </summary>
        public JsonSerializer JsonSerializer { get; }

        /// <inheritdoc />
        public void SerializeToStream(Stream stream, object item, Type type)
        {
            Guard.Guard.Against.Null(stream, nameof(stream));
            Guard.Guard.Against.Null(item, nameof(item));
            Guard.Guard.Against.Null(type, nameof(type));

            using var writer = new StreamWriter(stream, StreamEncoding, StreamBufferSize, true);
            using var jsonWriter = new JsonTextWriter(writer);

            JsonSerializer.Serialize(jsonWriter, item);
        }

        /// <inheritdoc />
        public object DeserializeFromStream(Stream stream, Type type)
        {
            Guard.Guard.Against.Null(stream, nameof(stream));
            Guard.Guard.Against.Null(type, nameof(type));

            using var reader = new StreamReader(stream, StreamEncoding, true, StreamBufferSize, true);
            using var jsonReader = new JsonTextReader(reader);

            return JsonSerializer.Deserialize(jsonReader, type);
        }

        /// <inheritdoc />
        public string SerializeToString(object item, Type type)
        {
            Guard.Guard.Against.Null(item, nameof(item));
            Guard.Guard.Against.Null(type, nameof(type));

            var builder = new StringBuilder();

            using var stringWriter = new StringWriter(builder);
            using var jsonWriter = new JsonTextWriter(stringWriter);
            JsonSerializer.Serialize(jsonWriter, item, type);

            return builder.ToString();
        }

        /// <inheritdoc />
        public object DeserializeFromString(string data, Type type)
        {
            Guard.Guard.Against.Null(data, nameof(data));
            Guard.Guard.Against.Null(type, nameof(type));

            using var stringReader = new StringReader(data);
            using var jsonReader = new JsonTextReader(stringReader);
            return JsonSerializer.Deserialize(jsonReader, type);
        }
    }
}
