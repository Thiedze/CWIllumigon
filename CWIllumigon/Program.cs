using System.Collections.Generic;
using CWIllumigon.Domain;
using OpenCvSharp;

namespace CWIllumigon
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var triangle = CreateTriangle();

            var wledService = new WledService("10.10.12.145");
            wledService.StartDetection(triangle);

            using var image = new Mat("Resources/test.jpg");
            using var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);
            gray.SaveImage("test-gray.jpg");
        }

        private static Triangle CreateTriangle()
        {
            var triangle = new Triangle
            {
                Brightness = 50
            };
            triangle.Segments.Add(CreateSegment(0, 3, 0, 0, 255));
            triangle.Segments.Add(CreateSegment(3, 6, 0, 255, 0));
            triangle.Segments.Add(CreateSegment(6, 9, 255, 0, 0));
            return triangle;
        }

        private static Segment CreateSegment(int start, int stop, int r, int g, int b)
        {
            var segment = new Segment
            {
                Start = start,
                Stop = stop,
                Length = stop - start
            };
            segment.Color.Add(new List<int>
            {
                r, g, b
            });

            return segment;
        }
    }
}