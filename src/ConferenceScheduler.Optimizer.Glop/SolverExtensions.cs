using ConferenceScheduler.Entities;
using ConferenceScheduler.Extensions;
using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceScheduler.Optimizer.Glop
{
    public static class SolverExtensions
    {
        public static Solver CreateMixedIntegerProgrammingSolver(this Solver ignore)
        {
            var solver = new Solver("MIP", Solver.CBC_MIXED_INTEGER_PROGRAMMING);
            if (solver == null)
                throw new InvalidOperationException("Could not create solver");
            return solver;
        }

        public static Variable[] CreateRoomVariables(this Solver model, int sessionCount, int roomCount, bool disableTrace)
        {
            var r = new Variable[sessionCount];

            for (int s = 0; s < sessionCount; s++)
            {
                r[s] = model.MakeIntVar(0, roomCount, $"z[{s}]");
                if (!disableTrace)
                    Console.WriteLine($"Variable: z[{s}]");
            }

            return r;
        }

        public static Variable[,,] CreateAssignmentVariables(this Solver model, int sessionCount, int roomCount, int timeslotCount, bool disableTrace)
        {
            var v = new Variable[sessionCount, roomCount, timeslotCount];
            for (int s = 0; s < sessionCount; s++)
                for (int r = 0; r < roomCount; r++)
                    for (int t = 0; t < timeslotCount; t++)
                    {
                        v[s, r, t] = model.MakeBoolVar($"x[{s},{r},{t}]");
                        if (!disableTrace)
                            Console.WriteLine($"Variable: x[{s},{r},{t}]");
                    }

            return v;
        }


        public static void CreateOneSessionPerRoomTimeslotConstraint(this Solver model, Variable[,,] v, int sessionCount, int roomCount, int timeslotCount, bool disableTrace)
        {
            // Each room can have no more than 1 session per timeslot
            for (int rm = 0; rm < roomCount; rm++)
                for (int t = 0; t < timeslotCount; t++)
                {
                    Constraint expr = model.MakeConstraint(0, 1, $"x[*,{rm},{t}]_LessEqual_1");
                    for (int s = 0; s < sessionCount; s++)
                        expr.SetCoefficient(v[s, rm, t], 1);

                    if (!disableTrace)
                        Console.WriteLine($"x[*,{rm},{t}]_LessEqual_1");
                }
        }

        public static void CreateAssignEverySessionOnceConstraint(this Solver model, Variable[,,] v, int sessionCount, int roomCount, int timeslotCount, bool disableTrace)
        {
            // Each session must be assigned to exactly 1 room/timeslot combination
            for (int s = 0; s < sessionCount; s++)
            {
                Constraint expr = model.MakeConstraint(1.0, 1.0, $"x[{s},*,*]_Equals_1");
                for (int rm = 0; rm < roomCount; rm++)
                    for (int t = 0; t < timeslotCount; t++)
                        expr.SetCoefficient(v[s, rm, t], 1.0);

                if (!disableTrace)
                    Console.WriteLine($"x[{s},*,*]_Equals_1");
            }
        }

        public static void CreateRoomUnavailableConstraint(this Solver model, Variable[,,] v, IEnumerable<Room> rooms, int[] roomIds, int[] timeslotIds, int sessionCount, bool disableTrace)
        {
            // No room can be assigned to a session in a timeslot 
            // during which it is not available
            foreach (var room in rooms)
            {
                int roomIndex = roomIds.IndexOfValue(room.Id).Value;
                foreach (var uts in room.UnavailableForTimeslots)
                {
                    int utsi = timeslotIds.IndexOfValue(uts).Value;
                    Constraint expr = model.MakeConstraint(0.0, 0.0, $"x[*,{roomIndex},{utsi}]_Equals_0");
                    for (int s = 0; s < sessionCount; s++)
                        expr.SetCoefficient(v[s, roomIndex, utsi], 1.0);
                    if (!disableTrace)
                        Console.WriteLine($"x[*,{roomIndex},{utsi}]_Equals_0");
                }
            }
        }

        public static void CreatePresenterUnavailableConstraint(this Solver model, Variable[,,] v, IEnumerable<Session> sessions, int[] sessionIds, int[] timeslotIds, int roomCount, bool disableTrace)
        {
            // Sessions cannot be assigned to a timeslot during which
            // any presenter is unavailable
            foreach (var session in sessions)
            {
                int sessionIndex = sessionIds.IndexOfValue(session.Id).Value;

                List<int> unavailableTimeslotIndexes = new List<int>();
                foreach (var presenter in session.Presenters)
                {
                    foreach (var unavailableTimeslot in presenter.UnavailableForTimeslots)
                    {
                        if (!timeslotIds.IndexOfValue(unavailableTimeslot).HasValue)
                            throw new ArgumentException($"Invalid timeslot {unavailableTimeslot} in presenter {presenter.Name}");
                        else
                        {
                            int timeslotIndex = timeslotIds.IndexOfValue(unavailableTimeslot).Value;
                            unavailableTimeslotIndexes.Add(timeslotIndex);
                        }
                    }
                }

                if (unavailableTimeslotIndexes.Any())
                {
                    Constraint expr = model.MakeConstraint(0.0, 0.0, $"PresentersUnavailableForTimeslot_{session.Name}");
                    foreach (var utsi in unavailableTimeslotIndexes.Distinct())
                        for (int rm = 0; rm < roomCount; rm++)
                            expr.SetCoefficient(v[sessionIndex, rm, utsi], 1.0);
                    if (!disableTrace)
                        Console.WriteLine($"PresentersUnavailableForTimeslot_{session.Name}");
                }
            }
        }

        public static void CreatePresenterCanOnlyPresentOneSessionAtATimeConstraint(this Solver model, Variable[,,] v, IEnumerable<Session> sessions, int[] sessionIds, int roomCount, int timeslotCount, bool disableTrace)
        {
            // A speaker can only be involved with 1 session per timeslot
            var speakerIds = sessions.SelectMany(s => s.Presenters.Select(p => p.Id)).Distinct();
            foreach (int speakerId in speakerIds)
            {
                var pIds = sessions.Where(s => s.Presenters.Select(p => p.Id).Contains(speakerId)).Select(s => s.Id).ToArray();
                for (int i = 0; i < pIds.Length - 1; i++)
                    for (int j = i + 1; j < pIds.Length; j++)
                    {
                        int session1Index = sessionIds.IndexOfValue(pIds[i]).Value;
                        int session2Index = sessionIds.IndexOfValue(pIds[j]).Value;
                        model.CreateConstraintSessionsMustBeInDifferentTimeslots(v, session1Index, session2Index, timeslotCount, roomCount, disableTrace);
                    }
            }
        }

        public static void CreateConstraintSessionsMustBeInDifferentTimeslots(this Solver model, Variable[,,] v, int session1Index, int session2Index, int timeslotCount, int roomCount, bool disableTrace)
        {
            for (int t = 0; t < timeslotCount; t++)
            {
                Constraint expr = model.MakeConstraint(0.0, 1.0, $"x[{session1Index},*,{t}]_NotEqual_x[{session2Index},*,{t}]");
                for (int r = 0; r < roomCount; r++)
                {
                    expr.SetCoefficient(v[session1Index, r, t], 1.0);
                    expr.SetCoefficient(v[session2Index, r, t], 1.0);
                }
                if (!disableTrace)
                    Console.WriteLine($"x[{session1Index},*,{t}]_NotEqual_x[{session2Index},*,{t}]");
            }
        }

        public static void CreateLimitOnTopicIdsInASingleTimeslotConstraint(this Solver model, Variable[,,] v, IEnumerable<Session> sessions, int[] sessionIds, int roomCount, int timeslotCount, bool disableTrace)
        {
            // A timeslot should have no more sessions in a particular 
            // topicId than absolutely necessary.
            var topicIds = sessions.Where(s => s.TopicId.HasValue).Select(s => s.TopicId.Value).Distinct();
            foreach (var topicId in topicIds)
            {
                double topicCount = sessions.Count(s => s.TopicId == topicId);
                double maxTopicCount = System.Math.Ceiling(topicCount / Convert.ToDouble(timeslotCount));

                for (int t = 0; t < timeslotCount; t++)
                {
                    var expr = model.MakeConstraint(0.0, maxTopicCount, $"x[(topicId={topicId}),*,{t}]_LessEqual_{maxTopicCount}");
                    foreach (var session in sessions.Where(s => s.TopicId == topicId))
                    {
                        int sessionIndex = sessionIds.IndexOfValue(session.Id).Value;
                        for (int rm = 0; rm < roomCount; rm++)
                            expr.SetCoefficient(v[sessionIndex, rm, t], 1.0);
                    }
                    if (!disableTrace)
                        Console.WriteLine($"x[(topicId={topicId}),*,{t}]_LessEqual_{maxTopicCount}");
                }
            }
        }

        public static void CreateLimitOnSessionsInATimeslotConstraint(this Solver model, Variable[,,] v, IEnumerable<Session> sessions, int[] sessionIds, int roomCount, int timeslotCount, bool disableTrace)
        {
            // A timeslot should have no more sessions than absolutely necessary.
            // This serves to distribute the sessions around so we don't end up with 
            // one empty (or nearly empty) timeslot
            // NOTE: Because this is a hard constraint, it is possible that it could cause
            // problems when there are a lot of dependencies. 
            // TODO: Make an objective rather than a constraint
            double maxSessionCount = System.Math.Ceiling(Convert.ToDouble(sessions.Count()) / Convert.ToDouble(timeslotCount));
            for (int t = 0; t < timeslotCount; t++)
            {
                var expr = model.MakeConstraint(0.0, maxSessionCount, $"x[*,*,{t}]_LessEqual_{maxSessionCount}");
                foreach (var session in sessions)
                {
                    int sessionIndex = sessionIds.IndexOfValue(session.Id).Value;
                    for (int rm = 0; rm < roomCount; rm++)
                        expr.SetCoefficient(v[sessionIndex, rm, t], 1.0);
                }
                if (!disableTrace)
                    Console.WriteLine($"x[*,*,{t}]_LessEqual_{maxSessionCount}");
            }
        }

        public static void CreateDependenciesMustBeScheduledInOrderConstraint(this Solver model, Variable[,,] v, IEnumerable<Session> sessions, int[] sessionIds, int roomCount, int timeslotCount, bool disableTrace)
        {
            // All sessions with dependencies on a session must be scheduled
            // later (with a higher timeslot index value) than that session S
            foreach (var session in sessions)
            {
                int sessionIndex = sessionIds.IndexOfValue(session.Id).Value;
                foreach (var dependentSession in session.Dependencies)
                {
                    int dependentSessionIndex = sessionIds.IndexOfValue(dependentSession.Id).Value;
                    LinearExpr dExpr = new LinearExpr();
                    LinearExpr sExpr = new LinearExpr();

                    for (int rm = 0; rm < roomCount; rm++)
                        for (int t = 0; t < timeslotCount; t++)
                        {
                            dExpr += (v[dependentSessionIndex, rm, t] * (t + 1));
                            sExpr += (v[sessionIndex, rm, t] * t);
                        }

                    model.Add(dExpr <= sExpr);
                    if (!disableTrace)
                        Console.WriteLine($"s[{sessionIndex},*,*]_GreaterThan_s[{dependentSessionIndex},*,*]");
                }
            }
        }

        public static void CreateRoomVariableConstraints(this Solver model, Variable[,,] v, Variable[] r, IEnumerable<Session> sessions, int[] sessionIds, int roomCount, int timeslotCount, bool disableTrace)
        {
            // Variable R[s] should hold the room # of the session
            foreach (var session in sessions)
            {
                int sessionIndex = sessionIds.IndexOfValue(session.Id).Value;
                if (!disableTrace)
                    Console.WriteLine($"r[{sessionIndex}]=RoomIndex");
                var expr = new LinearExpr();
                for (int t = 0; t < timeslotCount; t++)
                    for (int rm = 0; rm < roomCount; rm++)
                        expr += v[sessionIndex, rm, t] * rm;

                model.Add(r[sessionIndex] == expr);
            }
        }

        public static void CreateLimitsOnRoomsForATimeslotConstraints(this Solver model, Variable[] r, IEnumerable<Session> sessions, int[] sessionIds, int timeslotCount, bool disableTrace)
        {
            // A topicId should be spread-out across no more rooms than absolutely necessary.
            var topicIds = sessions.Where(s => s.TopicId.HasValue).Select(s => s.TopicId.Value).Distinct();
            foreach (var topicId in topicIds)
            {
                double topicCount = sessions.Count(s => s.TopicId == topicId);
                if (topicCount > timeslotCount)
                {
                    if (!disableTrace)
                        Console.WriteLine($"Topic {topicId} has {topicCount} sessions which is more than the {timeslotCount} timeslots.  This topic will not be included in a track");
                }
                else if (topicCount == 1)
                {
                    if (!disableTrace)
                        Console.WriteLine($"Topic {topicId} has only 1 session.  This topic will not be included in a track");
                }
                else
                {
                    var sessionsInTopic = sessions.Where(s => s.TopicId.HasValue && s.TopicId == topicId);
                    foreach (var session in sessionsInTopic)
                    {
                        int sessionIndex = sessionIds.IndexOfValue(session.Id).Value;
                        var otherSessionsInTopic = sessions.Where(s => s.TopicId.HasValue && s.TopicId == topicId && s.Id != session.Id);
                        foreach (var otherSession in otherSessionsInTopic)
                        {
                            int otherSessionIndex = sessionIds.IndexOfValue(otherSession.Id).Value;
                            model.Add(r[sessionIndex] == r[otherSessionIndex]);
                            if (!disableTrace)
                                Console.WriteLine($"z[{sessionIndex}]_Equal_z[{otherSessionIndex}]");
                        }
                    }
                }
            }
        }

        public static void CreateObjective(this Solver model, Variable[,,] v, IEnumerable<Session> sessions, IEnumerable<Room> rooms, int[] timeslotIds, bool disableTrace)
        {
            // Add objective to improve # of presenter's preferred timeslots used
            var sessionIds = sessions.GetIdCollection();
            var objective = model.Objective();
            objective.SetMaximization();
            foreach (var session in sessions)
                foreach (var presenter in session.Presenters)
                    if (presenter.PreferredTimeslots != null)
                        foreach (var timeslotId in presenter.PreferredTimeslots)
                            for (int roomIndex = 0; roomIndex < rooms.Count(); roomIndex++)
                            {
                                int sessionIndex = sessionIds.IndexOfValue(session.Id).Value;
                                int timeslotIndex = timeslotIds.IndexOfValue(timeslotId).Value;
                                objective.SetCoefficient(v[sessionIndex, roomIndex, timeslotIndex], 1);
                                if (!disableTrace)
                                    Console.WriteLine($"Objective Coefficient: x[{sessionIndex},{roomIndex},{timeslotIndex}]");
                            }
        }

        public static void CreateConstraints(this Solver model, Variable[,,] v, Variable[] r, IEnumerable<Session> sessions, IEnumerable<Room> rooms, int[] timeslotIds, bool disableTrace)
        {
            int sessionCount = sessions.Count();
            int roomCount = rooms.Count();
            int timeslotCount = timeslotIds.Count();

            var sessionIds = sessions.GetIdCollection();
            var roomIds = rooms.GetIdCollection();

            model.CreateOneSessionPerRoomTimeslotConstraint(v, sessionCount, roomCount, timeslotCount, disableTrace);
            model.CreateAssignEverySessionOnceConstraint(v, sessionCount, roomCount, timeslotCount, disableTrace);
            model.CreateRoomUnavailableConstraint(v, rooms, roomIds, timeslotIds, sessionCount, disableTrace);
            model.CreatePresenterUnavailableConstraint(v, sessions, sessionIds, timeslotIds, roomCount, disableTrace);
            model.CreatePresenterCanOnlyPresentOneSessionAtATimeConstraint(v, sessions, sessionIds, roomCount, timeslotCount, disableTrace);
            model.CreateLimitOnTopicIdsInASingleTimeslotConstraint(v, sessions, sessionIds, roomCount, timeslotCount, disableTrace);
            model.CreateLimitOnSessionsInATimeslotConstraint(v, sessions, sessionIds, roomCount, timeslotCount, disableTrace);
            model.CreateDependenciesMustBeScheduledInOrderConstraint(v, sessions, sessionIds, roomCount, timeslotCount, disableTrace);

            model.CreateRoomVariableConstraints(v, r, sessions, sessionIds, roomCount, timeslotCount, disableTrace);
            model.CreateLimitsOnRoomsForATimeslotConstraints(r, sessions, sessionIds, timeslotCount, disableTrace);

            //// Variable Y[r,t] should hold the topic id of the session scheduled in the room during that timeslot
            //for (int r = 0; r < roomCount; r++)
            //    for (int t = 0; t < timeslotCount; t++)
            //    {
            //        var expr = _model.MakeConstraint(0.0, 999, $"y[{r},{t}]=TopicId");
            //        Console.WriteLine($"y[{r},{t}]=TopicId");
            //        foreach (var session in sessions.Where(s => s.TopicId.HasValue))
            //        {
            //            int sessionIndex = _sessionIds.IndexOfValue(session.Id).Value;
            //            expr.SetCoefficient(_v[sessionIndex, r, t], session.TopicId.Value);
            //        }
            //    }

        }


    }
}
