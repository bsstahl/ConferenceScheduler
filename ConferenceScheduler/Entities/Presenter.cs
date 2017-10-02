using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceScheduler.Entities
{
    /// <summary>
    /// Represents an instance of a person who is presenting one or more sessions
    /// </summary>
    public class Presenter
    {
        /// <summary>
        /// The primary-key identifier of the object
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name (for display) of the presenter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of timeslot Ids during which the presenter would prefer to present
        /// </summary>
        public IEnumerable<int> PreferredTimeslots { get; set; }

        /// <summary>
        /// The list of timeslot Ids during which the person cannot present
        /// </summary>
        public IEnumerable<int> UnavailableForTimeslots { get; set; }

        /// <summary>
        /// Create an instance of the object based on the supplied data
        /// </summary>
        /// <param name="id">The unique Id of the presenter</param>
        /// <returns>A Presenter object</returns>
        public static Presenter Create(int id)
        {
            return Presenter.Create(id, string.Empty, new int[] { });
        }

        /// <summary>
        /// Create an instance of the object based on the supplied data
        /// </summary>
        /// <param name="id">The unique Id of the presenter</param>
        /// <param name="presenterUnavailableForTimeslots">The list of timeslot Ids during which the person cannot present</param>
        /// <returns>A Presenter object</returns>
        public static Presenter Create(int id, int[] presenterUnavailableForTimeslots)
        {
            return Presenter.Create(id, string.Empty, presenterUnavailableForTimeslots);
        }

        /// <summary>
        /// Create an instance of the object based on the supplied data
        /// </summary>
        /// <param name="id">The unique Id of the presenter</param>
        /// <param name="name">The display name of the presenter</param>
        public static Presenter Create(int id, string name)
        {
            return Presenter.Create(id, name, new int[] { }, new int[] { });
        }

        /// <summary>
        /// Create an instance of the object based on the supplied data
        /// </summary>
        /// <param name="id">The unique Id of the presenter</param>
        /// <param name="name">The display name of the presenter</param>
        /// <param name="presenterUnavailableForTimeslots">The list of timeslot Ids during which the person cannot present</param>
        public static Presenter Create(int id, string name, int[] presenterUnavailableForTimeslots)
        {
            return Presenter.Create(id, name, presenterUnavailableForTimeslots, new int[] { });
        }

        /// <summary>
        /// Create an instance of the object based on the supplied data
        /// </summary>
        /// <param name="id">The unique Id of the presenter</param>
        /// <param name="name">The display name of the presenter</param>
        /// <param name="presenterPreferredTimeslots">The list of timeslot Ids the presenter would prefer to present in</param>
        /// <param name="presenterUnavailableForTimeslots">The list of timeslot Ids during which the person cannot present</param>
        /// <returns>A Presenter object</returns>
        public static Presenter Create(int id, string name, int[] presenterUnavailableForTimeslots, int[] presenterPreferredTimeslots)
        {
            return new Presenter()
            {
                Id = id,
                Name = name,
                PreferredTimeslots = presenterPreferredTimeslots,
                UnavailableForTimeslots = presenterUnavailableForTimeslots.AsEnumerable()
            };
        }

        /// <summary>
        /// Returns a display serialization of the object
        /// </summary>
        /// <returns>A string for display</returns>
        public override string ToString()
        {
            string result;
            if (string.IsNullOrWhiteSpace(this.Name))
                result = $"Presenter {this.Id.ToString(System.Globalization.CultureInfo.CurrentCulture)}";
            else
                result = $"{this.Name} ({this.Id.ToString(System.Globalization.CultureInfo.CurrentCulture)})";
            return result;
        }
    }
}
