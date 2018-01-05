using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.Linq;

namespace ConferenceScheduler
{
    public class TopicCollection : List<Topic>
    {
        public int? this[string name]
        {
            get =>
                this.FirstOrDefault(t => t.Name.ToLower() == name.ToLower())?.Id;
            // set => throw new NotImplementedException();
        }

        public bool ContainsName(string name)
        {
            return this.Select(t => t.Name.ToLower().RemoveWhitespace()).Contains(name.ToLower().RemoveWhitespace());
        }
    }
}
