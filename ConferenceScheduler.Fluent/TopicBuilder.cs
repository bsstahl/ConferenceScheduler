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
            int topicId = 0;
            foreach (var topicName in _topicNames.Distinct())
                if (!result.ContainsName(topicName))
                    result.Add(new Topic()
                    {
                        Id = ++topicId,
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
