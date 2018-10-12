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

    public class Episode
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
        //public int Rating { get; set; }
        public List<string> Reviews = new List<string>();

        //public void RateEpisode(int rating)
        //{
        //    Rating = rating;
        //}

        public void ReviewEpisode(string review)
        {
            Reviews.Add(review);
        }
    }
}
