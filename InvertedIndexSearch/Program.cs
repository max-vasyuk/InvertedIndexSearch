using System.Text;
using System.Text.RegularExpressions;

namespace InvertedIndexSearch
{
    public static class Program
    {
        public static List<string> filepaths = new();
        public static Dictionary<string, List<(string, int)>> dictionary = new();
        public static int minPrior = int.MaxValue;
        public static int indexMinPrior = -1;

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

        public static void FormDictionary(string filePath)
        {
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
        }

        public static void PrintList(List<string> list)
        {
            foreach (var l in list)
            {
                Console.WriteLine(l);
            }
        }

        public static List<(string, int)> Prioritize(List<string> words)
        {
            var priorList = new List<(string, int)>();
            int i = 0;
            foreach (var word in words)
            {
                switch (word)
                {
                    case "не":
                        priorList.Add((word, 2));
                        if (minPrior > 2)
                        {
                            minPrior = 2;
                            indexMinPrior = i;
                        }
                        break;
                    case "и":
                        priorList.Add((word, 1));
                        if (minPrior > 1)
                        {
                            minPrior = 1;
                            indexMinPrior = i;
                        }
                        break;
                    case "или":
                        priorList.Add((word, 0));
                        if (minPrior > 0)
                        {
                            minPrior = 0;
                            indexMinPrior = i;
                        }
                        break;
                    default:
                        priorList.Add((word, -1));
                        break;
                }
                i++;
            }
            return priorList;
        }

        public static Node buildTree(List<string> words)
        {
            if (words.Count == 3)
            {
                return new Node(words[1], new Node(words[0]), new Node(words[2]));
            }
            return new Node("dummy");
        }

        public static void Main(string[] args)
        {
            // request processing
            Console.WriteLine("Enter request:");
            var request = Console.ReadLine();
            var requestWords = ParseRawStr(request);
            PrintList(requestWords);

            var priorWords = Prioritize(requestWords);
            foreach (var (word, prior) in priorWords)
            {
                Console.WriteLine("{0}: {1}", word, prior);
            }
            Console.WriteLine("minPrior: {0}, idx: {1}", minPrior, indexMinPrior);
            //buildTree(requestWords).ViewNode();


        }
    }
}