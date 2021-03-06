﻿using ConferenceScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceScheduler.Builders
{
    public class PresenterBuilder : Presenter
    {
        bool _autoId = true;

        readonly List<int> _unavailableForTimeslots = new List<int>();
        readonly List<int> _preferredTimeslots = new List<int>();

        public Presenter Build()
        {
            if (_autoId)
                throw new InvalidOperationException("A nextId value is required if automatic ID is needed");
            this.UnavailableForTimeslots = _unavailableForTimeslots;
            this.PreferredTimeslots = _preferredTimeslots;
            return this;
        }

        public Presenter Build(int nextId)
        {
            if (_autoId)
            {
                base.Id = nextId;
                _autoId = false;
            }
            return this.Build();
        }

        public new PresenterBuilder Id(int id)
        {
            _autoId = false;
            base.Id = id;
            return this;
        }

        public new PresenterBuilder Name(string name)
        {
            base.Name = name;
            return this;
        }

        public PresenterBuilder AddTimeslotUnavailable(int timeslotId)
        {
            _unavailableForTimeslots.Add(timeslotId);
            return this;
        }

        public PresenterBuilder AddPreferredTimeslot(int timeslotId)
        {
            _preferredTimeslots.Add(timeslotId);
            return this;
        }

    }
}
