using System;
using TestHelperExtensions;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace ConferenceScheduler.Fluent.Test
{
    public class TopicCollection_NameAccessor_Should
    {
        [Fact]
        public void ReturnNullIfCollectionIsEmpty()
        {
            var target = new TopicCollection();
            var actual = target["".GetRandom()];
            Assert.False(actual.HasValue);
        }

        [Fact]
        public void ReturnNullIfNameIsNotInTheCollection()
        {
            var target = new TopicCollection();
            for (int i = 0; i < 10.GetRandom(4); i++)
                target.Add(new Topic() { Id = i, Name = "".GetRandom(10) });

            var actual = target["".GetRandom(15)];
            Assert.False(actual.HasValue);
        }

        [Fact]
        public void ReturnTheProperIdIfTheNameIsFirstInTheCollection()
        {
            int expectedId = 1;
            string expectedName = "".GetRandom(10);

            var target = new TopicCollection();
            target.Add(new Topic() { Id = expectedId, Name = expectedName });
            target.Add(new Topic() { Id = 999.GetRandom(), Name = "".GetRandom(10) });
            target.Add(new Topic() { Id = 999.GetRandom(), Name = "".GetRandom(10) });

            var actual = target[expectedName];
            Assert.Equal(expectedId, actual);
        }

        [Fact]
        public void ReturnTheProperIdIfTheNameIsLastInTheCollection()
        {
            int expectedId = 3;
            string expectedName = "".GetRandom(10);

            var target = new TopicCollection();
            target.Add(new Topic() { Id = 999.GetRandom(), Name = "".GetRandom(10) });
            target.Add(new Topic() { Id = 999.GetRandom(), Name = "".GetRandom(10) });
            target.Add(new Topic() { Id = expectedId, Name = expectedName });

            var actual = target[expectedName];
            Assert.Equal(expectedId, actual);
        }

        [Fact]
        public void ReturnTheProperIdIfTheNameIsInTheCollection()
        {
            var target = new TopicCollection();
            for (int i = 0; i < 10.GetRandom(4); i++)
                target.Add(new Topic() { Id = i, Name = "".GetRandom(10) });

            var expectedTopic = target.GetRandom();
            var actual = target[expectedTopic.Name];
            Assert.Equal(expectedTopic.Id, actual);
        }

    }
}
