using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceScheduler.Builders.Test
{
    public static class ExtensionMethods
    {
        public static bool HasSameValues<T>(this IEnumerable<T> list, IEnumerable<T> compareTo)
        {
            bool result = (list.Count() == compareTo.Count());

            if (result)
            {
                var orderedList1 = list.OrderBy(s => s).ToArray();
                var orderedList2 = compareTo.OrderBy(s => s).ToArray();

                int i = 0;
                while (i < list.Count() && result)
                {
                    if (!orderedList1[i].Equals(orderedList2[i]))
                        result = false;
                    i++;
                }
            }

            return result;
        }
    }
}
