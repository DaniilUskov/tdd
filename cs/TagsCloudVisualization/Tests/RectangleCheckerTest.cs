using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Helpers;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class RectangleCheckerTest
{
    [Test]
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 5, 5, 15, 15 })] // Частичное пересечение
    [TestCase(new[] { -5, -5, 5, 5 }, new[] { 0, 0, 10, 10 })] // Пересечение в одной точке
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 2, 2, 8, 8 })]   // Второй прямоугольник внутри первого
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { -5, -5, 15, 15 })] // Первый прямоугольник внутри второго
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 10, 10, 20, 20 })] // Касание углами
    public void AreRectanglesIntersect_ShouldReturnTrue_WhenRectanglesIntersect(int[] firstRectCoordinates, int[] secondRectCoordinates)
    {
        var rectangles = GetRectangles(firstRectCoordinates, secondRectCoordinates);
        
        var actual = RectanglesChecker.AreRectanglesIntersect(rectangles.first, rectangles.second);

        actual.Should().BeTrue();
    }
    
    [Test]
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 15, 15, 25, 25 })] // Прямоугольники далеко друг от друга
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 10, 11, 20, 20 })] // Прямоугольники касаются сторонами, но не пересекаются
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { -15, -15, -5, -5 })] // Прямоугольники в разных квадрантах
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 11, 0, 20, 10 })] // Прямоугольники рядом по горизонтали
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 0, 11, 10, 20 })] // Прямоугольники рядом по вертикали
    public void AreRectanglesIntersect_ShouldReturnFalse_WhenRectanglesNotIntersect(int[] firstRectCoordinates, int[] secondRectCoordinates)
    {
        var rectangles = GetRectangles(firstRectCoordinates, secondRectCoordinates);
        
        var actual = RectanglesChecker.AreRectanglesIntersect(rectangles.first, rectangles.second);

        actual.Should().BeFalse();
    }
    
    [Test]
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 2, 2, 8, 8 })] // Второй прямоугольник полностью внутри первого
    [TestCase(new[] { -5, -5, 5, 5 }, new[] { -4, -4, 4, 4 })] // Второй прямоугольник внутри первого (с отрицательными координатами)
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 0, 0, 10, 10 })] // Прямоугольники идентичны
    public void IsRectangleIsInsideAnother_ShouldReturnTrue_WhenRectangleIsInsideAnother(int[] firstRectCoordinates, int[] secondRectCoordinates)
    {
        var rectangles = GetRectangles(firstRectCoordinates, secondRectCoordinates);

        var actual = RectanglesChecker.IsRectangleIsInsideAnother(rectangles.first, rectangles.second) 
                     || RectanglesChecker.IsRectangleIsInsideAnother(rectangles.second, rectangles.first);

        actual.Should().BeTrue();
    }
    
    [Test]
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 5, 5, 15, 15 })] // Второй прямоугольник частично выходит за пределы первого
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { -5, -5, 5, 5 })] // Второй прямоугольник частично выходит за пределы первого (с отрицательными координатами)
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 11, 11, 20, 20 })] // Второй прямоугольник полностью снаружи первого
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 11, 0, 20, 10 })] // Второй прямоугольник снаружи по горизонтали
    [TestCase(new[] { 0, 0, 10, 10 }, new[] { 0, 0, 11, 11 })] // Второй прямоугольник выходит за пределы первого по ширине и высоте
    public void IsRectangleIsInsideAnother_ShouldReturnFalse_WhenRectangleIsNotInsideAnother(int[] firstRectCoordinates, int[] secondRectCoordinates)
    {
        var rectangles = GetRectangles(firstRectCoordinates, secondRectCoordinates);

        var actual = RectanglesChecker.IsRectangleIsInsideAnother(rectangles.first, rectangles.second)
                     || RectanglesChecker.IsRectangleIsInsideAnother(rectangles.second, rectangles.first);

        actual.Should().BeFalse();
    }

    private (Rectangle first, Rectangle second) GetRectangles(int[] firstRect, int[] secondRect)
    {
        var firstRectangle = new Rectangle(new Point(firstRect[0], firstRect[1]), new Size(firstRect[2], firstRect[3]));
        var secondRectangle = new Rectangle(new Point(secondRect[0], secondRect[1]), new Size(secondRect[2], secondRect[3]));
        
        return (firstRectangle, secondRectangle);
    }
}