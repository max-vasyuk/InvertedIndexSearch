using System.Text;
using System.Text.RegularExpressions;

namespace InvertedIndexSearch
{
    public static class Program
    {
        public static List<string> filepaths = new ();
        public static Dictionary<string, List<(string, int)>> dictionary = new ();

        public static List<(string, int)> GetCountOccurrencesFromList(List<string> words)
        {
            List<(string, int)> countOccurrences = new();
            var g = words.GroupBy( i => i );
            foreach( var grp in g )
            {
                countOccurrences.Add((grp.Key, grp.Count()));
            }

            return countOccurrences;
        } 

        public static List<string> ParseRawStr(string rawStr)
        {
            List<string> words = new(); 
            Regex pattern = new Regex(@"[а-яА-Яa-zA-Z]+",RegexOptions.IgnorePatternWhitespace);
            foreach (Match m in pattern.Matches(rawStr))
            {
                words.Add(m.Value.ToLower());
            }
            return words;
        }
        
        public static List<string> GetWordsFromFile(string path)
        {
            StringBuilder stringBuilder = new ();
            StreamReader streamReader = new StreamReader(path);
            while (!streamReader.EndOfStream)
            {
                stringBuilder.Append(streamReader.ReadLine()); 
            }
            List<string> wordsFromFile = ParseRawStr(stringBuilder.ToString());
            return wordsFromFile;
        }

        public static void Main(string[] args)
        {
            var start = DateTime.Now;
            const string bathPath = @"files";
            filepaths = Directory.GetFiles(bathPath, "*", SearchOption.AllDirectories).Select(Path.GetFileName).ToList();

            StringBuilder stringBuilder = new ();
            foreach (var path in filepaths)
            {
                stringBuilder.Append(bathPath).Append('/').Append(path);
                var words = GetWordsFromFile(stringBuilder.ToString());
                var countOccurrences = GetCountOccurrencesFromList(words);
                foreach (var (word, count) in countOccurrences)
                {
                    List<(string, int)> val = new();
                    if (dictionary.TryGetValue(word, out val))
                    {
                        dictionary[word].Add((path, count));
                    }
                    else
                    {
                        dictionary.Add(word, new List<(string, int)>
                        {
                            (path, count)
                        });
                    }
                }
                
                stringBuilder.Clear();
            }
            var stop = DateTime.Now;
            Console.WriteLine(stop - start);
        }
    }
}