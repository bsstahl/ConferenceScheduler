using ConferenceScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceScheduler.Builders
{
    public class SessionBuilder
    {
        private readonly Session _session = new Session();

        public Session Build()
        {
            if (_session.Id == 0)
                throw new InvalidOperationException("Unable to determine Id");

            if (_session.Presenters == null)
                _session.Presenters = new Presenter[] { };

            return _session;
        }

        public Session Build(int nextId)
        {
            if (_session.Id == 0)
                _session.Id = nextId;
            return this.Build();
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

    }
}
