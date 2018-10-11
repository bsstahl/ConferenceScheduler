using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceScheduler.Exceptions
{
    public class DuplicateEntityException: Exception
    {
        public DuplicateEntityException(Type entityType, int id)
            : base($"Multiple {entityType.FullName} entities have id {id}")  { }
    }
}
