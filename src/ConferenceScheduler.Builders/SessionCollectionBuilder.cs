using ConferenceScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceScheduler.Builders
{
    public class SessionCollectionBuilder
    {
        private readonly List<Session> _sessions = new List<Session>();
        private readonly List<SessionBuilder> _sessionBuilders = new List<SessionBuilder>();
        private readonly List<SessionCollectionBuilder> _sessionCollectionBuilders = new List<SessionCollectionBuilder>();

        private readonly List<(Guid SessionId, Guid DependentSessionId)> _sessionBuilderDependencies = new List<(Guid, Guid)>();
        private readonly List<(Guid SessionId, Guid DependentSessionCollectionID)> _sessionCollectionBuilderDependencies = new List<(Guid, Guid)>();
        private readonly List<(Guid SessionId, Guid DependentSessionId)> _sessionDependencies = new List<(Guid SessionId, Guid DependentSessionId)>();

        private readonly Dictionary<Guid, Guid> _sessionBuilderSessionXref = new Dictionary<Guid, Guid>();

        public Guid BuilderId { get; private set; } = Guid.NewGuid();

        public IEnumerable<Session> Build()
        {
            return this.Build(1);
        }

        public IEnumerable<Session> Build(int startingId)
        {
            var results = new List<Session>(_sessions);
            int i = startingId;
            foreach (var session in results)
            {
                if (session.Id == 0)
                {
                    session.Id = i;
                    i++;
                }
            }

            foreach (var sessionBuilder in _sessionBuilders)
            {
                var session = sessionBuilder.Build(i);
                results.Add(session);

                _sessionBuilderSessionXref.Add(sessionBuilder.BuilderId, session.Uid);
                if (session.Id == i) i++;
            }

            foreach (var sessionCollectionBuilder in _sessionCollectionBuilders)
            {
                var (primarySessionBuilderUid, _) = _sessionCollectionBuilderDependencies
                    .Single(d => d.DependentSessionCollectionID == sessionCollectionBuilder.BuilderId);

                var previousSessionUid = _sessionBuilderSessionXref
                    .Single(x => x.Key == primarySessionBuilderUid).Value;

                var sessions = sessionCollectionBuilder.Build(i);
                foreach (var session in sessions)
                {
                    results.Add(session);
                    if (session.Id == i) i++;
                    _sessionDependencies.Add((previousSessionUid, session.Uid));
                    previousSessionUid = session.Uid;
                }
            }

            foreach (var (primaryBuilder, dependentBuilder) in _sessionBuilderDependencies)
            {
                var primarySessionId = _sessionBuilderSessionXref
                    .Single(x => x.Key == primaryBuilder).Value;

                var dependentSessionId = _sessionBuilderSessionXref
                    .Single(x => x.Key == dependentBuilder).Value;

                _sessionDependencies.Add((primarySessionId, dependentSessionId));
            }

            foreach (var (sessionId, dependentSessionId) in _sessionDependencies)
            {
                var session = results.Single(s => s.Uid == sessionId);
                var dependentSession = results.Single(s => s.Uid == dependentSessionId);

                dependentSession.AddDependency(session);
            }

            return results;
        }

        public SessionCollectionBuilder AddSession(Session session)
        {
            _sessions.Add(session);
            return this;
        }

        public SessionCollectionBuilder AddSession(SessionBuilder builder)
        {
            _sessionBuilders.Add(builder);
            return this;
        }

        public SessionCollectionBuilder AddSessionWithDependency(SessionBuilder session, SessionBuilder dependentSession)
        {
            _sessionBuilders.Add(session);
            _sessionBuilders.Add(dependentSession);
            _sessionBuilderDependencies.Add((session.BuilderId, dependentSession.BuilderId));
            return this;
        }

        public SessionCollectionBuilder AddSessionWithDependency(SessionBuilder session, SessionCollectionBuilder dependentSessionCollection)
        {
            _sessionBuilders.Add(session);
            _sessionCollectionBuilders.Add(dependentSessionCollection);
            _sessionCollectionBuilderDependencies.Add((session.BuilderId, dependentSessionCollection.BuilderId));
            return this;
        }
    }
}
