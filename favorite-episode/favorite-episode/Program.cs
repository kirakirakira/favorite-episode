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
            bool ready = false;
            Console.WriteLine("Welcome to the Gilmore Girls Rating and Reviews App.");

            while(!ready)
            {
                Console.WriteLine("Please enter which season you would like to review (1-7): ");
                string entry = Console.ReadLine();
                try
                {
                    int seasonNumber = Convert.ToInt16(entry);

                    if (seasonNumber <= 7 && seasonNumber >= 1)
                    {
                        Console.WriteLine("Good job, I'll look for it. Now what is the episode number?");
                        string epEntry = Console.ReadLine();
                        try
                        {
                            int episodeNumber = Convert.ToInt16(epEntry);
                            Console.WriteLine("Okay I am looking for Season {0} Episode {1}.", seasonNumber, episodeNumber);
                            // Find the episode object
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine(epEntry + " is not a number!");
                        }
                        ready = true;
                    }
                    else
                    {
                        Console.WriteLine("There are only seasons 1 to 7 of Gilmore Girls. Try again.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine(entry + " is not a number!");
                }
            }

            // Ask user to leave a review (string) or rating (int)

            // Serialize to file after each change (or save all and do when they exit?)

            //foreach (var episode in episodes)
            //{
            //    Console.WriteLine("Season " + episode.Season + " Episode " + episode.EpisodeNumber + ": Episode name: " + episode.EpisodeName);
            //}
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
