using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ofl.Serialization.Json.Newtonsoft
{
    public static class JsonSerializerExtensions
    {
        public static string SerializeToString<TRequest>(this JsonSerializer serializer, TRequest request)
        {
            // Validate parameters.
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Create a StringWriter.
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            // Now a json writer.
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                // Serialize.
                serializer.Serialize(jsonWriter, request);

                // Flush the writer.
                jsonWriter.Flush();

                // Write the json.
                return stringWriter.ToString();
            }
        }

        public static TResponse DeserializeFromString<TResponse>(this JsonSerializer serializer, string json)
        {
            // Validate parameters.
            if (serializer == null) throw new ArgumentNullException(nameof(json));
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));

            // Create a reader.
            using (var stringReader = new StringReader(json))
            // Json reader.
            using (var jsonReader = new JsonTextReader(stringReader))
                // Deserialize.
                return serializer.Deserialize<TResponse>(jsonReader);
        }

        public static async Task<MemoryStream> SerializeToMemoryStreamAsync<TRequest>(this JsonSerializer serializer, TRequest request,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Create the stream.
            using (var stream = new MemoryStream())
            using (TextWriter textWriter = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                // Serialize.
                serializer.Serialize(jsonWriter, request);

                // Flush.
                await jsonWriter.FlushAsync(cancellationToken).ConfigureAwait(false);
                await textWriter.FlushAsync().ConfigureAwait(false);

                // Reset the origin.
                stream.Position = 0;

                // Try and get the buffer.
                if (!stream.TryGetBuffer(out ArraySegment<byte> buffer))
                    throw new InvalidOperationException($"The call to { nameof(stream.TryGetBuffer) } returned false.");

                // Create a new memory stream from the buffer.
                return new MemoryStream(buffer.Array, buffer.Offset, buffer.Count);
            }
        }
    }
}
