namespace TagsCloudVisualization.Helpers
{
    public class TextProcessor
    {
        public Dictionary<string, int> GetTextTagsFromFile(Stream file)
        {
            var tags = new Dictionary<string, int>();
            var reader = new StreamReader(file);

            var line = reader.ReadLine();
            while (line != null)
            {
                var words = line.Split(' ');

                foreach (var word in words)
                {
                    var clearedWord = ClearWordFromNonPunctuationMarks(word);
                    if (string.IsNullOrEmpty(clearedWord))
                    {
                        continue;
                    }

                    if (!tags.TryAdd(clearedWord, 1))
                    {
                        tags[clearedWord] += 1;
                    }
                }
                
                line = reader.ReadLine();
            }

            return tags;
        }

        private string ClearWordFromNonPunctuationMarks(string word)
        {
            return new string(word.Where(char.IsLetter).ToArray());
        }
    }
}
