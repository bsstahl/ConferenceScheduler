using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TestHelperExtensions;

namespace ConferenceScheduler.Builders.Test
{
    public class TimeslotBuilder_Build_Should
    {
        [Fact]
        public void ReturnATimeslotWithTheIdSpecifiedInTheConstructor()
        {
            int id = Int32.MaxValue.GetRandom();
            var actual = new TimeslotBuilder(id).Build();
            Assert.Equal(id, actual.Id);
        }

        [Fact]
        public void ReturnATimeslotWithTheStartTimeSpecifiedByTheStartingAtMethod()
        {
            int id = Int32.MaxValue.GetRandom();
            var startTime = Convert.ToSingle((20.0).GetRandom(0));
            var actual = new TimeslotBuilder(id).StartingAt(startTime).Build();
            Assert.Equal(startTime, actual.StartHour);
        }

        [Fact]
        public void ReturnATimeslotWithTheDayIndexSpecifiedByTheOnDayMethod()
        {
            int id = Int32.MaxValue.GetRandom();
            byte dayIndex = (byte)10.GetRandom();
            var actual = new TimeslotBuilder(id).OnDay(dayIndex).Build();
            Assert.Equal(dayIndex, actual.DayIndex);
        }
    }
}
