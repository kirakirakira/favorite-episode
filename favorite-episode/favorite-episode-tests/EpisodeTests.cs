using NUnit.Framework;
using System;
using FavoriteEpisode;

namespace favoriteepisodetests
{
    [TestFixture()]
    public class EpisodeTests
    {
        [Test()]
        public void ReviewEpisodeTest()
        {
            // Arrange
            Episode episode = new Episode();
            string review = "This is the test review";

            // Act
            episode.ReviewEpisode(review);

            // Assert
            Assert.Contains(review, episode.Reviews);
        }
    }
}
