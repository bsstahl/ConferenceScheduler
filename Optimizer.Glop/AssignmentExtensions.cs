using ConferenceScheduler.Entities;
using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceScheduler.Optimizer.Glop
{
    public static class AssignmentExtensions
    {
        public static IEnumerable<Assignment> CreateAssignments(this IEnumerable<Assignment> ignore, Variable[,,] v, int[] roomIds, int[] timeslotIds, int[] sessionIds)
        {
            var results = new List<Assignment>();
            for (int s = 0; s < sessionIds.Count(); s++)
                for (int rm = 0; rm < roomIds.Count(); rm++)
                    for (int t = 0; t < timeslotIds.Count(); t++)
                    {
                        if (v[s, rm, t].SolutionValue() == 1.0)
                            results.Add(new Assignment(roomIds[rm], timeslotIds[t], sessionIds[s]));
                    }

            return results;
        }
    }
}
