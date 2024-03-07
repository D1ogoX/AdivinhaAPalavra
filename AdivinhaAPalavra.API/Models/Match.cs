using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace AdivinhaAPalavra.API.Models
{
    public class Match
    {
        private string filePath = "matches.json";
        private string wordsFilePath = "words.json";

        //private static string[] portugueseWords = { "casar", "garfo", "comer", "filme", "solta", "almas", "peito", "manta", "tapas", "datas", "gatas", "patas", "navio", "vazio", "audio", "pista", "vadio", "sacar", "nisso", "sacos", "ansia", "pinos", "hinos", "horas", "foras" };
        private static string[] portugueseWords = { "çãóêa" };

        public string Id { get; set; }
        public string Word { get; set; }
        public DateTime date { get; set; }

        private readonly Models.DBConnection _dbConnection = new DBConnection();

        public Match()
        {
            filePath = "matches_" + DateTime.Now.ToString("yyyyMMdd") + ".json";

            _dbConnection.IsConnect();
        }

        public string getWord(string token)
        {
            string word = string.Format("SELECT word from words where id = (select id_word from matches where Id = '{0}')", token);

            return RemoveDiacritics(_dbConnection.ExecuteSelectQuery(word).Rows[0][0].ToString());
        }

        public bool checkWord(string word)
        {
            string query = string.Format("SELECT word from words");

            List<string> wordList = new List<string>();
            wordList = _dbConnection.ExecuteSelectQuery(query).AsEnumerable()
                           .Select(r => RemoveDiacritics(r.Field<string>("word").ToUpper()))
                           .ToList();

            return wordList.BinarySearch(word.ToUpper()) != 0 ? true : false;
        }

        public string createMatch()
        {
            var word = ReadWordFromDB();

            string matchId = Guid.NewGuid().ToString();

            string queryInsert = string.Format("INSERT INTO matches (id, id_word) values ('{0}', {1})", matchId, word.Item1) ;

            _dbConnection.InsertQuery(queryInsert);

            return matchId;
        }

        public bool finish(string token)
        {
            List<Models.Match> matches = ReadMatchesFromFile();

            Models.Match match = matches.Where(x => x.Id == token).FirstOrDefault();

            matches.RemoveAt(matches.IndexOf(match));

            saveMatches(matches);

            return true;
        }

        private List<Models.Match> ReadMatchesFromFile()
        {
            List<Models.Match> matches = new List<Models.Match>();
            if (fileExists(this.filePath))
            {
                string json = System.IO.File.ReadAllText(this.filePath);
                matches = JsonConvert.DeserializeObject<List<Models.Match>>(json);
            }
            return matches;
        }

        private List<string> ReadWordsFromFile()
        {
            List<string> words = new List<string>();
            if (fileExists(this.wordsFilePath))
            {
                string json = System.IO.File.ReadAllText(this.wordsFilePath);
                words = JsonConvert.DeserializeObject<List<string>>(json);
            }
            return words;
        }

        private (int,string) ReadWordFromDB()
        {
            string query = "SELECT id, word from words order by RAND() LIMIT 1";

            DataTable dt = _dbConnection.ExecuteSelectQuery(query);

            return (Convert.ToInt32(dt.Rows[0][0]), dt.Rows[0][1].ToString());
        }

        private bool fileExists(string path)
        {
            if (System.IO.File.Exists(path))
                return true;

            return false;
        }

        private void saveMatches(List<Models.Match> matches)
        {
            string json = JsonConvert.SerializeObject(matches, Formatting.Indented);
            System.IO.File.WriteAllText(this.filePath, json);
        }

        private string RemoveDiacritics(string text)
        {
            return string.Concat(
      text.Normalize(NormalizationForm.FormD)
      .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                    UnicodeCategory.NonSpacingMark)
    ).Normalize(NormalizationForm.FormC);
        }
    }
}
