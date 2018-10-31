using ConferenceScheduler.Entities;
using ConferenceScheduler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceScheduler.Builders
{
    public class TimeslotCollectionBuilder
    {
        List<Timeslot> _timeslots = new List<Timeslot>();
        List<TimeslotBuilder> _builders = new List<TimeslotBuilder>();

        public IEnumerable<Timeslot> Build()
        {
            var result = new List<Timeslot>();
            foreach (var timeslot in _timeslots)
                result.Add(timeslot);
            foreach (var builder in _builders)
                result.Add(builder.Build());
            return result;
        }

        public TimeslotCollectionBuilder Add(Timeslot slot)
        {
            if (slot.Id == 0)
                throw new MissingIdentityException(typeof(Timeslot));

            if (this.Build().Select(t => t.Id).Contains(slot.Id))
                throw new DuplicateEntityException(typeof(Timeslot), slot.Id);

            _timeslots.Add(slot);
            return this;
        }

        public TimeslotCollectionBuilder Add(TimeslotBuilder builder)
        {
            if (this.Build().Select(t => t.Id).Contains(builder.TimeslotId))
                throw new DuplicateEntityException(typeof(Timeslot), builder.TimeslotId);
            _builders.Add(builder);
            return this;
        }

    }
}
