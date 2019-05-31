using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using ConferenceScheduler.Entities;

namespace ConferenceScheduler.Builders.Test
{
    public class SessionCollectionBuilder_Build_Should
    {
        [Fact]
        public void ReturnAnEmptySessionCollectionIfNoSessionsAdded()
        {
            var actual = new SessionCollectionBuilder()
                .Build();

            Assert.NotNull(actual);
        }

        [Fact]
        public void ReturnASingleSessionIfOneSessionAdded()
        {
            var expected = new Session();

            var actual = new SessionCollectionBuilder()
                .AddSession(expected)
                .Build();

            Assert.Single(actual);
        }

        [Fact]
        public void ReturnTheCorrectSessionIfOneSessionAdded()
        {
            var expected = new Session() { Id = Int32.MaxValue.GetRandom() };

            var actual = new SessionCollectionBuilder()
                .AddSession(expected)
                .Build();

            Assert.Equal(expected.Id, actual.Single().Id);
        }

        [Fact]
        public void ReturnAPositiveSessionIdIfNoIdProvided()
        {
            var expected = new Session();

            var actual = new SessionCollectionBuilder()
                .AddSession(expected)
                .Build();

            Assert.True(actual.Single().Id > 0);
        }

        [Fact]
        public void ReturnTheStartingIdIfNoSessionIdIsSupplied()
        {
            int expectedId = Int32.MaxValue.GetRandom();
            var expectedSession = new Session();

            var actual = new SessionCollectionBuilder()
                .AddSession(expectedSession)
                .Build(expectedId);

            Assert.Equal(expectedId, actual.Single().Id);
        }

        [Fact]
        public void ReturnTheSpecifiedSessionIdIfBothIDsAreSpecified()
        {
            int startingId = Int32.MaxValue.GetRandom();
            int expectedId = Int32.MaxValue.GetRandom();
            var expectedSession = new Session() { Id = expectedId };

            var actual = new SessionCollectionBuilder()
                .AddSession(expectedSession)
                .Build(startingId);

            Assert.Equal(expectedId, actual.Single().Id);
        }

        [Fact]
        public void ReturnTheSpecifiedSessionIdForTheSessionWhenOnlySomeSessionsHaveIds()
        {
            int startingId = Int32.MaxValue.GetRandom();
            string nonspecificIdSessionName = string.Empty.GetRandom();

            int specificId = Int32.MaxValue.GetRandom();
            string specificIdSessionName = string.Empty.GetRandom();

            var expectedSession = new Session() { Id = specificId, Name = specificIdSessionName };
            var otherSession = new Session() { Name = nonspecificIdSessionName };

            var actual = new SessionCollectionBuilder()
                .AddSession(expectedSession)
                .AddSession(otherSession)
                .Build(startingId);

            var actualSession = actual.Single(s => s.Id == specificId);
            Assert.Equal(specificIdSessionName, actualSession.Name);
        }

        [Fact]
        public void ReturnTheStartingIdForTheSessionWhenOnlySomeSessionsHaveIds()
        {
            int startingId = Int32.MaxValue.GetRandom();
            string nonspecificIdSessionName = string.Empty.GetRandom();

            int specificId = Int32.MaxValue.GetRandom();
            string specificIdSessionName = string.Empty.GetRandom();

            var expectedSession = new Session() { Id = specificId, Name = specificIdSessionName };
            var otherSession = new Session() { Name = nonspecificIdSessionName };

            var actual = new SessionCollectionBuilder()
                .AddSession(expectedSession)
                .AddSession(otherSession)
                .Build(startingId);

            var actualSession = actual.Single(s => s.Id == startingId);
            Assert.Equal(nonspecificIdSessionName, actualSession.Name);
        }
    }
}
