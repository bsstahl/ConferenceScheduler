using ConferenceScheduler.Entities;
using ConferenceScheduler.Exceptions;
using ConferenceScheduler.Extensions;
using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceScheduler.Optimizer.Glop
{
    public class Engine : ConferenceScheduler.Interfaces.IConferenceOptimizer
    {
        Action<ProcessUpdateEventArgs> _updateEventHandler;

        // v holds a boolean indicator for each room/timeslot/session combination.
        // r holds the room number of the session

        #region Legacy Code
        // Legacy - May be reimplemented later
        // Variable[,] _t; // holds the topicId of each room/timeslot combination
        // Variable[] _s;
        #endregion

        /// <summary>
        /// Create an instance of the object
        /// </summary>
        /// <param name="updateEventHandler">A method to call to handle an update event.</param>
        public Engine(Action<ProcessUpdateEventArgs> updateEventHandler)
        {
            _updateEventHandler = updateEventHandler;
        }

        public IEnumerable<Assignment> Process(IEnumerable<Session> sessions, IEnumerable<Room> rooms, IEnumerable<Timeslot> timeslots)
        {
            Validate(sessions, rooms, timeslots);

            var model = (null as Solver).CreateMixedIntegerProgrammingSolver();

            var v = model.CreateAssignmentVariables(sessions.Count(), rooms.Count(), timeslots.Count());
            var r = model.CreateRoomVariables(sessions.Count(), rooms.Count());

            var timeslotIds = timeslots.GetIdCollection();
            model.CreateConstraints(v, r, sessions, rooms, timeslotIds);
            model.CreateObjective(v, sessions, rooms, timeslotIds);

            int status = model.Solve();
            if (status != Solver.OPTIMAL)
                throw new NoFeasibleSolutionsException();

            return (null as IEnumerable<Assignment>).CreateAssignments(v, rooms.GetIdCollection(), timeslotIds, sessions.GetIdCollection());

            #region Scar Tissue

            // var v = _model.Get(GRB.DoubleAttr.X, _v);
            //var p = _model.Get(GRB.DoubleAttr.X, _s);

            //for (int i = 0; i < sessions.Count(); i++)
            //    Console.WriteLine($"s[{i}] = {p[i]}");

            //var roomValues = new int[sessions.Count()];
            //var rv = r.Select(rm => rm.SolutionValue()).ToArray();

            //_t = new Variable[roomCount, timeslotCount];
            //for (int r = 0; r < roomCount; r++)
            //    for (int t = 0; t < timeslotCount; t++)
            //    {
            //        _t[r, t] = _model.MakeIntVar(0, 999, $"y[{r},{t}]");
            //        Console.WriteLine($"Variable: y[{r},{t}]");
            //    }


            //_s = new Variable[sessionCount];
            //for (int s = 0; s < sessionCount; s++)
            //{
            //    _s[s] = _model.MakeIntVar(0.0, Convert.ToDouble(timeslotCount), $"s[{s}]");
            //    Console.WriteLine($"Variable: s[{s}]");
            //}

            #endregion
        }

        private static void Validate(IEnumerable<Session> sessions, IEnumerable<Room> rooms, IEnumerable<Timeslot> timeslots)
        {
            rooms.Validate();
            timeslots.Validate();
            sessions.Validate();

            sessions.ValidateAgainstRoomsAndTimeslots(rooms, timeslots);
        }

    }
}
