using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;

namespace ConferenceScheduler.Builders.Test
{
    public class SessionBuilder_Build_Should
    {
        [Fact]
        public void ReturnASessionInstanceIfAnIdIsSupplied()
        {
            var actual = new SessionBuilder()
                .Id(Int32.MaxValue.GetRandom()).Build();
            Assert.NotNull(actual);
        }

        [Fact]
        public void ReturnASessionInstanceIfANextIdIsSupplied()
        {
            var actual = new SessionBuilder()
                .Build(Int32.MaxValue.GetRandom());
            Assert.NotNull(actual);
        }

        [Fact]
        public void ThrowIfNoIdIsSupplied()
        {
            Assert.Throws<InvalidOperationException>(() => new SessionBuilder().Build());
        }

        [Fact]
        public void ReturnTheCorrectSessionIdIfTheIdIsSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualSession = new SessionBuilder()
                .Id(expected).Build();
            Assert.Equal(expected, actualSession.Id);
        }

        [Fact]
        public void ReturnTheCorrectSessionIdIfTheNextIdIsSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualSession = new SessionBuilder().Build(expected);
            Assert.Equal(expected, actualSession.Id);
        }

        [Fact]
        public void ReturnTheCorrectSessionIdIgnoringTheNextIdIfBothAreSupplied()
        {
            int expected, ignored;
            do
            {
                expected = Int32.MaxValue.GetRandom();
                ignored = Int32.MaxValue.GetRandom();
            }
            while (expected == ignored);

            var actualSession = new SessionBuilder()
                .Id(expected).Build(ignored);

            Assert.Equal(expected, actualSession.Id);
            Assert.NotEqual(ignored, actualSession.Id);
        }

    }
}
