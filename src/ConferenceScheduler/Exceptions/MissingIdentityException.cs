using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceScheduler.Exceptions
{
    public class MissingIdentityException:Exception
    {
        public MissingIdentityException(Type entityType):base($"A {entityType.Name} must have an Id")  { }
    }
}
