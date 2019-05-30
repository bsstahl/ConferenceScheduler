using ConferenceScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceScheduler.Builders
{
    public class SessionBuilder: Session
    {
        public Session Build()
        {
            if (base.Id == 0)
                throw new InvalidOperationException("Unable to determine Id");

            return this;
        }

        public Session Build(int nextId)
        {
            if (base.Id == 0)
                base.Id = nextId;
            return this;
        }

        public new SessionBuilder Id(int id)
        {
            base.Id = id;
            return this;
        }
    }
}
