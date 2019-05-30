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
                .Id(Int32.MaxValue.GetRandom())
                .Build();

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

        [Fact]
        public void ReturnTheCorrectSessionNameIfSupplied()
        {
            var expected = string.Empty.GetRandom();
            var actualSession = new SessionBuilder()
                .Name(expected)
                .Build(Int32.MaxValue.GetRandom());

            Assert.Equal(expected, actualSession.Name);
        }

        [Fact]
        public void ReturnTheCorrectTopicIdIfSupplied()
        {
            var expected = 25.GetRandom();
            var actualSession = new SessionBuilder()
                .TopicId(expected)
                .Build(Int32.MaxValue.GetRandom());

            Assert.Equal(expected, actualSession.TopicId.Value);
        }

        [Fact]
        public void ReturnANullTopicIdIfNotSupplied()
        {
            var actualSession = new SessionBuilder()
                .Build(Int32.MaxValue.GetRandom());

            Assert.False(actualSession.TopicId.HasValue);
        }

        [Fact]
        public void ReturnAnEmptyPresenterCollectionIfNoneAreAdded()
        {
            var actualSession = new SessionBuilder()
                .Build(Int32.MaxValue.GetRandom());

            Assert.False(actualSession.Presenters.Any());
        }

        [Fact]
        public void ReturnASinglePresenterIfOneIsAdded()
        {
            var expected = new Entities.Presenter()
            {
                Id = Int32.MaxValue.GetRandom()
            };

            var actualSession = new SessionBuilder()
                .AddPresenter(expected)
                .Build(Int32.MaxValue.GetRandom());

            Assert.Single(actualSession.Presenters);
        }

        [Fact]
        public void ReturnTheCorrectNumberOfPresenters()
        {
            int count = 25.GetRandom(5);
            var builder = new SessionBuilder();

            for (int i = 0; i < count; i++)
            {
                builder.AddPresenter(new Entities.Presenter()
                {
                    Id = Int32.MaxValue.GetRandom()
                });
            }

            var actualSession = builder
                .Build(Int32.MaxValue.GetRandom());

            Assert.Equal(count, actualSession.Presenters.Count());
        }

        [Fact]
        public void ReturnTheCorrectPresenterIfOneIsAdded()
        {
            var expected = new Entities.Presenter()
            {
                Id = Int32.MaxValue.GetRandom()
            };

            var actualSession = new SessionBuilder()
                .AddPresenter(expected)
                .Build(Int32.MaxValue.GetRandom());

            Assert.Equal(expected.Id, actualSession.Presenters.Single().Id);
        }

    }
}
