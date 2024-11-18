using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Draughts
{
    public static class Extension
    {
        static string path = ".\\results.json";
        public static bool IsInBound(Position pos)
            => pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        public static bool IsInBound(int row, int col)
            => row >= 0 && row < 8 && col >= 0 && col < 8;

        public static void SaveToFile(ResultEntry re)
        {

            if (File.Exists(path) && File.ReadAllText(path).Length == 0)
            {
                var results = new List<ResultEntry>()
                {
                    re
                };

                File.WriteAllText(path, JsonConvert.SerializeObject(results));
            }
            else if (!File.Exists(path))
            {
                File.Create(path).Close();

                var results = new List<ResultEntry>()
                {
                    re
                };

                File.WriteAllText(path, JsonConvert.SerializeObject(results));
            }
            else
            {
                var results = JsonConvert.DeserializeObject<List<ResultEntry>>(File.ReadAllText(path));
                results.Add(re);

                File.WriteAllText(path, JsonConvert.SerializeObject(results));
            }
        }

        public static List<ResultEntry> LoadFromFile()
        {
            if (!File.Exists(path) || File.ReadAllText(path).Length == 0)
                return new List<ResultEntry>();

            return JsonConvert.DeserializeObject<List<ResultEntry>>(File.ReadAllText(path));
        }
    }

}
