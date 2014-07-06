﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConferenceScheduler.Entities;

namespace ConferenceScheduler.Optimizer
{
    internal class SessionAvailabilityCollection : List<SessionAvailability>
    {
        internal SessionAvailabilityCollection(IEnumerable<Entities.Session> sessions, IEnumerable<Entities.Room> rooms, IEnumerable<Entities.Timeslot> timeslots)
        {
            Load(sessions, rooms, timeslots);
        }

        internal bool IsFeasible
        {
            get
            {
                return (this.Where(a => a.SessionIds.Count() == 0).Count() == 0);
            }
        }

        private void Load(IEnumerable<Entities.Session> sessions, IEnumerable<Entities.Room> rooms, IEnumerable<Entities.Timeslot> timeslots)
        {
            foreach (var room in rooms)
                foreach (var timeslot in timeslots)
                    this.Add(new SessionAvailability(timeslot.Id, room.Id, sessions));
        }

        internal IEnumerable<SessionAvailability> GetUnassignedItemsWithOnlyOneOption()
        {
            return this.Where(sa => !sa.Assigned && sa.SessionIds.Count() == 1);
        }

        internal void Assign(Assignment assignment)
        {
            var items = this.Where(i => i.SessionIds.Contains(assignment.SessionId.Value));
            foreach (var item in items)
            {
                item.SessionIds.Remove(assignment.SessionId.Value);
                if (item.RoomId == assignment.RoomId && item.TimeslotId == assignment.TimeslotId)
                    item.Assigned = true;
            }  
        }
    }
}
