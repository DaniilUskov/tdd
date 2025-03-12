using System.Drawing;

namespace TagsCloudVisualization.Helpers;

public static class RectanglesChecker
{
    public static bool AreRectanglesIntersect(Rectangle firstRectangle, Rectangle secondRectangle)
    {
        var firstRectangleAnglePoints = new List<Point>
        {
            new(firstRectangle.Left, firstRectangle.Top),
            new(firstRectangle.Right, firstRectangle.Top),
            new(firstRectangle.Left, firstRectangle.Bottom),
            new(firstRectangle.Right, firstRectangle.Bottom)
        };
        var secondRectangleAnglePoints = new List<Point>
        {
            new(secondRectangle.Left, secondRectangle.Top),
            new(secondRectangle.Right, secondRectangle.Top),
            new(secondRectangle.Left, secondRectangle.Bottom),
            new(secondRectangle.Right, secondRectangle.Bottom)
        };
        var areIntersectWithAngles = firstRectangleAnglePoints.Any(secondRectangleAnglePoints.Contains);
        
        return areIntersectWithAngles 
               || Math.Max(firstRectangle.Left, secondRectangle.Left) < Math.Min(firstRectangle.Right, secondRectangle.Right)
               && Math.Max(firstRectangle.Top, secondRectangle.Top) < Math.Min(firstRectangle.Bottom, secondRectangle.Bottom);
    }
    
    public static bool IsRectangleIsInsideAnother(Rectangle firstRectangle, Rectangle secondRectangle)
    {
        return secondRectangle.Left <= firstRectangle.Left && secondRectangle.Right >= firstRectangle.Right
                && secondRectangle.Bottom <= firstRectangle.Bottom && secondRectangle.Top <= firstRectangle.Top;
    }
}