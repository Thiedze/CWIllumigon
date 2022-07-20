using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CWIllumigon.Domain
{
    public class Triangle
    {
        private int _brightness;

        [JsonPropertyName("bri")]
        public int Brightness
        {
            get => _brightness;
            set => SetBrightness(value);
        }

        [JsonPropertyName("seg")] public List<Segment> Segments { get; } = new List<Segment>();

        private void SetBrightness(int value)
        {
            _brightness = 255 / 100 * value;
        }
    }
}