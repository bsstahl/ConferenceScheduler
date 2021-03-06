﻿using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConferenceScheduler.Optimizer;
using ConferenceScheduler.Entities;
using ConferenceScheduler.Interfaces;
using TestHelperExtensions;

namespace ConferenceScheduler.Optimizer.Test
{
    public class Engine_Process_ShouldRespectPresentersAvailabilityBy
    {
        [Fact]
        public void ThrowingNoFeasibleSolutionIfSpeakerIsUnavailableForAllTimeslots()
        {
            var sessions = new SessionsCollection();
            sessions.Add(1, null, Presenter.Create(1, new int[] { 1 }));

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(new Timeslot() { Id = 1 });

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<Exceptions.NoFeasibleSolutionsException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ReturningTheCorrectAssignmentIfOneSpeakerIsAvailableForOnlyOneSlot()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();
            sessions.Add(1, null, Presenter.Create(1));
            sessions.Add(2, null, Presenter.Create(2, new int[] { 2, 3 })); // Only available for slot 1
            sessions.Add(3, null, Presenter.Create(3));

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));
            timeslots.Add(Timeslot.Create(3));

            var assignments = engine.Process(sessions, rooms, timeslots);
            var checkAssignment = assignments.Where(a => a.SessionId == 2).Single();

            assignments.WriteSchedule();

            // Session 2 should have been assigned to slot 1
            Assert.Equal(1, checkAssignment.TimeslotId);
        }

        [Fact]
        public void ReturningTheCorrectAssignmentIfTwoSpeakersAreAvailableForTwoOfTheThreeSlots()
        {
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();
            sessions.Add(1, null, Presenter.Create(1, new int[] { 2 }));    // Not available for slot 2
            sessions.Add(2, null, Presenter.Create(2, new int[] { 2 }));    // Not available for slot 2
            sessions.Add(3, null, Presenter.Create(3));       // Available for all but must be assigned to slot 2

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));
            timeslots.Add(Timeslot.Create(3));

            var assignments = engine.Process(sessions, rooms, timeslots);
            var checkAssignment = assignments.Where(a => a.SessionId == 3).Single();

            assignments.WriteSchedule();

            // Session 3 should have been assigned to slot 2
            Assert.Equal(2, checkAssignment.TimeslotId);
        }

        [Fact]
        public void NotFailIfTwoUnnamedSessionsHaveTheSameTimeslotUnavailability()
        {
            // This test exposes a bug that existed in a version
            // of the tool in early Oct 2018 where failing to add
            // a name caused a collision in naming constraints
            // if the same Timeslot is unavailable for multiple sessions
            var engine = (null as IConferenceOptimizer).Create();

            var sessions = new SessionsCollection();
            sessions.Add(1, null, Presenter.Create(1, new int[] { 2 }));
            sessions.Add(2, null, Presenter.Create(2, new int[] { 2 }));

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));
            timeslots.Add(Timeslot.Create(3));

            var assignments = engine.Process(sessions, rooms, timeslots);

            assignments.WriteSchedule();
        }

        [Fact]
        public void ThrowingNoFeasibleSolutionIfSpeakerWouldHaveToBeInTwoPlacesAtOnceDueTo2Sessions1Speaker1Timeslot()
        {
            var sessions = new SessionsCollection();
            sessions.Add(1, null, Presenter.Create(1));
            sessions.Add(2, null, Presenter.Create(1));

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));
            rooms.Add(Room.Create(2, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(new Timeslot() { Id = 1 });

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<Exceptions.NoFeasibleSolutionsException>(() => engine.Process(sessions, rooms, timeslots));
        }

        [Fact]
        public void ThrowingNoFeasibleSolutionIfSpeakerWouldHaveToBeInTwoPlacesAtOnce3SessionsFor1SpeakerWith2Timeslots()
        {
            var speaker1 = Presenter.Create(1);
            var speaker2 = Presenter.Create(2);

            var sessions = new SessionsCollection();
            sessions.Add(1, null, speaker1);
            sessions.Add(2, null, speaker1);
            sessions.Add(3, null, speaker2);
            sessions.Add(4, null, speaker1);

            var rooms = new List<Room>();
            rooms.Add(Room.Create(1, 10));
            rooms.Add(Room.Create(2, 10));

            var timeslots = new List<Timeslot>();
            timeslots.Add(Timeslot.Create(1));
            timeslots.Add(Timeslot.Create(2));

            var engine = (null as IConferenceOptimizer).Create();

            Assert.Throws<Exceptions.NoFeasibleSolutionsException>(() => engine.Process(sessions, rooms, timeslots));
        }
    }
}
