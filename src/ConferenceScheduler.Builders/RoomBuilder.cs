using ConferenceScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConferenceScheduler.Builders
{
    public class RoomBuilder : Room
    {
        bool _autoId = true;
        List<int> _unavailableForTimeslots = new List<int>();

        public bool HasSpecifiedId()
        {
            return !base.Id.Equals(0);
        }

        public int GetSpecifiedId()
        {
            return base.Id;
        }

        public Room Build()
        {
            if (_autoId)
                throw new InvalidOperationException("A nextId value is required if automatic ID is needed");
            this.UnavailableForTimeslots = _unavailableForTimeslots;
            return this;
        }

        public Room Build(int nextId)
        {
            if (_autoId)
            {
                base.Id = nextId;
                _autoId = false;
            }
            return this.Build();
        }

        public new RoomBuilder Capacity(int capacity)
        {
            base.Capacity = capacity;
            return this;
        }

        public new RoomBuilder Id(int id)
        {
            _autoId = false;
            base.Id = id;
            return this;
        }

        public RoomBuilder AddTimeslotUnavailable(int timeslotId)
        {
            _unavailableForTimeslots.Add(timeslotId);
            return this;
        }

    }
}
