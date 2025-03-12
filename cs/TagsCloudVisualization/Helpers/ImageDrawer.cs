using System.Drawing;
using TagsCloudVisualization.Models;

#pragma warning disable CA1416 // Проверка совместимости платформы
namespace TagsCloudVisualization.Helpers
{
    public class ImageDrawer
    {
        private readonly string outputPath = Directory
            .GetCurrentDirectory()
            .Replace(@"\", "/")
            .Replace("/bin/Debug/net8.0", "/");

        private readonly Size imageSize = new(800, 600);

        public void DrawTagsCloudFromEmptyRectangles(List<Rectangle> clouds, string imageName)
        {
            using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);

            var imageCenterX = bitmap.Width / 2f;
            var imageCenterY = bitmap.Height / 2f;

            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Black);

            var centerX = clouds.Average(r => r.Left + r.Width / 2f);
            var centerY = clouds.Average(r => r.Top + r.Height / 2f);
            var offsetX = imageCenterX - centerX;
            var offsetY = imageCenterY - centerY;

            var brush = new SolidBrush(Color.LightBlue);
            var pen = new Pen(Color.Black, 2);

            foreach (var rectangle in clouds)
            {
                var centeredRect = rectangle with { X = (int)(rectangle.Left + offsetX), Y = (int)(rectangle.Top + offsetY) };

                graphics.FillRectangle(brush, centeredRect);

                graphics.DrawRectangle(pen, centeredRect);
            }

            bitmap.Save($"{outputPath}{imageName}.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public void DrawTagsCloudFromTags(List<CloudTag> cloudTags, Func<Size, Rectangle> nextRectangleMaker, string imageName)
        {
            using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.AliceBlue);

            var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var textBrush = new SolidBrush(Color.Black);

            var pen = new Pen(Color.Transparent, 2);

            foreach (var (priority, word) in cloudTags)
            {
                var font = new Font("Arial", priority + 10, FontStyle.Bold);
                var textSize = graphics.MeasureString(word, font);
                var nextRectangle = nextRectangleMaker(new Size((int)textSize.Width + 2, (int)textSize.Height));

                graphics.FillRectangle(Brushes.Transparent, nextRectangle);
                graphics.DrawRectangle(pen, nextRectangle);

                graphics.DrawString(
                    word,
                    font,
                    textBrush,
                    nextRectangle,
                    stringFormat
                );
            }

            bitmap.Save($"{outputPath}{imageName}.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
