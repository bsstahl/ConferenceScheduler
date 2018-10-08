﻿using ConferenceScheduler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceScheduler.Entities
{
    /// <summary>
    /// Represents a duration of time during which a conference session can occur
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes")]
    public class Timeslot : IComparable<Timeslot>, IIdentifiable
    {
        /// <summary>
        /// The primary-key identifier of the object
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The 0-based index value of the conference day for multi-day conferences
        /// </summary>
        /// <remarks>Set the day to 0 for 1-day conferences</remarks>
        public int DayIndex { get; set; }

        /// <summary>
        /// The starting hour of the timeslot
        /// </summary>
        /// <example>A Session starting at 11:15 am would have a StartHour of 11.25</example>
        /// <example>A Session starting at 2:30 pm would have a StartHour of 14.5</example>
        public Single StartHour { get; set; }

        /// <summary>
        /// Compares this object to another of the same type
        /// </summary>
        /// <param name="other">The object to compare this one to</param>
        /// <returns>An integer</returns>
        public int CompareTo(Timeslot other)
        {
            int result = 1;
            if (other != null)
            {
                result = this.DayIndex.CompareTo(other.DayIndex);
                if (result == 0)
                    result = this.StartHour.CompareTo(other.StartHour);
            }
            return result;
        }

        /// <summary>
        /// Create an instance of the object
        /// </summary>
        /// <param name="id">The unique identifier of the timeslot</param>
        /// <returns>An object representing a duration of time during which 
        /// a conference session can occur</returns>
        public static Timeslot Create(int id)
        {
            return Create(id, 0);
        }

        /// <summary>
        /// Create an instance of the object
        /// </summary>
        /// <param name="id">The unique identifier of the timeslot</param>
        /// <param name="startHour">The starting hour of the timeslot</param>
        /// <returns>An object representing a duration of time during which 
        /// a conference session can occur</returns>
        public static Timeslot Create(int id, Single startHour)
        {
            return Create(id, startHour, 0);
        }

        /// <summary>
        /// Create an instance of the object
        /// </summary>
        /// <param name="id">The unique identifier of the timeslot</param>
        /// <param name="startHour">The starting hour of the timeslot</param>
        /// <returns>An object representing a duration of time during which 
        /// a conference session can occur</returns>
        public static Timeslot Create(int id, double startHour)
        {
            return Create(id, Convert.ToSingle(startHour), 0);
        }

        /// <summary>
        /// Create an instance of the object
        /// </summary>
        /// <param name="id">The unique identifier of the timeslot</param>
        /// <param name="startHour">The starting hour of the timeslot</param>
        /// <param name="dayIndex">The 0-based index value of the conference day 
        /// for multi-day conferences</param>
        /// <returns>An object representing a duration of time during which 
        /// a conference session can occur</returns>
        public static Timeslot Create(int id, Single startHour, int dayIndex)
        {
            return new Timeslot()
            {
                Id = id,
                StartHour = startHour,
                DayIndex = dayIndex
            };
        }

        /// <summary>
        /// Returns a hash code representing the object
        /// </summary>
        /// <returns>An integer hash of the object value</returns>
        /// <remarks>Omitting getHashCode violates FxCop rule OverrideGetHashCodeOnOverridingEquals.</remarks>
        public override int GetHashCode()
        {
            return Convert.ToInt32(this.StartHour * 60);
        }

        /// <summary>
        /// Tests for value equality of the specified object with this object
        /// </summary>
        /// <param name="obj">The object to compare this one to</param>
        /// <returns>An integer</returns>
        /// <remarks>Omitting Equals violates the FxCop rule OverrideMethodsOnComparableTypes.</remarks>
        public override bool Equals(object obj)
        {
            Timeslot other = obj as Timeslot; //avoid double casting 
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }
            return this.CompareTo(other) == 0;
        }

    }
}


