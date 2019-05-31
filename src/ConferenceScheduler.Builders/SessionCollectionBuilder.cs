using ConferenceScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceScheduler.Builders
{
    public class SessionCollectionBuilder
    {
        private readonly List<Session> _sessions = new List<Session>();
        private readonly List<SessionBuilder> _sessionBuilders = new List<SessionBuilder>();

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
                if (session.Id == i) i++;
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
    }
}
