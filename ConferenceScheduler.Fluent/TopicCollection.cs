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
            get
            {
                string lName = name.ToLower();
                if (!this.Any(t => t.Name.ToLower() == lName))
                    throw new Exceptions.ItemNotFoundException(this.GetType(), $"Unable to find topic '{name}'");
                return this.Single(t => t.Name.ToLower() == lName).Id;
            }

            // set => throw new NotImplementedException();
        }

        public bool ContainsName(string name)
        {
            return this.Select(t => t.Name.ToLower().RemoveWhitespace()).Contains(name.ToLower().RemoveWhitespace());
        }
    }
}
