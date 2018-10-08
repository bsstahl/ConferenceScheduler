using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConferenceScheduler.Optimizer;
using ConferenceScheduler.Entities;
using ConferenceScheduler.Interfaces;

namespace ConferenceScheduler.Optimizer.Test
{
    public class Engine_Process_ShouldRespectVenueNeedsBy
    {
        [Fact]
        public void NotAssigningASessionToARoomWhenItIsNotAvailableInTimesdlot1()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();
            sessions.Add(1, null, Presenter.Create(1));
            sessions.Add(2, null, Presenter.Create(2));
            sessions.Add(3, null, Presenter.Create(3));

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));
            rooms.Add(Room.Create(2, 10, 1));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));

            var assignments = engine.Process(sessions, rooms, timeslots);
            var checkAssignment = assignments.Where(a => a.RoomId == 2 && a.TimeslotId == 1).SingleOrDefault();

            assignments.WriteSchedule();

            if (checkAssignment == null)
                Assert.Null(checkAssignment);
            else
                // No session should have been assigned to room 2 during timeslot 1
                Assert.Null(checkAssignment.SessionId);
        }

        [Fact]
        public void NotAssigningASessionToARoomWhenItIsNotAvailableInTimesdlot2()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();
            sessions.Add(1, null, Presenter.Create(1));
            sessions.Add(2, null, Presenter.Create(2));
            sessions.Add(3, null, Presenter.Create(3));

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));
            rooms.Add(Room.Create(2, 10, 2));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));

            var assignments = engine.Process(sessions, rooms, timeslots);
            var checkAssignment = assignments.Where(a => a.RoomId == 2 && a.TimeslotId == 2).SingleOrDefault();

            assignments.WriteSchedule();

            if (checkAssignment == null)
                Assert.Null(checkAssignment);
            else
                // No session should have been assigned to room 2 during timeslot 2
                Assert.Null(checkAssignment.SessionId);
        }

    }
}
