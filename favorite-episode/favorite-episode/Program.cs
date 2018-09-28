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

            var episodeInParticular = episodes.Find(i => (i.Season == "5") && (i.EpisodeNumber == "2"));
            Console.WriteLine(episodeInParticular.EpisodeName);

            //Ask user which episode (season # and episode #)
            //or search name of episode
            bool ready = false;
            List<Episode> reviewedEpisodes = new List<Episode>();
            Console.WriteLine("Welcome to the Gilmore Girls Rating and Reviews App.");

            while(!ready)
            {
                Console.WriteLine("Please enter which season you would like to review (1-7): ");
                string seasonNumber = Console.ReadLine();

                try
                {
                    int intSeasonNumber = Convert.ToInt16(seasonNumber);
                    if (intSeasonNumber <= 7 && intSeasonNumber >= 1)
                    {
                        Console.WriteLine("Good job, I'll look for it.");
                        bool readyAgain = false;
                        while (!readyAgain)
                        {
                            Console.WriteLine("Now what is the episode number?");
                            string episodeNumber = Console.ReadLine();
                            try
                            {
                                int intEpisodeNumber = Convert.ToInt16(episodeNumber);
                                Console.WriteLine("Okay I am looking for Season {0} Episode {1}.", seasonNumber, episodeNumber);
                                // Find the episode object
                                Episode foundEpisode = FindEpisode(episodes, seasonNumber, episodeNumber);
                                Console.WriteLine("I found the episode. Name is " + foundEpisode.EpisodeName);
                                Console.WriteLine("Summary: " + foundEpisode.Summary);

                                // Review the episode
                                Console.WriteLine("Please enter your review: ");
                                string userReview = Console.ReadLine();
                                foundEpisode.ReviewEpisode(userReview);
                                reviewedEpisodes.Add(foundEpisode);
                                ready = true;
                                readyAgain = true;
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("You didn't enter a number!");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("There are only seasons 1 to 7 of Gilmore Girls. Try again.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("You didn't enter a number!");
                }
            }

            // Serialize all reviewed episodes to a new json file (how do I combine this with the original file and overwrite it?)
            fileName = Path.Combine(directory.FullName, "reviewEpisodes.json");
            SerializeEpisodesToFile(reviewedEpisodes, fileName);

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

        public static Episode FindEpisode(List<Episode> episodes, string seasonNumber, string episodeNumber)
        {
            return episodes.Find(i => (i.Season == seasonNumber) && (i.EpisodeNumber == episodeNumber));
        }
    }
}
