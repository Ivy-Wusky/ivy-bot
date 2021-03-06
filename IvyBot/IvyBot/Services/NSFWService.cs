using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IvyBot.Services {
    public class NSFWService {
        public static async Task<string> GetRule34File (string tag) {
            try {
                var random = new Random ();
                var url = $"http://rule34.xxx/index.php?page=dapi&s=post&q=index&limit=100&tags={tag.Replace(" ", "_")}";
                var webpage = await SearchService.GetResponseStringAsync (url);
                var matches = Regex.Matches (webpage, "file_url=\"(?<url>.*?)\"");

                if (matches.Count == 0)
                    return "No results found";

                var match = matches[random.Next (0, matches.Count)];
                return matches[random.Next (0, matches.Count)].Groups["url"].Value;
            } catch (Exception ex) {
                return $"Error in Rule34 search: {ex.Message}";
            }
        }

        public static async Task<string> GetE621File (string tag) {
            try {
                var headers = new Dictionary<string, string> { { "User-Agent", "IvyBot/6.2.0 (https://github.com/ivydrinkscoffee/IvyBot)" } };

                var random = new Random ();
                var url = $"http://e621.net/posts.json?limit=100&tags={tag.Replace(" ", "&")}";
                var webpage = await SearchService.GetResponseStringAsync (url, headers);
                var matches = Regex.Matches (webpage, @"""url"":""(?<url>.*?)""");

                if (matches.Count == 0)
                    return "No results found";

                var match = matches[random.Next (0, matches.Count)];
                return matches[random.Next (0, matches.Count)].Groups["url"].Value;
            } catch (Exception ex) {
                return $"Error in E621 search: {ex.Message}";
            }
        }
    }
}
