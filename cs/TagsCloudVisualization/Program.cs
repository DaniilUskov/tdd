using System.Drawing;
using TagsCloudVisualization.Helpers;
using TagsCloudVisualization.Models;

#pragma warning disable CA1416 // Проверка совместимости платформы
namespace TagsCloudVisualization
{
    public static class Program
    {
        private static readonly string TextPath = Directory
            .GetCurrentDirectory()
            .Replace(@"\", "/")
            .Replace("/bin/Debug/net8.0", "/Texts/");
        
        public static void Main(string[] args)
        {
            DrawAndSaveTagCloud();
            
            DrawAndSaveCloudWithEmptyRectangles();
        }
        
        private static void DrawAndSaveCloudWithEmptyRectangles()
        {
            var imageDrawer = new ImageDrawer();
            imageDrawer.DrawTagsCloudFromEmptyRectangles(GetEmptyRectangles(), "CloudWithEmptyRectangles");
            
            Console.WriteLine("Cloud with empty rectangles was created successfully.");
        }

        private static void DrawAndSaveTagCloud()
        {
            var filePath = Path.Combine(TextPath, "Grand escape lyrics.txt");
            var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            
            var textProcessor = new TextProcessor();
            var tags = textProcessor.GetTextTagsFromFile(stream);
            var cloudTags = GetTagsWithPriority(tags);

            var imageDrawer = new ImageDrawer();
            var layouter = new CircularCloudLayouter(new Point(400, 300));
            imageDrawer.DrawTagsCloudFromTags(cloudTags.Take(30).ToList(), layouter.PutNextRectangle, "TextCloud");

            Console.WriteLine("Tag cloud was created successfully.");
        }

        private static List<CloudTag> GetTagsWithPriority(Dictionary<string, int> tags)
        {
            return tags.Select(x => new CloudTag(x.Value, x.Key)).ToList();
        }

        private static List<Rectangle> GetEmptyRectangles()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));

            circularCloudLayouter.PutNextRectangle(new Size(50, 35));
            circularCloudLayouter.PutNextRectangle(new Size(48, 42));
            circularCloudLayouter.PutNextRectangle(new Size(40, 45));
            circularCloudLayouter.PutNextRectangle(new Size(52, 47));
            circularCloudLayouter.PutNextRectangle(new Size(51, 60));
            circularCloudLayouter.PutNextRectangle(new Size(68, 50));
            circularCloudLayouter.PutNextRectangle(new Size(72, 65));
            circularCloudLayouter.PutNextRectangle(new Size(42, 20));
            circularCloudLayouter.PutNextRectangle(new Size(14, 22));

            return [.. circularCloudLayouter.Rectangles.OrderByDescending(x => x.Width * x.Height)];
        }
    }
}
