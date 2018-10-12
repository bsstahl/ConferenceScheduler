using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TestHelperExtensions;
using ConferenceScheduler.Exceptions;

namespace ConferenceScheduler.Builders.Test
{
    public class TimeslotBuilder_Add_Should
    {
        [Fact]
        public void ThrowAMissingIdentityExceptionIfTheIdIsZero()
        {
            Assert.Throws<MissingIdentityException>(() => new TimeslotBuilder(0));
        }
    }
}
