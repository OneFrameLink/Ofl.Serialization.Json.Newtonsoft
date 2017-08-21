using System;
using System.Xml;
using Newtonsoft.Json;

namespace Ofl.Serialization.Json.Newtonsoft
{
    public class Iso8601DurationTimeSpanJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);

        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Validate parameters.
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (objectType != typeof(TimeSpan) && objectType != typeof(TimeSpan?))
                throw new ArgumentException($"The { nameof(objectType) } parameter must be a { nameof(TimeSpan) } or a { typeof(TimeSpan?).Name }.", nameof(objectType));

            // Get the value.
            var spanString = reader.Value as string;

            // If the value is null, return null.
            if (spanString == null) return null;

            // Convert.
            return XmlConvert.ToTimeSpan(spanString);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Validate parameters.
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            // Get the duration.
            var duration = (TimeSpan?) value;

            // If null, write null.
            if (duration == null)
                // Write null.
                writer.WriteNull();
            else
                // Write the value.
                writer.WriteValue(XmlConvert.ToString(duration.Value));
        }
    }
}