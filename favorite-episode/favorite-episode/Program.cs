using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FavoriteEpisode
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            var fileName = Path.Combine(directory.FullName, "gilmoregirls.json");
            var episodes = DeserializeEpisodes(fileName);

            // Ask user which episode (season # and episode #)
            // or search name of episode

            // Find the episode object

            // Ask user to leave a review (string) or rating (int)

            // Serialize to file after each change (or save all and do when they exit?)

            foreach (var episode in episodes)
            {
                Console.WriteLine("Season " + episode.Season + " Episode " + episode.EpisodeNumber + ": Episode name: " + episode.EpisodeName);
            }
        }

        public static List<Episode> DeserializeEpisodes(string fileName)
        {
            var episodes = new List<Episode>();
            var serializer = new JsonSerializer();
            using(var reader = new StreamReader(fileName))
            using(var jsonReader = new JsonTextReader(reader))
            {
                episodes = serializer.Deserialize<List<Episode>>(jsonReader);
            }
            return episodes;
        }

        public static void SerializeEpisodesToFile(List<Episode> episodes, string fileName)
        {
            var serializer = new JsonSerializer();
            using(var writer = new StreamWriter(fileName))
            using(var jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, episodes);
            }
        }
    }
}
