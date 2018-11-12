using System;
using System.Collections;
using System.Collections.Generic;

namespace FavoriteEpisode
{
    public class Reviews : IEnumerable<Review>
    {
        private List<Review> reviewList = new List<Review>();

        public Review this[int index]
        {
            get => reviewList[index];
            set => reviewList.Insert(index, value);
        }

        IEnumerator<Review> IEnumerable<Review>.GetEnumerator()
        {
            return reviewList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
