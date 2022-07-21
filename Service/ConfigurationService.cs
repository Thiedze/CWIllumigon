using CWIllumigon.Domain;
using OpenCvSharp;

namespace CWIllumigon.Service;

public class ConfigurationService
{
    private int IndexColor { get; set; } = 0;

    private static int RedColorRange => 20;
    private int Red { get; set; } = 320;

    private static int GreenColorRange => 20;
    private int Green { get; set; } = 245;

    private static int BlueColorRange => 20;
    private int Blue { get; set; } = 170;

    private List<int> _lowH;

    private List<int> _highH;

    private static string WindowName => "Image";

    private WledService WledService { get; }

    public ConfigurationService(string ip)
    {
        WledService = new WledService(ip);

        _lowH = new List<int>();
        _highH = new List<int>();
    }

    public void Start(int triangleCount)
    {
        var image = new Mat("Resources/test.jpg");

        var scale = Math.Min(Math.Min((double) 600 / image.Cols, (double) 600 / image.Rows), 1.0);
        Cv2.Resize(image, image, new Size(), scale, scale);

        RenderImage(image, IndexColor);
        CreateTrackbars(image);

        for (var index = 0; index < triangleCount; index++)
        {
            var triangle = CreateTriangle(index);
            WledService.Show(triangle);
        }

        Cv2.WaitKey();
    }

    private void RenderImage(Mat image, int index)
    {
        RefreshLowH(Red, Green, Blue);
        RefreshHighH(Red, Green, Blue);

        var result = image.Clone();
        Cv2.CvtColor(image, result, ColorConversionCodes.BGR2HSV);

        var lowerHsv = new Scalar(_lowH[index], 100, 100);
        var higherHsv = new Scalar(_highH[index], 255, 255);

        Cv2.InRange(result, lowerHsv, higherHsv, result);
        // Cv2.ImShow("Hsv", result);

        Cv2.GaussianBlur(result, result, new Size(11, 11), 0);
        // Cv2.ImShow("Blur", result);

        Cv2.Threshold(result, result, 200, 255, ThresholdTypes.Binary);
        // Cv2.ImShow("Threshold", result);

        Cv2.Erode(result, result, null, null, 2);
        Cv2.Dilate(result, result, null, null, 4);
        // Cv2.ImShow("Erode / Dilate", result);

        var imageWithCycle = DrawCycle(image, result);
        Cv2.ImShow(WindowName, imageWithCycle);
    }

    private static Mat DrawCycle(Mat image, Mat result)
    {
        Cv2.FindContours(result.Clone(), out var contours, new Mat(), RetrievalModes.External,
            ContourApproximationModes.ApproxSimple);

        var imageWithCycle = image.Clone();
        foreach (var contour in contours)
        {
            Cv2.MinEnclosingCircle(contour, out var point, out var radius);
            Cv2.Circle(imageWithCycle, (int) point.X, (int) point.Y, (int) radius, new Scalar(0, 0, 255), 3);
        }

        return imageWithCycle;
    }

    private static Triangle CreateTriangle(int position)
    {
        var triangle = new Triangle
        {
            Brightness = 50
        };
        var start = position * 10;

        triangle.Segments.Add(CreateSegment(start, start + 3, 0, 0, 255));
        triangle.Segments.Add(CreateSegment(start + 3, start + 6, 0, 255, 0));
        triangle.Segments.Add(CreateSegment(start + 6, start + 9, 255, 0, 0));
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

    private void CreateTrackbars(Mat image)
    {
        var valueRed = Red;
        Cv2.CreateTrackbar("Red", WindowName, ref valueRed, 360, (pos, _) =>
        {
            Red = pos;
            RenderImage(image, IndexColor);
        });

        var valueGreen = Green;
        Cv2.CreateTrackbar("Green", WindowName, ref valueGreen, 360, (pos, _) =>
        {
            Green = pos;
            RenderImage(image, IndexColor);
        });

        var valueBlue = Blue;
        Cv2.CreateTrackbar("Blue", WindowName, ref valueBlue, 360, (pos, _) =>
        {
            Blue = pos;
            RenderImage(image, IndexColor);
        });

        var valueIndex = IndexColor;
        Cv2.CreateTrackbar("Index", WindowName, ref valueIndex, 2, (pos, _) =>
        {
            IndexColor = pos;
            RenderImage(image, IndexColor);
        });
    }

    private void RefreshLowH(int hValueRed, int hValueGreen, int hValueBlue)
    {
        _lowH = new List<int>
            {(hValueRed - RedColorRange) / 2, (hValueGreen - GreenColorRange) / 2, (hValueBlue - BlueColorRange) / 2};
    }

    private void RefreshHighH(int hValueRed, int hValueGreen, int hValueBlue)
    {
        _highH = new List<int>
            {(hValueRed + RedColorRange) / 2, (hValueGreen + GreenColorRange) / 2, (hValueBlue + BlueColorRange) / 2};
    }
}