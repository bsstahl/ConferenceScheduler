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
    public class Engine_Process_ShouldValidateBy
    {

        [Fact]
        public void ThrowingNoFeasibleSolutionsExceptionIfNoSessionsSupplied()
        {
            var sessions = new SessionsCollection();

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<ArgumentException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingArgumentNullExceptionIfSessionsIsNull()
        {
            IEnumerable<Session> sessions = null;

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<ArgumentNullException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingArgumentExceptionIfNoRoomsSupplied()
        {
            var sessions = new SessionsCollection();
            sessions.Add(1, 1);

            var rooms = new List<Room>();

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<ArgumentException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingArgumentNullExceptionIfRoomsIsNull()
        {
            var sessions = new SessionsCollection();
            sessions.Add(1, 1, Presenter.Create(1));

            IEnumerable<Room> rooms = null;

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<ArgumentNullException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingArgumentExceptionIfNoTimeslotsSupplied()
        {
            var sessions = new SessionsCollection();
            sessions.Add(1, 1);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<ArgumentException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingArgumentNullExceptionIfTimeslotsIsNull()
        {
            var sessions = new SessionsCollection();
            sessions.Add(1, 1, Presenter.Create(1));

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            IEnumerable<Timeslot> timeslots = null;

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<ArgumentNullException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingArgumentExceptionIfThereAreMoreSessionsThanSlotsAndRooms()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();
            sessions.Add(1, null, Presenter.Create(1));
            sessions.Add(2, null, Presenter.Create(2));

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));

            Assert.Throws<ArgumentException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingArgumentExceptionIfThereAreMoreSessionsThanRoomSlotCombinations()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();
            sessions.Add(1, null, Presenter.Create(1));
            sessions.Add(2, null, Presenter.Create(2));
            sessions.Add(3, null, Presenter.Create(3));
            sessions.Add(4, null, Presenter.Create(4));

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));
            rooms.Add(Room.Create(2, 10, 2)); // Room is not available in timeslot 2

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));

            Assert.Throws<ArgumentException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingArgumentExceptionIfThereIsntAtLeastOnePresenterForEachSession()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();
            sessions.Add(1, 1);
            sessions.Add(new Session() { Id = 2 });

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));

            Assert.Throws<ArgumentException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingNoFeasibleSolutionIfAvailableTimeslotsForAMultiPresenterSessionDontIntersect()
        {
            // 2 presenters for one session where neither
            // is available to present when the other is available

            var presenter1 = Presenter.Create(1, new int[] { 2 });
            var presenter2 = Presenter.Create(2, new int[] { 1 });

            var sessions = new SessionsCollection();
            sessions.Add(1, null, presenter1, presenter2);

            var rooms = new List<Room>() { Room.Create(1, 10) };

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<Exceptions.NoFeasibleSolutionsException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingDependencyExceptionIfCircularDependenciesExist()
        {

            var presenter1 = Presenter.Create(1, new int[] { 2 });

            var sessions = new SessionsCollection();

            var session1 = sessions.Add(1, 1, Presenter.Create(1));
            var session2 = sessions.Add(2, 1, Presenter.Create(2));
            var session3 = sessions.Add(3, 1, Presenter.Create(3));

            session1.AddDependency(session2);
            session2.AddDependency(session3);
            session3.AddDependency(session1);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));
            rooms.Add(Room.Create(2, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));

            var engine = (null as IConferenceOptimizer).Create();
            Assert.Throws<Exceptions.DependencyException>(() => engine.Process(sessions, rooms, timeslots));

            Assert.Throws<Exceptions.DependencyException>(() => engine.Process(sessions, rooms, timeslots));
        }


        [Fact]
        public void ThrowingArgumentExceptionIfDuplicateSessionIdsExist()
        {
            var sessions = new SessionsCollection();

            var session1 = sessions.Add(1, 1, Presenter.Create(1));
            var session2 = sessions.Add(2, 1, Presenter.Create(2));
            var session3 = sessions.Add(1, 1, Presenter.Create(3));

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));
            rooms.Add(Room.Create(2, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<ArgumentException>(() => engine.Process(sessions, rooms, timeslots));

        }

        [Fact]
        public void ThrowingArgumentExceptionIfDuplicateRoomIdsExist()
        {
            var sessions = new SessionsCollection();

            var session1 = sessions.Add(1, 1, Presenter.Create(1));
            var session2 = sessions.Add(2, 1, Presenter.Create(2));
            var session3 = sessions.Add(3, 1, Presenter.Create(3));

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));
            rooms.Add(Room.Create(2, 10));
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<ArgumentException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingArgumentExceptionIfDuplicateTimeslotIdsExist()
        {
            var sessions = new SessionsCollection();

            var session1 = sessions.Add(1, 1, Presenter.Create(1));
            var session2 = sessions.Add(2, 1, Presenter.Create(2));
            var session3 = sessions.Add(3, 1, Presenter.Create(3));

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));
            rooms.Add(Room.Create(2, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));
            timeslots.Add(Timeslot.Create(1));

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<ArgumentException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingArgumentExceptionIfDuplicatePresenterIdsWithDifferentAvailabilitiesExist()
        {

            var presenter1 = Presenter.Create(1, new int[] { 2, 3 });
            var presenter2 = Presenter.Create(2, new int[] { 2 });
            var presenter3 = Presenter.Create(1, new int[] { 1, 2 });

            var sessions = new SessionsCollection();

            var session1 = sessions.Add(1, 1, presenter1);
            var session2 = sessions.Add(2, 1, presenter2);
            var session3 = sessions.Add(3, 1, presenter3);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));
            rooms.Add(Room.Create(2, 10));
            rooms.Add(Room.Create(3, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<ArgumentException>(() => engine.Process(sessions, rooms, timeslots));

        }

        [Fact]
        public void ThrowingArgumentExceptionIfDuplicatePresenterIdsWithDifferentAvailabilityCountsExist()
        {

            var presenter1 = Presenter.Create(1, new int[] { 2 });
            var presenter2 = Presenter.Create(2, new int[] { 2 });
            var presenter3 = Presenter.Create(1);

            var sessions = new SessionsCollection();

            var session1 = sessions.Add(1, 1, presenter1);
            var session2 = sessions.Add(2, 1, presenter2);
            var session3 = sessions.Add(3, 1, presenter3);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));
            rooms.Add(Room.Create(2, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<ArgumentException>(() => engine.Process(sessions, rooms, timeslots));

        }

        [Fact]
        public void NotThrowingIfDuplicatePresenterSessionsAllHaveSameAvailabilities()
        {

            var presenter1 = Presenter.Create(1, new int[] { 2, 3 });
            var presenter2 = Presenter.Create(2, new int[] { 2 });
            var presenter3 = Presenter.Create(3, new int[] { 1, 2 });

            var sessions = new SessionsCollection();

            var session1 = sessions.Add(1, 1, presenter1);
            var session2 = sessions.Add(2, 1, presenter2);
            var session3 = sessions.Add(3, 1, presenter3);

            var session4 = sessions.Add(4, 2, presenter1);
            var session5 = sessions.Add(5, 2, presenter2);
            var session6 = sessions.Add(6, 2, presenter3);

            var session7 = sessions.Add(7, 3, presenter1);
            var session8 = sessions.Add(8, 3, presenter2);
            var session9 = sessions.Add(9, 3, presenter3);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));
            rooms.Add(Room.Create(2, 10));
            rooms.Add(Room.Create(3, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));
            timeslots.Add(Timeslot.Create(3));
            timeslots.Add(Timeslot.Create(4));
            timeslots.Add(Timeslot.Create(5));
            timeslots.Add(Timeslot.Create(6));

            var engine = (null as IConferenceOptimizer).Create();
            // var result = engine.Process(sessions, rooms, timeslots);

            Assert.False(true, "Fix this test");
        }

    }
}
