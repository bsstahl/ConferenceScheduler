using System;
using TestHelperExtensions;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace ConferenceScheduler.Fluent.Test
{
    public class TopicBuilder_Build_Should
    {
        [Fact]
        public void ReturnAnEmptyListIfNoTopicsSupplied()
        {
            var actual = new TopicBuilder().Build();
            Assert.Empty(actual);
        }

        [Theory]
        [InlineData("only")]
        [InlineData("a", "b")]
        [InlineData("one", "two", "three")]
        [InlineData("one", "two", "buckle", "my", "shoe")]
        public void ReturnTheCorrectNumberOfTopics(params string[] topicNames)
        {
            var target = new TopicBuilder();
            target.Add(topicNames);
            var actual = target.Build();
            Assert.Equal(topicNames.Count(), actual.Count());
        }

        [Theory]
        [InlineData(1, "only", "only")]
        [InlineData(2, "a", "b", "a", "b", "a")]
        [InlineData(3, "one", "two", "three", "two", "two")]
        [InlineData(5, "one", "one", "buckle", "my", "shoe", "one", "one", "two", "one", "shoe")]
        public void ReturnTheCorrectNumberOfTopicsEvenIfTopicsAreDuplicated(int topicCount, params string[] topicNames)
        {
            var target = new TopicBuilder();
            target.Add(topicNames);
            var actual = target.Build();
            Assert.Equal(topicCount, actual.Count());
        }

        [Fact]
        public void ReturnTheCorrectTopicNameIfOneTopicIsSupplied()
        {
            string expected = "".GetRandom();
            var actual = new TopicBuilder().Add(expected).Build();
            Assert.Equal(expected, actual.Single().Name);
        }

        [Fact]
        public void ReturnTheCorrectTopicNamesIfMultipleTopicsAreSupplied()
        {
            int topicCount = 15.GetRandom(3);
            var expected = new List<string>();
            for (int i = 0; i < topicCount; i++)
                expected.Add("".GetRandom());

            var actual = new TopicBuilder().Add(expected.ToArray()).Build();
            var actualNames = string.Join(';', actual.Select(n => n.Name));
            foreach (var item in expected)
                Assert.Contains(item, actualNames);
        }

        [Fact]
        public void ReturnANonZeroTopicIdForAllTopics()
        {
            int topicCount = 15.GetRandom(3);
            var expected = new List<string>();
            for (int i = 0; i < topicCount; i++)
                expected.Add("".GetRandom());

            var actual = new TopicBuilder().Add(expected.ToArray()).Build();
            Assert.DoesNotContain(actual, t => t.Id == 0);
        }

        [Fact]
        public void ReturnAUniqueTopicIdForEachDistinctTopic()
        {
            int topicCount = 15.GetRandom(3);
            var expected = new List<string>();
            for (int i = 0; i < topicCount; i++)
                expected.Add("".GetRandom());

            var actual = new TopicBuilder().Add(expected.ToArray()).Build();

            int expectedCount = actual.Select(t => t.Name).Distinct().Count();
            int actualCount = actual.Select(t => t.Id).Distinct().Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void ReturnOnlyOneTopicForNamesDifferingByOnlyCase()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void ReturnOnlyOneTopicForNamesDifferingByOnlyWhitespace()
        {
            throw new NotImplementedException();
        }
    }
}
