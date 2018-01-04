using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceScheduler
{
    public class TopicBuilder
    {
        List<string> _topicNames = new List<string>();

        public TopicCollection Build()
        {
            var result = new TopicCollection();
            foreach (var topicName in _topicNames.Distinct())
                result.Add(new Topic()
                {
                    Id = 0,
                    Name = topicName
                });
            return result;
        }


        public TopicBuilder Add(params string[] topicNames)
        {
            _topicNames.AddRange(topicNames);
            return this;
        }

    }

}
