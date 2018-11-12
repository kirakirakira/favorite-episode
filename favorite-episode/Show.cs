using System;

namespace FavoriteEpisode
{
    // Abstraction
    public abstract class Show
    {
        public virtual string Describe()
        {
            return "This is a generic show";
        }
    }
}
