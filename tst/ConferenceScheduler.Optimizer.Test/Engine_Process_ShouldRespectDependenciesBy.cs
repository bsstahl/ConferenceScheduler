using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConferenceScheduler.Optimizer;
using ConferenceScheduler.Entities;
using ConferenceScheduler.Interfaces;

namespace ConferenceScheduler.Optimizer.Test
{
    public class Engine_Process_ShouldRespectDependenciesBy
    {
        [Fact]
        public void ReturningTheOnlyPossibleAssignmentIfTheFirstSessionIsDependentOnTheSecond()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();
            var session1 = sessions.Add(1, null, Presenter.Create(1));
            var session2 = sessions.Add(2, null, Presenter.Create(1));
            session1.AddDependency(session2);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1, 10));
            timeslots.Add(Timeslot.Create(2, 11));

            var assignments = engine.Process(sessions, rooms, timeslots);
            var session1Assignment = assignments.Where(a => a.SessionId.Value == 1).Single();
            assignments.WriteSchedule();

            // Session 1 must be assigned to timeslot 2 to satisfy the dependencies
            Assert.Equal(2, session1Assignment.TimeslotId); 
        }

        [Fact]
        public void ReturningTheOnlyPossibleAssignmentIfTheSecondSessionIsDependentOnTheFirst()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();
            var session1 = sessions.Add(1, null, Presenter.Create(1));
            var session2 = sessions.Add(2, null, Presenter.Create(1));
            session2.AddDependency(session1);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1, 10));
            timeslots.Add(Timeslot.Create(2, 11));

            var assignments = engine.Process(sessions, rooms, timeslots);
            var session2Assignment = assignments.Where(a => a.SessionId.Value == 2).Single();
            assignments.WriteSchedule();

            // Session 2 must be assigned to timeslot 2 to satisfy the dependencies
            Assert.Equal(2, session2Assignment.TimeslotId);
        }

        [Fact]
        public void FindingTheOnlyValidTimeslotForASessionWithChainedDependencies()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();

            var session1 = sessions.Add(1, null, Presenter.Create(1));
            var session2 = sessions.Add(2, null, Presenter.Create(3));
            var session3 = sessions.Add(3, null, Presenter.Create(1));
            var session4 = sessions.Add(4, null, Presenter.Create(2));
            var session5 = sessions.Add(5, null, Presenter.Create(2));
            var session6 = sessions.Add(6, null, Presenter.Create(2));

            session5.AddDependency(session6);
            session4.AddDependency(session5);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1));
            rooms.Add(Room.Create(2));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1, 8.5));
            timeslots.Add(Timeslot.Create(2, 9.75));
            timeslots.Add(Timeslot.Create(3, 11.0));

            var assignments = engine.Process(sessions, rooms, timeslots);
            assignments.WriteSchedule();
            var testAssignment = assignments.Single(a => a.SessionId == 4);

            // Session 4 must be in the 3rd timeslot
            Assert.Equal(3, testAssignment.TimeslotId);
        }

        [Fact]
        public void FindingTheOnlyValidTimeslotForASessionWithMultipleDependencies()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();

            var session1 = sessions.Add(1, null, Presenter.Create(1));
            var session2 = sessions.Add(2, null, Presenter.Create(3));
            var session3 = sessions.Add(3, null, Presenter.Create(1));
            var session4 = sessions.Add(4, null, Presenter.Create(2));
            var session5 = sessions.Add(5, null, Presenter.Create(2));
            var session6 = sessions.Add(6, null, Presenter.Create(2));

            session4.AddDependency(session6);
            session4.AddDependency(session5);
            session4.AddDependency(session1);
            session4.AddDependency(session2);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1));
            rooms.Add(Room.Create(2));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1, 8.5));
            timeslots.Add(Timeslot.Create(2, 9.75));
            timeslots.Add(Timeslot.Create(3, 11.0));

            var assignments = engine.Process(sessions, rooms, timeslots);
            assignments.WriteSchedule();
            var testAssignment = assignments.Single(a => a.SessionId == 4);

            // Session 4 must be in the 3rd timeslot
            Assert.Equal(3, testAssignment.TimeslotId);
        }

        [Fact]
        public void FindingTheOnlyValidTimeslotForASessionWithMoreDependenciesThenRooms()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();

            var session1 = sessions.Add(1, null, Presenter.Create(1));
            var session2 = sessions.Add(2, null, Presenter.Create(3));
            var session3 = sessions.Add(3, null, Presenter.Create(1));
            var session4 = sessions.Add(4, null, Presenter.Create(2));
            var session5 = sessions.Add(5, null, Presenter.Create(2));
            var session6 = sessions.Add(6, null, Presenter.Create(2));

            session4.AddDependency(session6);
            session4.AddDependency(session5);
            session4.AddDependency(session1);
            session4.AddDependency(session2);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1));
            rooms.Add(Room.Create(2));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1, 8.5));
            timeslots.Add(Timeslot.Create(2, 9.75));
            timeslots.Add(Timeslot.Create(3, 11.0));

            var assignments = engine.Process(sessions, rooms, timeslots);
            assignments.WriteSchedule();
            var testAssignment = assignments.Single(a => a.SessionId == 4);

            // Session 4 must be in the 3rd timeslot
            Assert.Equal(3, testAssignment.TimeslotId);
        }

        [Fact]
        public void AssigningAllDependenciesOfASessionPriorToThatSession()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();

            var session1 = sessions.Add(1, null, Presenter.Create(1));
            var session2 = sessions.Add(2, null, Presenter.Create(3));
            var session3 = sessions.Add(3, null, Presenter.Create(1));
            var session4 = sessions.Add(4, null, Presenter.Create(2));
            var session5 = sessions.Add(5, null, Presenter.Create(2));
            var session6 = sessions.Add(6, null, Presenter.Create(2));

            session4.AddDependency(session6);
            session4.AddDependency(session5);
            session4.AddDependency(session1);
            session4.AddDependency(session2);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1));
            rooms.Add(Room.Create(2));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1, 8.5));
            timeslots.Add(Timeslot.Create(2, 9.75));
            timeslots.Add(Timeslot.Create(3, 11.0));

            var assignments = engine.Process(sessions, rooms, timeslots);
            assignments.WriteSchedule();

            var targetTimeslotId = assignments.Single(a => a.SessionId == 4).TimeslotId;
            var dependentSessionIds = new List<int>() { 1, 2, 5, 6 };
            var maxTimeslotId = assignments.Where(a => dependentSessionIds.Contains(a.SessionId.Value)).Max(a => a.TimeslotId);

            Assert.True(maxTimeslotId < targetTimeslotId, "All dependent sessions must assigned earlier than the dependency session");
        }

    }
}
