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

        [Fact]
        public void ReturnTwoSessionsIfADependentSessionIsSpecified()
        {
            var actual = new SessionCollectionBuilder()
                .AddSessionWithDependency(new SessionBuilder(), new SessionBuilder())
                .Build();

            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public void ReturnASessionDependentOnAnotherSessionIfADependentSessionIsSpecified()
        {
            int sessionId = Int32.MaxValue.GetRandom();
            var session = new SessionBuilder()
                .Id(sessionId);

            int dependentSessionId = Int32.MaxValue.GetRandom();
            var dependentSession = new SessionBuilder()
                .Id(dependentSessionId);

            var actualSessions = new SessionCollectionBuilder()
                .AddSessionWithDependency(session, dependentSession)
                .Build();

            var actual = actualSessions.Single(s => s.Id == dependentSessionId);
            Assert.True(actual.IsDependentUpon(sessionId));
        }

        [Fact]
        public void ReturnTheCorrectNumberOfSessionsIfADependentSessionCollectionIsSpecified()
        {
            int primarySessionId = Int32.MaxValue.GetRandom();
            var primarySession = new SessionBuilder()
                .Id(primarySessionId);

            var collectionBuilder = new SessionCollectionBuilder()
                .AddSession(new SessionBuilder())
                .AddSession(new SessionBuilder());

            var actual = new SessionCollectionBuilder()
                .AddSessionWithDependency(primarySession, collectionBuilder)
                .Build();

            Assert.Equal(3, actual.Count());
        }

        [Fact]
        public void ReturnTheCorrectDependencyChainIfADependentSessionCollectionIsSpecified()
        {
            int primarySessionId = Int32.MaxValue.GetRandom();
            int secondarySessionId = Int32.MaxValue.GetRandom();
            int tertiarySessionId = Int32.MaxValue.GetRandom();

            var primarySessionBuilder = new SessionBuilder()
                .Id(primarySessionId);

            var collectionBuilder = new SessionCollectionBuilder()
                .AddSession(new SessionBuilder().Id(secondarySessionId))
                .AddSession(new SessionBuilder().Id(tertiarySessionId));

            var actualSessions = new SessionCollectionBuilder()
                .AddSessionWithDependency(primarySessionBuilder, collectionBuilder)
                .Build();

            var primarySession = actualSessions.Single(s => s.Id == primarySessionId);
            var secondarySession = actualSessions.Single(s => s.Id == secondarySessionId);
            var tertiarySession = actualSessions.Single(s => s.Id == tertiarySessionId);

            Assert.True(secondarySession.IsDependentUpon(primarySessionId));
            Assert.True(tertiarySession.IsDependentUpon(primarySessionId));
            Assert.True(tertiarySession.IsDependentUpon(secondarySessionId));
        }

        [Fact]
        public void ReturnTheCorrectDependencyChainIfNoDependenciesSpecified()
        {
            int primarySessionId = Int32.MaxValue.GetRandom();
            int secondarySessionId = Int32.MaxValue.GetRandom();
            int tertiarySessionId = Int32.MaxValue.GetRandom();

            var primarySessionBuilder = new SessionBuilder()
                .Id(primarySessionId);

            var secondarySessionBuilder = new SessionBuilder()
                .Id(secondarySessionId);

            var tertiarySessionBuilder = new SessionBuilder()
                .Id(tertiarySessionId);

            var actualSessions = new SessionCollectionBuilder()
                .AddSession(primarySessionBuilder)
                .AddSession(secondarySessionBuilder)
                .AddSession(tertiarySessionBuilder)
                .Build();

            var primarySession = actualSessions.Single(s => s.Id == primarySessionId);
            var secondarySession = actualSessions.Single(s => s.Id == secondarySessionId);
            var tertiarySession = actualSessions.Single(s => s.Id == tertiarySessionId);

            Assert.False(tertiarySession.IsDependentUpon(secondarySessionId));
            Assert.False(tertiarySession.IsDependentUpon(primarySessionId));

            Assert.False(secondarySession.IsDependentUpon(primarySessionId));
            Assert.False(secondarySession.IsDependentUpon(tertiarySessionId));

            Assert.False(primarySession.IsDependentUpon(secondarySessionId));
            Assert.False(primarySession.IsDependentUpon(tertiarySessionId));
        }
    }
}
