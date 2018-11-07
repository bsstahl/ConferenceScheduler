using ConferenceScheduler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceScheduler.Entities
{
    /// <summary>
    /// Represents a session (presentation) to be delivered at the conference
    /// </summary>
    public class Session : IIdentifiable
    {
        private List<Session> _dependentSessions;

        /// <summary>
        /// The primary-key identifier of the object
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A secondary (optional) unique id
        /// typically used to sync with an external system
        /// </summary>
        /// <remarks>This is ignored by the schedule optimizer</remarks>
        public string Uid { get; set; }

        /// <summary>
        /// The title of the session.
        /// </summary>
        /// <remarks>This field is used for display only and is not needed by the optimizer</remarks>
        public string Name { get; set; }

        /// <summary>
        /// The primary-key identifier of the Topic (Track) of the session
        /// </summary>
        public int? TopicId { get; set; }

        /// <summary>
        /// Contains the list of presenters for the session
        /// </summary>
        public IEnumerable<Presenter> Presenters { get; set; }

        /// <summary>
        /// Contains the list of sessions that this session is dependent upon.
        /// All dependencies must happen prior to this session.
        /// </summary>
        public IEnumerable<Session> Dependencies
        {
            get { return _dependentSessions; }
        }

        /// <summary>
        /// Create an instance of the object
        /// </summary>
        public Session()
        {
            _dependentSessions = new List<Session>();
        }

        /// <summary>
        /// Recursively searches the dependency tree to see if the
        /// current session is dependent on the specified session.
        /// </summary>
        /// <param name="dependencySessionId">The ID of the session 
        /// that we want to determine whether or not it is in 
        /// the dependency chain of the current session.</param>
        /// <returns>A boolean indicating whether or not the current
        /// session is dependent on the specified session.</returns>
        public bool IsDependentUpon(int dependencySessionId)
        {
            bool result = false;
            var i = 0;

            if (this.Dependencies != null)
            {
                var dependencyArray = this.Dependencies.ToArray();
                while (i < this.Dependencies.Count())
                {
                    var dependency = dependencyArray[i];
                    if (dependency.Id == dependencySessionId || dependency.IsDependentUpon(dependencySessionId))
                        result = true;
                    i++;
                }
            }

            return result;
        }

        /// <summary>
        /// Add a dependent session to the list of dependencies for this session
        /// </summary>
        /// <param name="session2"></param>
        public void AddDependency(Session session2)
        {
            _dependentSessions.Add(session2);
        }

    }
}
