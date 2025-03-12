namespace TagsCloudVisualization.Models;

public class SpiralParameters
{
    public const int Padding = 5;

    public int SpiralStep { get; set; }

    public int StartRadius { get; init; }

    public double LastAngle { get; set; }
}