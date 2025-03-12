using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Helpers;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class TextProcessorTests
{
    private TextProcessor textProcessor = null!;

    [SetUp]
    public void SetUp()
    {
        textProcessor = new TextProcessor();
    }

    [Test]
    public void GetTextTagsFromFile_ShouldReturnEmptyTuple_WhenFileIsEmpty()
    {
        var stream = GetTextFile("");
        
        var actual = textProcessor.GetTextTagsFromFile(stream);
        
        stream.Close();
        
        actual.Should().BeEmpty();
    }

    [Test]
    [TestCase("Sam 1,is 1,a 1,brave 1,Hobbit 1", "Sam is a brave Hobbit.")]
    [TestCase("Sam 1,is 1,a 1,very 2,brave 1,Hobbit 1", "Sam is a very very brave Hobbit.")]
    public void GetTextTagsFromFile_ShouldReturnCorrectTags_WhenFileContainsText(string expected, string inputText)
    {
        var stream = GetTextFile(inputText);
        
        var actual = textProcessor.GetTextTagsFromFile(stream);
        
        stream.Close();
        
        actual.Should().BeEquivalentTo(GetTagsFromExpected(expected));
    }

    [Test]
    [TestCase("Sam 1,is 1,a 1,very 2,brave 1,Hobbit 1", "Sam is a very\n very brave Hobbit")]
    public void GetTextTagsFromFile_ShouldReturnCorrectTags_WhenTextContainsEmptyLines(string expected, string inputText)
    {
        var stream = GetTextFile(inputText);
        
        var actual = textProcessor.GetTextTagsFromFile(stream);
        
        stream.Close();
        
        actual.Should().BeEquivalentTo(GetTagsFromExpected(expected));
    }

    private Dictionary<string, int> GetTagsFromExpected(string expected)
    {
        var tags = new Dictionary<string, int>();
        
        var splittedText = expected.Split(',');

        foreach (var item in splittedText)
        {
            var splittedItem = item.Split(' ');

            tags[splittedItem[0]] = int.Parse(splittedItem[1]);
        }

        return tags;
    }

    private MemoryStream GetTextFile(string text)
    {
        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        
        streamWriter.WriteLine(text);
        streamWriter.Flush();

        stream.Position = 0;

        return stream;
    }
}