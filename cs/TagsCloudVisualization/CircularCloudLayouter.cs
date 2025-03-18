using System.Drawing;
using TagsCloudVisualization.Helpers;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center) : ICircularCloudLayouter
{
    private SpiralParameters spiralParameters = null!;

    public readonly List<Rectangle> Rectangles = [];

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (Rectangles.Count == 0)
        {
            var centerRectangle = GetRectangleFromCenterPointAndSize(center, rectangleSize);
            InitialiseParametersWithFirstRectangle(centerRectangle);
            Rectangles.Add(centerRectangle);
            return centerRectangle;
        }

        var nextPoint = GetNextSpiralPoint(rectangleSize);
        var newRectangle = GetRectangleFromCenterPointAndSize(nextPoint, rectangleSize);
        var attempts = 0;
        while (Rectangles.Any(r => RectanglesChecker.AreRectanglesIntersect(r, newRectangle)))
        {
            attempts++;
            if (attempts > 10)
            {
                spiralParameters.SpiralStep *= (int)1.1;
                spiralParameters.LastAngle *= 0.9;
            }
            nextPoint = GetNextSpiralPoint(rectangleSize);
            newRectangle = GetRectangleFromCenterPointAndSize(nextPoint, rectangleSize);
        }
        Rectangles.Add(newRectangle);
        return newRectangle;
    }

    public Point GetNextSpiralPoint(Size rectangleSize)
    {
        spiralParameters.SpiralStep = Math.Max(rectangleSize.Width, rectangleSize.Height) / 4 + SpiralParameters.Padding;

        var radius = spiralParameters.StartRadius + spiralParameters.SpiralStep * spiralParameters.LastAngle;
        var x = radius * Math.Cos(spiralParameters.LastAngle);
        var y = radius * Math.Sin(spiralParameters.LastAngle);
        spiralParameters.LastAngle += Math.PI / (6 + radius / 100);

        return new Point((int)x + center.X, (int)y + center.Y);
    }

    public void InitialiseParametersWithFirstRectangle(Rectangle firstRectangle)
    {
        spiralParameters = new SpiralParameters
        {
            StartRadius = Math.Max(firstRectangle.Width, firstRectangle.Height) / 4 + SpiralParameters.Padding,
            SpiralStep = Math.Max(firstRectangle.Width, firstRectangle.Height) / 6 + SpiralParameters.Padding,
            LastAngle = Math.PI / 6
        };
    }

    public Rectangle GetRectangleFromCenterPointAndSize(Point centerPoint, Size rectangleSize)
    {
        return new Rectangle(
            new Point(centerPoint.X - rectangleSize.Width / 2, centerPoint.Y + rectangleSize.Height / 2),
            rectangleSize);
    }
}