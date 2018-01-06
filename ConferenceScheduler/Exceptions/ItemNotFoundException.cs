using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceScheduler.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public Type ItemType { get; private set; }

        public ItemNotFoundException() : base() { }

        public ItemNotFoundException(Type itemType, string message) : base(message)
        {
            this.ItemType = itemType;
        }
    }
}
