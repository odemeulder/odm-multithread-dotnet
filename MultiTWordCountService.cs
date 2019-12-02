using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace OdmWordCount
{
    public class MultiTWordCountService {

      private static bool isValidChar(char c) => !char.IsPunctuation(c)  && c != '|';

      private static string cleanLine(string input) {
        string rv = new string(input.Where(isValidChar).ToArray());
        rv = rv.Replace('\t', ' ');
        rv = rv.ToLower();
        return rv;
      }

      private static List<string> ingestLines() {
        List<string> rv = new List<string>();
        using (StreamReader inputFile = new StreamReader(Config.INPUTFILE)) {
          while (!inputFile.EndOfStream) 
            rv.Add(inputFile.ReadLine());
        }
        return rv;
      }

      private static Dictionary<string, int> countWordsForLines(string input) {
        string inputWithoutPunctuation = cleanLine(input);
        var words = inputWithoutPunctuation.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Dictionary<string, int> rv = new Dictionary<string, int>();
        foreach(string word in words) {
          rv[word] = rv.GetValueOrDefault(word) + 1;
        }
        return rv;
      }

      private static IDictionary<string, int> countWords(List<string> lines) {
        ConcurrentDictionary<string, int> rv = new ConcurrentDictionary<string, int>();;
        Parallel.ForEach(lines, (line, loopState) => {
          var lineDictionary = countWordsForLines(line);
          foreach(var entry in lineDictionary) {
            rv[entry.Key] = entry.Value + rv.GetValueOrDefault(entry.Key);
          }
        });
        return rv;
      }

      private static void writeOutput(IDictionary<string, int> map) {
        using (StreamWriter outputFile = new StreamWriter(Config.OUTPUTFILEMULTI))
        {
          foreach (KeyValuePair<string, int> entry in map)
            outputFile.WriteLine($"{entry.Key}: {entry.Value}");
        }
      }

      public static void GenerateWordCount() {
        var lines = ingestLines();
        var map = countWords(lines);
        writeOutput(map);
      }

    }
}