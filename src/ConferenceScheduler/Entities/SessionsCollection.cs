﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceScheduler.Entities
{
    /// <summary>
    /// Holds the collection of sessions for the conference
    /// </summary>
    public class SessionsCollection : List<Session>
    {
        ///// <summary>
        ///// Holds the list of presenters for all of the sessions
        ///// </summary>
        //public ICollection<Presenter> Presenters { get; private set; }

        /// <summary>
        /// Create an instance of the collection
        /// </summary>
        public SessionsCollection() { }

        /// <summary>
        /// Create an instance of the collection from an existing collection
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "sessions")]
        public SessionsCollection(IEnumerable<Session> sessions)
        {
            if (sessions != null && sessions.Any())
                foreach (var session in sessions)
                    this.Add(session);
        }

        /// <summary>
        /// Add a new session to the collection
        /// </summary>
        /// <param name="id">The unique identifier of the session</param>
        /// <param name="topicId">The unique identifier of the topic (track) of the session</param>
        /// <param name="presenters">The collection of people who will be presenting the session</param>
        /// <returns>The session object that was added to the collection</returns>
        public Session Add(int id, int? topicId, params Presenter[] presenters)
        {
            return this.Add(id, string.Empty, topicId, presenters);
        }

        /// <summary>
        /// Add a new session to the collection
        /// </summary>
        /// <param name="id">The unique identifier of the session</param>
        /// <param name="name">The title of the session (used for display)</param>
        /// <param name="topicId">The unique identifier of the topic (track) of the session</param>
        /// <param name="presenters">The collection of people who will be presenting the session</param>
        /// <returns>The session object that was added to the collection</returns>
        public Session Add(int id, string name, int? topicId, params Presenter[] presenters)
        {
            var session = CreateSession(id, name, topicId, presenters);
            this.Add(session);
            return session;
        }

        ///// <summary>
        ///// Adds a new session to the collection
        ///// </summary>
        ///// <param name="id">The unique identifier of the session</param>
        ///// <param name="presenterId">The unique identifier of the presenter of the session</param>
        ///// <param name="presenterUnavailableForTimeslots">A list of timeslot Ids during which the presenter cannot present.</param>
        ///// <returns>The session object that was added to the collection</returns>
        //public Session Add(int id, int presenterId, params int[] presenterUnavailableForTimeslots)
        //{
        //    var session = CreateSession(id, GetPresenters(presenterId, presenterUnavailableForTimeslots));
        //    this.Add(session);
        //    return session;
        //}

        ///// <summary>
        ///// Adds a new session to the collection
        ///// </summary>
        ///// <param name="id">The unique identifier of the session</param>
        ///// <param name="presenterId">The unique identifier of the presenter of the session</param>
        ///// <param name="topicId">The unique identifier of the topic (track) of the session</param>
        ///// <param name="presenterUnavailableForTimeslots">A list of timeslot Ids during which the presenter cannot present.</param>
        ///// <returns>The session object that was added to the collection</returns>
        //public Session Add(int id, int presenterId, int topicId, params int[] presenterUnavailableForTimeslots)
        //{
        //    var session = CreateSession(id, topicId, GetPresenters(presenterId, presenterUnavailableForTimeslots));
        //    this.Add(session);
        //    return session;
        //}

        // TODO: Document
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name">The title of the session (used for display)</param>
        /// <param name="topicId"></param>
        /// <param name="presenters"></param>
        /// <returns></returns>
        public static Session CreateSession(int id, string name, int? topicId, IEnumerable<Presenter> presenters)
        {
            return new Session()
            {
                Id = id,
                Name = name,
                TopicId = topicId,
                Presenters = presenters
            };
        }

        // TODO: Document
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HaveCircularDependencies()
        {
            bool result = false;
            int i = 0;
            var sessionsArray = this.ToArray();

            while (!result && (i < this.Count()))
            {
                var session = sessionsArray[i];
                if (session.IsDependentUpon(session.Id))
                    result = true;
                i++;
            }

            return result;
        }

        // TODO: Document
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IEnumerable<Presenter> GetPresenters()
        {
            return this.SelectMany(s => s.Presenters).Distinct();
        }

        //internal IEnumerable<Presenter> GetPresenters(int id, IEnumerable<Presenter> presenters)
        //{
        //    var results = new List<Presenter>();

        //    foreach (var presenter in presenters)
        //    {
        //        if (this.Presenters.Any(p => p.Id == id))
        //        {
        //            var existingPresenter = this.Presenters.Where(p => p.Id == id).Single();
        //            results.Add(existingPresenter);
        //        }
        //        else
        //        {
        //            this.Presenters.Add(presenter);
        //            results.Add(presenter);
        //        }
        //    }

        //    return results;
        //}

        //private IEnumerable<Presenter> GetPresenters(int id, params int[] presenterUnavailableForTimeslots)
        //{
        //    var presenter = new Presenter()
        //        {
        //            Id = id,
        //            UnavailableForTimeslots = presenterUnavailableForTimeslots.AsEnumerable()
        //        };

        //    return GetPresenters(id, new List<Presenter>() { presenter });
        //}

    }
}
