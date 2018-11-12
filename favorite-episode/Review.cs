using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FavoriteEpisode
{
    // Encapsulation & Inheritance (Implement interface)
    public class Review : IOpinion
    {
        public string Reviewer { get; set; }
        public DateTime DateTime { get; set; }
        public string ReviewText { get; set; }

        // Polymorphism
        public override string ToString()
        {
            return string.Format("{1}: {0} said '{2}'", Reviewer, DateTime, ReviewText);
        }

        // Shout a review by writing it in all caps
        public string Shout()
        {
            if (Reviewer == null || ReviewText == null)
            {
                return null;
            }
            else
            {
                return string.Format("{1}: {0} said '{2}'", Reviewer.ToUpper(), DateTime, ReviewText.ToUpper());
            }
        }
    }
}
