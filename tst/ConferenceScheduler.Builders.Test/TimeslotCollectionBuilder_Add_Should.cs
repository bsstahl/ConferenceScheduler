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
    public class TimeslotCollectionBuilder_Add_Should
    {
        [Fact]
        public void ThrowAMissingIdentityExceptionIfATimeslotIsAddedWithoutAnId()
        {
            var target = new TimeslotCollectionBuilder();
            Assert.Throws<MissingIdentityException>(() => target.Add(new Timeslot()));
        }

        [Fact]
        public void ThrowADuplicateEntityExceptionIfATimeslotIsAddedWithAnIdAlreadyAddedAsATimeslot()
        {
            int id = Int32.MaxValue.GetRandom();
            var target = new TimeslotCollectionBuilder()
                .Add(Timeslot.Create(id));
            Assert.Throws<DuplicateEntityException>(() => target.Add(Timeslot.Create(id)));
        }

        [Fact]
        public void ThrowADuplicateEntityExceptionIfATimeslotIsAddedWithAnIdAlreadyAddedAsATimeslotBuilder()
        {
            int id = Int32.MaxValue.GetRandom();
            var target = new TimeslotCollectionBuilder()
                .Add(new TimeslotBuilder(id));
            Assert.Throws<DuplicateEntityException>(() => target.Add(Timeslot.Create(id)));
        }

        [Fact]
        public void ThrowADuplicateEntityExceptionIfATimeslotBuilderIsAddedWithAnIdAlreadyAddedAsATimeslot()
        {
            int id = Int32.MaxValue.GetRandom();
            var target = new TimeslotCollectionBuilder()
                .Add(Timeslot.Create(id));
            Assert.Throws<DuplicateEntityException>(() => target.Add(new TimeslotBuilder(id)));
        }

        [Fact]
        public void ThrowADuplicateEntityExceptionIfATimeslotBuilderIsAddedWithAnIdAlreadyAddedAsATimeslotBuilder()
        {
            int id = Int32.MaxValue.GetRandom();
            var target = new TimeslotCollectionBuilder()
                .Add(new TimeslotBuilder(id));
            Assert.Throws<DuplicateEntityException>(() => target.Add(new TimeslotBuilder(id)));
        }
    }
}
