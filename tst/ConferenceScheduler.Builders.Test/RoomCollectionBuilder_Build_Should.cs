using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using ConferenceScheduler.Entities;

namespace ConferenceScheduler.Builders.Test
{
    public class RoomCollectionBuilder_Build_Should
    {
        [Fact]
        public void ReturnAnInstanceOfARoomCollection()
        {
            var actual = new RoomCollectionBuilder().Build();
            Assert.NotNull(actual);
        }

        [Fact]
        public void ReturnAnEmptyCollectionIfNoRoomsAreSupplied()
        {
            var actual = new RoomCollectionBuilder().Build();
            Assert.Empty(actual);
        }

        [Fact]
        public void ReturnACollectionWithASingleRoomIfOneRoomBuilderAdded()
        {
            var actual = new RoomCollectionBuilder()
                .Add(new RoomBuilder())
                .Build();
            Assert.Single(actual);
        }

        [Fact]
        public void ReturnACollectionWithASingleRoomIfOneRoomAdded()
        {
            var actual = new RoomCollectionBuilder()
                .Add(new Room())
                .Build();
            Assert.Single(actual);
        }

        [Fact]
        public void ReturnACollectionWithTwoRoomsIfARoomAndARoomBuilderAreAdded()
        {
            var actual = new RoomCollectionBuilder()
                .Add(new Room())
                .Add(new RoomBuilder())
                .Build();
            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public void NotReturnAnyRoomsWithDuplicateIdsIfNoIdsAreSpecified()
        {
            var target = new RoomCollectionBuilder();

            int count = 100.GetRandom(25);
            for (int i = 0; i < count; i++)
            {
                target.Add(new Room());
                target.Add(new RoomBuilder());
            }

            var actual = target.Build();
            Assert.Equal(2 * count, actual.Select(r => r.Id).Distinct().Count());
        }
    }
}
