﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConferenceScheduler.Interfaces;
using ConferenceScheduler.Entities;
using Xunit.Abstractions;
using ConferenceScheduler.Builders;

namespace ConferenceScheduler.Optimizer.DataSetsTest
{
    public static class ExtensionMethods
    {
        public static SessionBuilder Topic(this SessionBuilder builder, SoCalSanDiegoTopic topic)
        {
            return builder.TopicId((int)topic);
        }

        public static IConferenceOptimizer Create(this IConferenceOptimizer ignore)
        {
            return ignore.Create(new EventHandler());
        }

        public static ConferenceScheduler.Interfaces.IConferenceOptimizer Create(this IConferenceOptimizer ignore, EventHandler eventHandlers)
        {
            // return new Engine(eventHandlers.EngineUpdateEventHandler);
            // return new Gurobi.Engine(eventHandlers.EngineUpdateEventHandler);
            return new ConferenceScheduler.Optimizer.Glop.Engine(eventHandlers.EngineUpdateEventHandler);
        }

        public static string ToTimeFormat(this Single time)
        {
            if ((time > 24.0) || (time < 0.0))
                throw new ArgumentException("The time must be between 0.0 and 24.0", nameof(time));
            var minutes = Convert.ToInt32(Math.Floor(time));
            var seconds = Convert.ToInt32(Math.Round((time - minutes) * 60.0));
            return $"{minutes:00}:{seconds:00}";
        }

        public static void WriteTimeslotConfiguration(this ITestOutputHelper output, IEnumerable<Timeslot> timeslots)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Timeslot configuration:");
            foreach (var timeslot in timeslots)
                sb.AppendLine($"\tSlot {timeslot.Id} starts at {timeslot.StartHour.ToTimeFormat()} on day {timeslot.DayIndex}");
            output.WriteLine(sb.ToString());
        }

        public static void WriteRoomConfiguration(this ITestOutputHelper output, IEnumerable<Room> rooms)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Room configuration:");
            foreach (var room in rooms)
            {
                sb.Append($"\tRoom {room.Id} with capacity {room.Capacity} is ");
                if (room.UnavailableForTimeslots == null || !room.UnavailableForTimeslots.Any())
                    sb.AppendLine("available in all Timeslots");
                else
                    sb.AppendLine($"not available in timeslot(s) {string.Join(',', room.UnavailableForTimeslots)}");
            }
            output.WriteLine(sb.ToString());
        }

        public static void WriteSchedule(this ITestOutputHelper output, IEnumerable<Assignment> assignments, IEnumerable<Session> sessions)
        {
            var names = new Dictionary<int, string>();
            foreach (var s in sessions)
            {
                names.Add(s.Id, s.Name);
            }

            var timeslots = assignments.Select(a => a.TimeslotId).Distinct().OrderBy(a => a);
            var rooms = assignments.Select(a => a.RoomId).Distinct().OrderBy(a => a);

            var result = new StringBuilder();

            result.Append("R\\T\t|\t");

            foreach (var timeslot in timeslots)
                result.Append($"{timeslot.ToString().PadRight(15, ' ')}\t");

            result.AppendLine();
            result.AppendLine("---------------------------------------------------------------------------");

            foreach (var room in rooms)
            {
                result.Append($"{room}\t|\t");
                foreach (var timeslot in timeslots)
                {
                    if (assignments.Count(a => a.RoomId == room && a.TimeslotId == timeslot) > 1)
                        throw new ArgumentException($"Multiple assignments to room {room} and timeslot {timeslot}.");
                    else
                    {
                        string name;

                        var session = assignments.Where(a => a.RoomId == room && a.TimeslotId == timeslot).SingleOrDefault();
                        if (session == null)
                            name = "(empty)".PadRight(15);
                        else if (names == null)
                            name = session.SessionId.ToString().PadRight(15);
                        else
                        {
                            var fullName = names.Single(n => n.Key == session.SessionId).Value;
                            var currentSession = sessions?.Single(s => s.Id == session.SessionId);
                            name = $"{fullName.Substring(0, Math.Min(fullName.Length, 10)).PadRight(10)} ({ (currentSession == null ? session.SessionId.Value : currentSession.TopicId)})";
                        }
                        result.Append($"{name}\t");
                    }
                }
                result.AppendLine();
            }

            output.WriteLine(result.ToString());
        }

        public static void WriteSchedule(this IEnumerable<Assignment> assignments)
        {
            // TODO: Implement
        }


    }
}
