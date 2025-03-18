using System.Drawing;

namespace TagsCloudVisualization.Interfaces;

public interface ICircularCloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    
    Point GetNextSpiralPoint(Size rectangleSize);

    void InitialiseParametersWithFirstRectangle(Rectangle firstRectangle);

    Rectangle GetRectangleFromCenterPointAndSize(Point centerPoint, Size rectangleSize);
}