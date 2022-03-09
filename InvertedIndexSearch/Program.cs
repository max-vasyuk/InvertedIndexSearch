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
        
        public static void PrintList2(List<(string, int)> list)
        {
            foreach (var l in list)
            {
                Console.WriteLine("{0} {1}", l.Item1, l.Item2);
            }
        }

        public static (int, int) findPriorOp(List<(string, int)> words)
        {
            if (words.Count == 1)
            {
                return (-1, 0);
            }
            int minIdx = -1;
            int minOp = int.MaxValue;
            int i = 0;
            foreach (var (_, prior) in words)
            {
                if (prior < minOp && prior != -1)
                {
                    minOp = prior;
                    minIdx = i;
                }
                i++;
            }
            
            return (minIdx != -1)?(minOp, minIdx):(-1,  0);
        }
        
        public static (List<(string, int)>, List<(string, int)>) Split(this List<(string, int)> source, int idx)
        {
            var l1 = source.Where(x => source.FindIndex(x2=> x.Item1 == x2.Item1) < idx).ToList();
            var l2 = source.Where(x => source.FindIndex(x2=> x.Item1 == x2.Item1) > idx).ToList();

            return (l1, l2);
        }

        public static Node BuildTree(List<(string, int)> priorWords)
        {
            if (priorWords.Count == 0)
            {
                return null;
            }
            if (priorWords.Count == 1)
            {
                return new Node(priorWords[0].Item1);
            }
            var (minOp, minIdx) = findPriorOp(priorWords);
            var (leftSubNode, rightSubNode) = Split(priorWords, minIdx);
            return new Node(priorWords[minIdx].Item1, BuildTree(leftSubNode), BuildTree(rightSubNode));
        }

        public static void ViewTree(Node tree, int level)
        {
            if (!(tree is null))
            {
                ViewTree(tree.rightNode , level + 1); // Вывод правого поддерева
                for ( int i = 0; i < level; i++) Console.Write("\t");
                Console.WriteLine(" {0}", tree.data);
                ViewTree(tree.leftNode , level + 1); // Вывод левого поддерева
            }
        }

        public static void Main(string[] args)
        {
            //TODO: обход дерева Left-Right-Root
            //TODO: выдача результатов из инвертированного словаря
            //TODO: упорядочивание по частоте
            //TODO: учитывать отсутствие логических операций в запросе
            // request processing
            Console.WriteLine("Enter request:");
            var request = Console.ReadLine();
            var requestWords = ParseRawStr(request);
            var priorWords = Prioritize(requestWords);
            var tree = BuildTree(priorWords);
            ViewTree(tree, 0);
            /*tree.ViewNode();
            if (!(tree.leftNode is null))
            {
                tree.leftNode.ViewNode();
            }

            if (!(tree.rightNode is null))
            {
                tree.rightNode.rightNode.ViewNode();
            }*/
        }
    }
}