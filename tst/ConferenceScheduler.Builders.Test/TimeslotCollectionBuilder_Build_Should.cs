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
    public class TimeslotCollectionBuilder_Build_Should
    {
        [Fact]
        public void ReturnAnInstanceOfATimeslotCollection()
        {
            var actual = new TimeslotCollectionBuilder().Build();
            Assert.NotNull(actual);
        }

        [Fact]
        public void ReturnAnEmptyCollectionIfNoTimeslotsAreSupplied()
        {
            var actual = new TimeslotCollectionBuilder().Build();
            Assert.Empty(actual);
        }

        [Fact]
        public void ReturnACollectionWithASingleTimeslotIfOneTimslotBuilderAdded()
        {
            int id = Int32.MaxValue.GetRandom();
            var actual = new TimeslotCollectionBuilder()
                .Add(new TimeslotBuilder(id))
                .Build();
            Assert.Single(actual);
        }

        [Fact]
        public void ReturnACollectionWithASingleTimeslotIfOneTimeslotAdded()
        {
            int id = Int32.MaxValue.GetRandom();
            var actual = new TimeslotCollectionBuilder()
                .Add(Timeslot.Create(id))
                .Build();
            Assert.Single(actual);
        }

        [Fact]
        public void ThrowIfATimeslotIsAddedWithoutAnId()
        {
            Assert.Throws<MissingIdentityException>(() => new TimeslotCollectionBuilder().Add(new Timeslot()));
        }

        [Fact]
        public void ReturnACollectionWithTwoTimeslotsIfATimeslotAndATimeslotBuilderAreAdded()
        {
            int id1 = Int32.MaxValue.GetRandom();
            int id2 = Int32.MaxValue.GetRandom();
            var actual = new TimeslotCollectionBuilder()
                .Add(Timeslot.Create(id1))
                .Add(new TimeslotBuilder(id2))
                .Build();
            Assert.Equal(2, actual.Count());
        }

    }
}
