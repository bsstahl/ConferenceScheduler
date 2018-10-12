using System;
using System.Collections.Generic;
using System.Text;
using ConferenceScheduler.Entities;
using ConferenceScheduler.Exceptions;

namespace ConferenceScheduler.Builders
{
    public class TimeslotBuilder
    {
        private Timeslot _timeslot = new Timeslot();

        public TimeslotBuilder(int id)
        {
            if (id == 0)
                throw new MissingIdentityException(typeof(Timeslot));

            _timeslot.Id = id;
        }

        public int TimeslotId
        {
            get { return _timeslot.Id; }
        }

        public Timeslot Build()
        {
            return _timeslot;
        }

        public TimeslotBuilder StartingAt(double timeOfDay)
        {
            _timeslot.StartHour = Convert.ToSingle(timeOfDay);
            return this;
        }
        
        public TimeslotBuilder OnDay(int dayIndex)
        {
            _timeslot.DayIndex = dayIndex;
            return this;
        }
    }
}
