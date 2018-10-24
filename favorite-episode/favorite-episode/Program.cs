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

            while(!ready)
            {
                Console.WriteLine("Please enter which season you would like to review (1-7): ");
                string seasonNumber = Console.ReadLine();

                if(seasonAndEpisodeNumbersDictionary.ContainsKey(seasonNumber))
                {
                    Console.WriteLine("Good job, I'll look for it.");
                    Console.WriteLine();
                    bool readyAgain = false;
                    while (!readyAgain)
                    {
                        Console.WriteLine("Now what is the episode number?");
                        string episodeNumber = Console.ReadLine();
                        if(seasonAndEpisodeNumbersDictionary[seasonNumber].Contains(episodeNumber))
                        {
                            // Find the episode object
                            Episode foundEpisode = FindEpisode(episodes, seasonNumber, episodeNumber);
                            DisplayEpisodeInfo(foundEpisode);

                            // Display current reviews
                            DisplayCurrentReviews(foundEpisode);

                            do {
                                //
                                bool repeatMenu = false;

                                // Call menu function to add/edit/delete reviews
                                DisplayCRUDMenu();

                                // Accept user input from menu choice
                                string menuChoice = Console.ReadLine();

                                switch (menuChoice)
                                {
                                    case "1":
                                        // call add review function
                                        AddReview(foundEpisode);
                                        break;
                                    case "2":
                                        // call edit review function
                                        Console.WriteLine("Which review number would you like to edit?");
                                        string reviewEditNumber = Console.ReadLine();
                                        EditReview(foundEpisode, reviewEditNumber);
                                        break;
                                    case "3":
                                        // call delete review function
                                        Console.WriteLine("Which review number would you like to delete?");
                                        string reviewDeleteNumber = Console.ReadLine();
                                        bool isItDeleted = DeleteReview(foundEpisode, reviewDeleteNumber);

                                        if(!isItDeleted)
                                        {
                                            repeatMenu = true;
                                        }

                                        break;
                                    case "e":
                                        // exit menu
                                        break;
                                    default:
                                        Console.WriteLine("Invalid choice. Please try again.");
                                        // display menu again and have them choose again
                                        repeatMenu = true;
                                        break;
                                }

                                if(!repeatMenu)
                                {
                                    break;
                                }

                            } while (true);

                            // Ask user if they want to quit or review more episodes
                            Console.WriteLine();
                            Console.WriteLine("Type 'quit or q' to quit.");
                            Console.WriteLine("Type 'more or m' to review more episodes.");
                            string userAction = Console.ReadLine();

                            if(userAction.ToLower() == "quit" || userAction.ToLower() == "q")
                            {
                                ready = true;
                                readyAgain = true;
                                break;
                            }
                            else if(userAction.ToLower() == "more" || userAction.ToLower() == "m")
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

            // Serialize all reviewed episodes to a new json file (it's currently overwritting the original json file with the new object data)
            fileName = Path.Combine(directory.FullName, "gilmoregirls.json");
            SerializeEpisodesToFile(episodes, fileName);
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

        public static void DisplayEpisodeInfo(Episode foundEpisode)
        {
            Console.WriteLine("I found the episode.");
            Console.WriteLine();
            Console.WriteLine("Episode Name: " + foundEpisode.EpisodeName);
            Console.WriteLine("Summary: " + foundEpisode.Summary);
            Console.WriteLine();
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

        public static void DisplayCRUDMenu()
        {
            // Menu: add episode, edit review by #, delete review by #
            Console.WriteLine("Menu:");
            Console.WriteLine("Type 1 to add a review");
            Console.WriteLine("Type 2 to edit a review");
            Console.WriteLine("Type 3 to delete a review");
            Console.WriteLine("Type e to exit menu");
            Console.WriteLine();
        }

        public static void DisplayCurrentReviews(Episode foundEpisode)
        {
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
        }

        public static void AddReview(Episode foundEpisode)
        {
            // Review the episode
            Console.WriteLine("Please enter your review: ");
            Console.WriteLine();
            string userReview = Console.ReadLine();

            if (userReview != "")
            {
                foundEpisode.ReviewEpisode(userReview);
            }
        }

        public static void EditReview(Episode foundEpisode, string reviewNumber)
        {
            int reviewsCount = foundEpisode.Reviews.Count();
            int reviewInt = Int32.Parse(reviewNumber);

            // Check that review number exists
            if(reviewInt <= reviewsCount && reviewInt >= 1)
            {
                Console.WriteLine("Please enter in your new review:");
                string newReview = Console.ReadLine();
                // Enter updated review in the list
                foundEpisode.Reviews[reviewInt - 1] = newReview;
            }
            else
            {
                Console.WriteLine("That is not a valid review number.");
                // should make this take you back to that same episode
            }
        }

        public static bool DeleteReview(Episode foundEpisode, string reviewNumber)
        {
            bool isItDeleted = false;

            int reviewsCount = foundEpisode.Reviews.Count();
            int reviewInt = Int32.Parse(reviewNumber);

            // Check that review number exists
            if (reviewInt <= reviewsCount && reviewInt >= 1)
            {
                Console.WriteLine("Are you sure you want to delete? Enter y/n: ");
                string deleteInput = Console.ReadLine();

                if(deleteInput == "y")
                {
                    foundEpisode.Reviews.RemoveAt(reviewInt - 1);
                    isItDeleted = true;
                }
                else
                {
                    Console.WriteLine("Nothing deleted.");
                }
            }
            else
            {
                Console.WriteLine("That is not a valid review number.");
                // this should take you back to the same review menu
            }

            return isItDeleted;
        }
    }
}