using ConferenceScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceScheduler.Builders
{
    public class SessionBuilder
    {
        private readonly Session _session = new Session();
        private readonly List<SessionBuilder> _dependentSessions = new List<SessionBuilder>();

        public Session Build()
        {
            if ((_session.Id == 0) || (_dependentSessions.Any()))
                throw new InvalidOperationException("Unable to determine Id");

            return this.Build(0);
        }

        public Session Build(int nextId)
        {
            int i = nextId;

            if (_session.Id == 0)
            {
                _session.Id = i;
                i++;
            }

            if (_session.Presenters == null)
                _session.Presenters = new Presenter[] { };

            foreach (var dependentSession in _dependentSessions)
            {
                var s = dependentSession.Build(i);
                if (s.Id == i)
                    i++;
                s.AddDependency(_session);
            }

            return _session;
        }

        public SessionBuilder Id(int id)
        {
            _session.Id = id;
            return this;
        }

        public SessionBuilder Name(string name)
        {
            _session.Name = name;
            return this;
        }

        public SessionBuilder TopicId(int topicId)
        {
            _session.TopicId = topicId;
            return this;
        }

        public SessionBuilder AddPresenter(Presenter presenter)
        {
            _session.Presenters = 
                _session.Presenters == null ? 
                    (new Presenter[] { presenter }) : 
                    (IEnumerable<Presenter>)new List<Presenter>(_session.Presenters) { presenter };

            return this;
        }

        public SessionBuilder AddPresenter(PresenterBuilder builder)
        {
            // Note: There is currently no benefit from
            // delaying the Build here. If there becomes one later,
            // simply add these to a collection and then execute them
            // in the Session.Build().
            return this.AddPresenter(builder.Build());
        }

        public SessionBuilder AddDependentSession(SessionBuilder dependentSession)
        {
            _dependentSessions.Add(dependentSession);
            return this;
        }

    }
}
