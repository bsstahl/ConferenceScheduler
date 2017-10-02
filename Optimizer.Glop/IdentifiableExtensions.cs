using ConferenceScheduler.Entities;
using ConferenceScheduler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceScheduler.Optimizer.Glop
{
    public static class IdentifiableExtensions
    {
        public static int[] GetIdCollection(this IEnumerable<IIdentifiable> data)
        {
            var result = new int[data.Count()];
            int index = 0;
            foreach (var item in data)
            {
                result[index] = item.Id;
                index++;
            }
            return result;
        }

    }
}
