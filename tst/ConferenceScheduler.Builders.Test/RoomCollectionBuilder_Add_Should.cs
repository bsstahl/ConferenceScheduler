using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using ConferenceScheduler.Entities;
using ConferenceScheduler.Exceptions;

namespace ConferenceScheduler.Builders.Test
{
    public class RoomCollectionBuilder_Add_Should
    {
        [Fact]
        public void ThrowADuplicateEntityExceptionIfARoomIsAddedWithAnIdAlreadyAddedAsARoom()
        {
            int id = Int32.MaxValue.GetRandom();
            var target = new RoomCollectionBuilder()
                .Add(Room.Create(id, 10));
            Assert.Throws<DuplicateEntityException>(() => target.Add(Room.Create(id, 5)));
        }

        [Fact]
        public void ThrowADuplicateEntityExceptionIfARoomIsAddedWithAnIdAlreadyAddedAsARoomBuilder()
        {
            int id = Int32.MaxValue.GetRandom();
            var target = new RoomCollectionBuilder()
                .Add(new RoomBuilder().Id(id));
            Assert.Throws<DuplicateEntityException>(() => target.Add(Room.Create(id, 5)));
        }

        [Fact]
        public void ThrowADuplicateEntityExceptionIfARoomBuilderIsAddedWithAnIdAlreadyAddedAsARoom()
        {
            int id = Int32.MaxValue.GetRandom();
            var target = new RoomCollectionBuilder()
                .Add(Room.Create(id, 5));
            Assert.Throws<DuplicateEntityException>(() => target.Add(new RoomBuilder().Id(id)));
        }

        [Fact]
        public void ThrowADuplicateEntityExceptionIfARoomBuilderIsAddedWithAnIdAlreadyAddedAsARoomBuilder()
        {
            int id = Int32.MaxValue.GetRandom();
            var target = new RoomCollectionBuilder()
                .Add(new RoomBuilder().Id(id));
            Assert.Throws<DuplicateEntityException>(() => target.Add(new RoomBuilder().Id(id)));
        }
    }
}
