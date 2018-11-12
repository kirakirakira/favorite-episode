﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FavoriteEpisode
{
    public class RootObject
    {
        public Episode[] Episode { get; set; }
    }

    // Encapsulation, Abstraction, and Inheritance
    public class Episode : Show
    {
        [JsonProperty(PropertyName = "name")]
        public string EpisodeName { get; set; }
        [JsonProperty(PropertyName = "season")]
        public string Season { get; set; }
        [JsonProperty(PropertyName = "number")]
        public string EpisodeNumber { get; set; }
        [JsonProperty(PropertyName = "airdate")]
        public string AirDate { get; set; }
        [JsonProperty(PropertyName = "airtime")]
        public string AirTime { get; set; }
        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }

        public List<Review> Reviews = new List<Review>();

        public void ReviewEpisode(Review review)
        {
            Reviews.Add(review);
        }

        public Dictionary<string, int> NumberReviewsByReviewer()
        {
            Dictionary<string, int> numberReviewsByReviewer = new Dictionary<string, int>();

            foreach(Review review in Reviews)
            {
                if (review.Reviewer != null)
                {
                    if (numberReviewsByReviewer.ContainsKey(review.Reviewer))
                    {
                        numberReviewsByReviewer[review.Reviewer] += 1;
                    }
                    else
                    {
                        numberReviewsByReviewer[review.Reviewer] = 1;
                    }
                }
            }

            return numberReviewsByReviewer;
        }

        // Abstraction & Polymorphism
        public override string Describe()
        {
            return string.Format("Season {0} Episode {1} - {2}", Season, EpisodeNumber, EpisodeName);
        }
    }
}
