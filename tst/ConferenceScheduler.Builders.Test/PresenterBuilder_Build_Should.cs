using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;

namespace ConferenceScheduler.Builders.Test
{    public class PresenterBuilder_Build_Should
    {
        [Fact]
        public void ReturnAPresenterInstanceIfAnIdIsSupplied()
        {
            var actual = new PresenterBuilder().Id(Int32.MaxValue.GetRandom()).Build();
            Assert.NotNull(actual);
        }

        [Fact]
        public void ReturnAPresenterInstanceIfANextIdIsSupplied()
        {
            var actual = new PresenterBuilder().Build(Int32.MaxValue.GetRandom());
            Assert.NotNull(actual);
        }

        [Fact]
        public void ThrowIfNoIdIsSupplied()
        {
            Assert.Throws<InvalidOperationException>(() => new PresenterBuilder().Build());
        }

        [Fact]
        public void ReturnTheCorrectPresenterIdIfTheIdIsSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualPresenter = new PresenterBuilder()
                .Id(expected).Build();
            Assert.Equal(expected, actualPresenter.Id);
        }

        [Fact]
        public void ReturnTheCorrectPresenterIdIfTheNextIdIsSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualPresenter = new PresenterBuilder().Build(expected);
            Assert.Equal(expected, actualPresenter.Id);
        }

        [Fact]
        public void ReturnTheCorrectPresenterIdIgnoringTheNextIdIfBothAreSupplied()
        {
            int expected, ignored;
            do
            {
                expected = Int32.MaxValue.GetRandom();
                ignored = Int32.MaxValue.GetRandom();
            }
            while (expected == ignored);

            var actualPresenter = new PresenterBuilder()
                .Id(expected).Build(ignored);

            Assert.Equal(expected, actualPresenter.Id);
            Assert.NotEqual(ignored, actualPresenter.Id);
        }

        [Fact]
        public void ReturnAnEmptyTimeslotsUnavailableCollectionIfNoneAreSupplied()
        {
            var actualPresenter = new PresenterBuilder().Build(Int32.MaxValue.GetRandom());
            Assert.Empty(actualPresenter.UnavailableForTimeslots);
        }

        [Fact]
        public void ReturnAnEmptyOrNullNameIfNoneIsSupplied()
        {
            var actualPresenter = new PresenterBuilder()
                .Build(Int32.MaxValue.GetRandom());
            Assert.True(string.IsNullOrEmpty(actualPresenter.Name));
        }

        [Fact]
        public void ReturnTheSpecifiedNameIfOneIsSupplied()
        {
            var actual = string.Empty.GetRandom();
            var actualPresenter = new PresenterBuilder()
                .Name(actual)
                .Build(Int32.MaxValue.GetRandom());
            Assert.Equal(actual, actualPresenter.Name);
        }

        [Fact]
        public void ReturnATimeslotsUnavailableCollectionWithOneValueIfOneIsSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualPresenter = new PresenterBuilder()
                .AddTimeslotUnavailable(expected)
                .Build(Int32.MaxValue.GetRandom());
            Assert.Single(actualPresenter.UnavailableForTimeslots);
        }

        [Fact]
        public void ReturnTheCorrectTimeslotUnavailableValueIfIfOneIsSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualPresenter = new PresenterBuilder()
                .AddTimeslotUnavailable(expected)
                .Build(Int32.MaxValue.GetRandom());
            Assert.Equal(expected, actualPresenter.UnavailableForTimeslots.First());
        }

        [Fact]
        public void ReturnATimeslotsUnavailableCollectionWithNValuesIfNAreSupplied()
        {
            var expected = new List<int>();
            var target = new PresenterBuilder();

            var count = 25.GetRandom(10);
            for (int i = 0; i < count; i++)
            {
                int expectedTimeslot = Int32.MaxValue.GetRandom();
                expected.Add(expectedTimeslot);
                target.AddTimeslotUnavailable(expectedTimeslot);
            }

            var actualPresenter = target.Build(Int32.MaxValue.GetRandom());
            Assert.Equal(count, actualPresenter.UnavailableForTimeslots.Count());
        }

        [Fact]
        public void ReturnTheCorrectTimeslotUnavailableValuesIfIfNAreSupplied()
        {
            var expected = new List<int>();
            var target = new PresenterBuilder();

            var count = 25.GetRandom(10);
            for (int i = 0; i < count; i++)
            {
                int expectedTimeslot = Int32.MaxValue.GetRandom();
                expected.Add(expectedTimeslot);
                target.AddTimeslotUnavailable(expectedTimeslot);
            }

            var actualPresenter = target.Build(Int32.MaxValue.GetRandom());
            Assert.True(expected.HasSameValues(actualPresenter.UnavailableForTimeslots));
        }

        [Fact]
        public void ReturnAnEmptyPreferredTimeslotCollectionIfNoneAreSupplied()
        {
            var actualPresenter = new PresenterBuilder().Build(Int32.MaxValue.GetRandom());
            Assert.Empty(actualPresenter.PreferredTimeslots);
        }

        [Fact]
        public void ReturnAPreferredTimeslotCollectionWithOneValueIfOneIsSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualPresenter = new PresenterBuilder()
                .AddPreferredTimeslot(expected)
                .Build(Int32.MaxValue.GetRandom());
            Assert.Single(actualPresenter.PreferredTimeslots);
        }

        [Fact]
        public void ReturnTheCorrectPreferredTimeslotValueIfIfOneIsSupplied()
        {
            var expected = Int32.MaxValue.GetRandom();
            var actualPresenter = new PresenterBuilder()
                .AddPreferredTimeslot(expected)
                .Build(Int32.MaxValue.GetRandom());
            Assert.Equal(expected, actualPresenter.PreferredTimeslots.First());
        }

        [Fact]
        public void ReturnAPreferredTimeslotCollectionWithNValuesIfNAreSupplied()
        {
            var expected = new List<int>();
            var target = new PresenterBuilder();

            var count = 25.GetRandom(10);
            for (int i = 0; i < count; i++)
            {
                int expectedTimeslot = Int32.MaxValue.GetRandom();
                expected.Add(expectedTimeslot);
                target.AddPreferredTimeslot(expectedTimeslot);
            }

            var actualPresenter = target.Build(Int32.MaxValue.GetRandom());
            Assert.Equal(count, actualPresenter.PreferredTimeslots.Count());
        }

        [Fact]
        public void ReturnTheCorrectPreferredTimeslotValuesIfIfNAreSupplied()
        {
            var expected = new List<int>();
            var target = new PresenterBuilder();

            var count = 25.GetRandom(10);
            for (int i = 0; i < count; i++)
            {
                int expectedTimeslot = Int32.MaxValue.GetRandom();
                expected.Add(expectedTimeslot);
                target.AddPreferredTimeslot(expectedTimeslot);
            }

            var actualPresenter = target.Build(Int32.MaxValue.GetRandom());
            Assert.True(expected.HasSameValues(actualPresenter.PreferredTimeslots));
        }

    }
}
