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
    public class Engine_Process_ShouldRespectPresentersPreferencesBy
    {
        [Fact]
        public void SchedulingThePresenterInAPreferredTimeslotIfPossible_1Presenter3Slots1Presentation()
        {
            int expectedTimeslot = 2;
            var preferredTimeslotIds = new int[] { expectedTimeslot };
            var presenter = Presenter.Create(1, "Test Presenter", new int[] { }, preferredTimeslotIds);

            var sessions = new SessionsCollection();
            sessions.Add(1, null, presenter);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(new Timeslot() { Id = 1 });
            timeslots.Add(new Timeslot() { Id = 2 });
            timeslots.Add(new Timeslot() { Id = 3 });

            var engine = (null as IConferenceOptimizer).Create();
            var assignments = engine.Process(sessions, rooms, timeslots);

            Assert.Equal(expectedTimeslot, assignments.Single().TimeslotId);
        }

        [Fact]
        public void SchedulingThePresenterInAPreferredTimeslotIfPossible_2Presenter2Slots2Presentation()
        {
            int expectedTimeslot1 = 2;
            var preferredTimeslotIds1 = new int[] { expectedTimeslot1 };
            var presenter1 = Presenter.Create(1, "Test Presenter 1", new int[] { }, preferredTimeslotIds1);

            int expectedTimeslot2 = 1;
            var preferredTimeslotIds2 = new int[] { expectedTimeslot2 };
            var presenter2 = Presenter.Create(2, "Test Presenter 2", new int[] { }, preferredTimeslotIds2);

            var sessions = new SessionsCollection();
            sessions.Add(1, null, presenter1);
            sessions.Add(2, null, presenter2);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(new Timeslot() { Id = 1 });
            timeslots.Add(new Timeslot() { Id = 2 });

            var engine = (null as IConferenceOptimizer).Create();
            var assignments = engine.Process(sessions, rooms, timeslots);

            Assert.Equal(expectedTimeslot2, assignments.Single(a => a.SessionId == 2).TimeslotId);
            Assert.Equal(expectedTimeslot1, assignments.Single(a => a.SessionId == 1).TimeslotId);
        }


        [Fact]
        public void SchedulingThePresenterInAPreferredTimeslotIfPossible_3Presenter3Slots3Presentation_2SpeakersPreferTheSameSlot()
        {
            int expectedTimeslot1 = 2;
            var preferredTimeslotIds1 = new int[] { expectedTimeslot1 };
            var presenter1 = Presenter.Create(1, "Test Presenter 1", new int[] { }, preferredTimeslotIds1);

            int expectedTimeslot2 = 1;
            var preferredTimeslotIds2 = new int[] { expectedTimeslot2 };
            var presenter2 = Presenter.Create(2, "Test Presenter 2", new int[] { }, preferredTimeslotIds2);

            int expectedTimeslot3 = 2;
            var preferredTimeslotIds3 = new int[] { expectedTimeslot3 };
            var presenter3 = Presenter.Create(3, "Test Presenter 3", new int[] { }, preferredTimeslotIds3);

            var sessions = new SessionsCollection();
            sessions.Add(1, null, presenter1);
            sessions.Add(2, null, presenter2);
            sessions.Add(3, null, presenter3);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(new Timeslot() { Id = 1 });
            timeslots.Add(new Timeslot() { Id = 2 });
            timeslots.Add(new Timeslot() { Id = 3 });

            var engine = (null as IConferenceOptimizer).Create();
            var assignments = engine.Process(sessions, rooms, timeslots);

            Assert.Equal(expectedTimeslot2, assignments.Single(a => a.SessionId == 2).TimeslotId);
        }


    }
}
