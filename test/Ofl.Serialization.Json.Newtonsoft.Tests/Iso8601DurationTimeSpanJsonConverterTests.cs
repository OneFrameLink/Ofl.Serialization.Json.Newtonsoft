using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Ofl.Serialization.Json.Newtonsoft.Tests
{
    public class Iso8601DurationTimeSpanJsonConverterTests
    {
        private class NonNullableTest
        {
            [JsonConverter(typeof(Iso8601DurationTimeSpanJsonConverter))]
            public TimeSpan Value { get; set; }
        }

        [Theory]
        [InlineData("{\"value\":\"PT1H\" }", "01:00:00")]
        [InlineData("{\"value\":\"PT1M\" }", "00:01:00")]
        [InlineData("{\"value\":\"PT1S\" }", "00:00:01")]
        [InlineData("{\"value\":\"PT1H1M\" }", "01:01:00")]
        [InlineData("{\"value\":\"PT1H1S\" }", "01:00:01")]
        [InlineData("{\"value\":\"PT1M1S\" }", "00:01:01")]
        [InlineData("{\"value\":\"PT1H1M1S\" }", "01:01:01")]
        public void Test_ReadNonNullableJson(string json, string durationString)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            if (string.IsNullOrWhiteSpace(durationString)) throw new ArgumentNullException(nameof(durationString));

            // Get the expected value.
            TimeSpan expected = TimeSpan.Parse(durationString);

            // Parse/etc.
            JObject obj = JObject.Parse(json);

            // Map to an object.
            NonNullableTest test = obj.ToObject<NonNullableTest>();

            // Equal.
            Assert.Equal(expected, test.Value);
        }

        private class NullableTest
        {
            [JsonConverter(typeof(Iso8601DurationTimeSpanJsonConverter))]
            public TimeSpan? Value { get; set; }
        }

        [Theory]
        [InlineData("{\"value\": null }", null)]
        [InlineData("{\"value\":\"PT1H\" }", "01:00:00")]
        [InlineData("{\"value\":\"PT1M\" }", "00:01:00")]
        [InlineData("{\"value\":\"PT1S\" }", "00:00:01")]
        [InlineData("{\"value\":\"PT1H1M\" }", "01:01:00")]
        [InlineData("{\"value\":\"PT1H1S\" }", "01:00:01")]
        [InlineData("{\"value\":\"PT1M1S\" }", "00:01:01")]
        [InlineData("{\"value\":\"PT1H1M1S\" }", "01:01:01")]
        public void Test_ReadNullableJson(string json, string durationString)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));

            // Get the expected value.
            TimeSpan? expected = string.IsNullOrWhiteSpace(durationString) ? 
                (TimeSpan?) null : TimeSpan.Parse(durationString);

            // Parse/etc.
            JObject obj = JObject.Parse(json);

            // Map to an object.
            NullableTest test = obj.ToObject<NullableTest>();

            // Equal.
            Assert.Equal(expected, test.Value);
        }
    }
}
