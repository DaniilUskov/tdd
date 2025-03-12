using FluentAssertions;
using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization.Helpers;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter? testLayouter;

    [TearDown]
    public void TearDown()
    {
        var context = TestContext.CurrentContext;
        if (context.Result.Outcome != NUnit.Framework.Interfaces.ResultState.Failure) 
            return;
        
        var imageDrawer = new ImageDrawer();
        var rectanglesFromTest = testLayouter!.Rectangles
            .OrderByDescending(x => x.Width * x.Height)
            .ToList();

        var filePath = $"Tests/TestFail-{context.Test.Name}";

        imageDrawer.DrawTagsCloudFromEmptyRectangles(rectanglesFromTest, filePath);

        Console.WriteLine($"Tag cloud visualization saved to file {filePath}");
    }

    [Test]
    public void Ctor_ShouldCreateCorrectRectangle()
    {
        testLayouter = new CircularCloudLayouter(new Point(0, 0));
        var expectedRectangle = new Rectangle(-10, 10, 20, 20);

        var actualRectangle = testLayouter.PutNextRectangle(new Size(20, 20));

        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }

    [Test]
    public void PutNextRectangle_ShouldIncreaseRectanglesCount()
    {
        testLayouter = new CircularCloudLayouter(new Point(0, 0));

        testLayouter.PutNextRectangle(new Size(100, 100));

        testLayouter.Rectangles.Should().HaveCount(1);
    }

    [Test]
    public void PutNextRectangle_ShouldNotIntersectWithPrevious()
    {
        testLayouter = new CircularCloudLayouter(new Point(0, 0));
        testLayouter.PutNextRectangle(new Size(20, 20));

        AreRectanglesNotIntersected(testLayouter.Rectangles).Should().BeTrue();
    }

    [Test]
    public void PutNextRectangle_ShouldNotPutOneRectangleInsideAnother()
    {
        testLayouter = new CircularCloudLayouter(new Point(0, 0));
        testLayouter.PutNextRectangle(new Size(20, 20));

        AreRectanglesNotIntersected(testLayouter.Rectangles).Should().BeTrue();
    }

    [Test]
    public void Rectangles_ShouldReturnCorrectRectangles_OnMultipleSmallSizes()
    {
        testLayouter = new CircularCloudLayouter(new Point(0, 0));

        var rectangles = new List<Rectangle>
        {
            testLayouter.PutNextRectangle(new Size(2, 2)),
            testLayouter.PutNextRectangle(new Size(4, 2)),
            testLayouter.PutNextRectangle(new Size(5, 3)),
            testLayouter.PutNextRectangle(new Size(7, 4)),
            testLayouter.PutNextRectangle(new Size(8, 6)),
            testLayouter.PutNextRectangle(new Size(9, 5))
        };

        AreRectanglesNotIntersected(rectangles).Should().BeTrue();
    }

    [Test]
    public void Rectangles_ShouldReturnCorrectRectangles_OnMultipleMediumSizes()
    {
        testLayouter = new CircularCloudLayouter(new Point(0, 0));

        var rectangles = new List<Rectangle>
        {
            testLayouter.PutNextRectangle(new Size(20, 20)),
            testLayouter.PutNextRectangle(new Size(25, 15)),
            testLayouter.PutNextRectangle(new Size(18, 8)),
            testLayouter.PutNextRectangle(new Size(30, 25)),
            testLayouter.PutNextRectangle(new Size(28, 25)),
            testLayouter.PutNextRectangle(new Size(28, 14))
        };

        AreRectanglesNotIntersected(rectangles).Should().BeTrue();
    }

    [Test]
    public void Rectangles_ShouldReturnCorrectRectangles_OnMultipleBigSizes()
    {
        testLayouter = new CircularCloudLayouter(new Point(0, 0));

        var rectangles = new List<Rectangle>
        {
            testLayouter.PutNextRectangle(new Size(50, 35)),
            testLayouter.PutNextRectangle(new Size(48, 42)),
            testLayouter.PutNextRectangle(new Size(40, 45)),
            testLayouter.PutNextRectangle(new Size(52, 47)),
            testLayouter.PutNextRectangle(new Size(51, 60)),
            testLayouter.PutNextRectangle(new Size(68, 50))
        };

        AreRectanglesNotIntersected(rectangles).Should().BeTrue();
    }

    [Test]
    public void Rectangles_ShouldReturnCorrectRectangles_OnMultipleRandomSizes()
    {
        testLayouter = new CircularCloudLayouter(new Point(0, 0));

        var rectangles = new List<Rectangle>
        {
            testLayouter.PutNextRectangle(new Size(1, 12)),
            testLayouter.PutNextRectangle(new Size(40, 52)),
            testLayouter.PutNextRectangle(new Size(85, 45)),
            testLayouter.PutNextRectangle(new Size(28, 6)),
            testLayouter.PutNextRectangle(new Size(50, 45)),
            testLayouter.PutNextRectangle(new Size(32, 28))
        };

        AreRectanglesNotIntersected(rectangles).Should().BeTrue();
    }

    private static bool AreRectanglesNotIntersected(List<Rectangle> rectangles)
    {
        return !rectangles.Where((firstRectangle, firstIndex) => rectangles
                .Where((_, secondIndex) => firstIndex != secondIndex)
                .Any(secondRectangle => !AreRectanglesValid(firstRectangle, secondRectangle)))
            .Any();
    }

    private static bool AreRectanglesValid(Rectangle firstRectangle, Rectangle secondRectangle)
    {
        return !RectanglesChecker.IsRectangleIsInsideAnother(firstRectangle, secondRectangle) 
               && !RectanglesChecker.AreRectanglesIntersect(firstRectangle, secondRectangle);
    }
}