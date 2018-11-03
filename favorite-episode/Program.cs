using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FavoriteEpisode
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Get current directly and create file name for data set: gilmoregirls.json
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            var fileName = Path.Combine(directory.FullName, "gilmoregirls.json");

            // Deserialize data in gilmoregirls.json
            var episodes = DeserializeEpisodes(fileName);

            // Create a list of season numbers (use for verification of user input)
            var seasonNumbers = GetSeasonNumbers(episodes);

            // Create a dictionary of season number: list of episode numbers (use for verification of user input)
            Dictionary<string, List<string>> seasonAndEpisodeNumbersDictionary = new Dictionary<string, List<string>>();

            // Populate the dictionary with season number: list of episode numbers in that season
            foreach(string season in seasonNumbers)
            {
                seasonAndEpisodeNumbersDictionary.Add(season, GetEpisodeNumbers(episodes, season));
            }

            //Ask user which episode (season # and episode #) for review
            //or search name of episode
            bool ready = false;

            Console.Title = "ASCII Art";
            string title = @"

   ___ _ _                      ___ _     _    
  / __(_| |_ __  ___ _ _ ___   / __(_)_ _| |___
 | (_ | | | '  \/ _ | '_/ -_) | (_ | | '_| (_-<
  \___|_|_|_|_|_\___|_| \___|  \___|_|_| |_/__/
                                               

            ";

            Console.WriteLine(title);

            Console.WriteLine("Welcome to the Gilmore Girls Rating and Reviews App.");

            while (!ready)
            {
                Console.WriteLine("Please enter which season you would like to review (1-7): ");
                string seasonNumber = Console.ReadLine();

                if (seasonAndEpisodeNumbersDictionary.ContainsKey(seasonNumber))
                {
                    Console.WriteLine("Good job, I'll look for it.");
                    bool readyAgain = false;
                    while (!readyAgain)
                    {
                        Console.WriteLine("Now what is the episode number?");
                        string episodeNumber = Console.ReadLine();
                        if (seasonAndEpisodeNumbersDictionary[seasonNumber].Contains(episodeNumber))
                        {
                            // Find the episode object
                            Episode foundEpisode = FindEpisode(episodes, seasonNumber, episodeNumber);
                            Console.WriteLine("I found the episode.");
                            Console.WriteLine();
                            Console.WriteLine("Episode Name: " + foundEpisode.EpisodeName);
                            Console.WriteLine("Summary: " + foundEpisode.Summary);
                            Console.WriteLine();

                            // Display current reviews
                            if (foundEpisode.Reviews.Count > 0)
                            {
                                Console.WriteLine("Current reviews are...");
                                Console.WriteLine();
                                for (int i = 0; i < foundEpisode.Reviews.Count; i++)
                                {
                                    Console.WriteLine("Review #{0}: ", i + 1);
                                    Console.WriteLine(foundEpisode.Reviews[i]);
                                    Console.WriteLine();
                                }
                                Console.WriteLine();
                            }

                            // Review the episode
                            Console.WriteLine("Please enter your review: ");
                            Console.WriteLine();
                            string userReview = Console.ReadLine();

                            if (userReview != "")
                            {
                                foundEpisode.ReviewEpisode(userReview);
                                // Serialize episodes to json file - do it here so you don't have to properly exit for the reviews to save
                                fileName = Path.Combine(directory.FullName, "gilmoregirls.json");
                                SerializeEpisodesToFile(episodes, fileName);
                            }

                            // Ask user if they want to quit or review more episodes
                            Console.WriteLine();
                            Console.WriteLine("Type 'quit or q' to quit.");
                            Console.WriteLine("Type 'more or m' to review more episodes.");
                            string userAction = Console.ReadLine();

                            if (userAction.ToLower() == "quit" || userAction.ToLower() == "q")
                            {
                                ready = true;
                                readyAgain = true;
                                break;
                            }
                            else if (userAction.ToLower() == "more" || userAction.ToLower() == "m")
                            {
                                break;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Episode number not found. Try again.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Season number not found. Try again.");
                }
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
            // wrapping the streamwriter in a using takes care of disposing it when it's done
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

        public static List<String> GetSeasonNumbers(List<Episode> episodes)
        {
            List<string> seasonNumbers = episodes.Select(e => e.Season).Distinct().ToList();
            return seasonNumbers;
        }

        public static List<string> GetEpisodeNumbers(List<Episode> episodes, string seasonNumber)
        {
            List<string> episodeNumbers = episodes.Where(episode => episode.Season == seasonNumber).Select(n => n.EpisodeNumber).Distinct().ToList();
            return episodeNumbers;
        }
    }
}