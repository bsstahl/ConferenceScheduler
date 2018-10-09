using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;

namespace ConferenceScheduler.Builders.Test
{
    public class RoomBuilder_Build_Should
    {
        [Fact]
        public void ReturnARoomInstanceIfAnIdIsSupplied()
        {
            var actual = new RoomBuilder().Id(Int32.MaxValue.GetRandom()).Build();
            Assert.NotNull(actual);
        }

        [Fact]
        public void ReturnARoomInstanceIfANextIdIsSupplied()
        {
            var actual = new RoomBuilder().Build(Int32.MaxValue.GetRandom());
            Assert.NotNull(actual);
        }

        [Fact]
        public void ThrowIfNoIdIsSupplied()
        {
            Assert.Throws<InvalidOperationException>(() => new RoomBuilder().Build());
        }

        [Fact]
        public void ReturnTheCorrectRoomIdIfTheIdIsSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualRoom = new RoomBuilder()
                .Id(expected).Build();
            Assert.Equal(expected, actualRoom.Id);
        }

        [Fact]
        public void ReturnTheCorrectRoomIdIfTheNextIdIsSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualRoom = new RoomBuilder().Build(expected);
            Assert.Equal(expected, actualRoom.Id);
        }

        [Fact]
        public void ReturnTheCorrectRoomIdIgnoringTheNextIdIfBothAreSupplied()
        {
            int expected, ignored;
            do
            {
                expected = Int32.MaxValue.GetRandom();
                ignored = Int32.MaxValue.GetRandom();
            }
            while (expected == ignored);

            var actualRoom = new RoomBuilder()
                .Id(expected).Build(ignored);

            Assert.Equal(expected, actualRoom.Id);
            Assert.NotEqual(ignored, actualRoom.Id);
        }

        [Fact]
        public void ReturnZeroIfCapacityNotSupplied()
        {
            var actualRoom = new RoomBuilder().Build(Int32.MaxValue.GetRandom());
            Assert.Equal(0, actualRoom.Capacity);
        }

        [Fact]
        public void ReturnTheCorrectCapacityIfSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualRoom = new RoomBuilder()
                .Capacity(expected).Build(Int32.MaxValue.GetRandom());
            Assert.Equal(expected, actualRoom.Capacity);
        }

        [Fact]
        public void ReturnAnEmptyTimeslotsUnavailableCollectionIfNoneAreSupplied()
        {
            var actualRoom = new RoomBuilder().Build(Int32.MaxValue.GetRandom());
            Assert.Empty(actualRoom.UnavailableForTimeslots);
        }

        [Fact]
        public void ReturnATimeslotsUnavailableCollectionWithOneValueIfOneIsSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualRoom = new RoomBuilder()
                .AddTimeslotUnavailable(expected)
                .Build(Int32.MaxValue.GetRandom());
            Assert.Single(actualRoom.UnavailableForTimeslots);
        }

        [Fact]
        public void ReturnTheCorrectTimeslotUnavailableValueIfIfOneIsSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualRoom = new RoomBuilder()
                .AddTimeslotUnavailable(expected)
                .Build(Int32.MaxValue.GetRandom());
            Assert.Equal(expected, actualRoom.UnavailableForTimeslots.First());
        }

        [Fact]
        public void ReturnATimeslotsUnavailableCollectionWithNValuesIfNAreSupplied()
        {
            var expected = new List<int>();
            var target = new RoomBuilder();

            var count = 25.GetRandom(10);
            for (int i = 0; i < count; i++)
            {
                int expectedTimeslot = Int32.MaxValue.GetRandom();
                expected.Add(expectedTimeslot);
                target.AddTimeslotUnavailable(expectedTimeslot);
            }

            var actualRoom = target.Build(Int32.MaxValue.GetRandom());
            Assert.Equal(count, actualRoom.UnavailableForTimeslots.Count());
        }

        [Fact]
        public void ReturnTheCorrectTimeslotUnavailableValuesIfIfNAreSupplied()
        {
            var expected = new List<int>();
            var target = new RoomBuilder();

            var count = 25.GetRandom(10);
            for (int i = 0; i < count; i++)
            {
                int expectedTimeslot = Int32.MaxValue.GetRandom();
                expected.Add(expectedTimeslot);
                target.AddTimeslotUnavailable(expectedTimeslot);
            }

            var actualRoom = target.Build(Int32.MaxValue.GetRandom());
            Assert.True(expected.HasSameValues(actualRoom.UnavailableForTimeslots));
        }

    }
}
