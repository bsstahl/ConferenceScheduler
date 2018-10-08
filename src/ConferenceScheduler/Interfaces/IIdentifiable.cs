using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceScheduler.Interfaces
{
    /// <summary>
    /// Indicates that an object has an integer Identity property
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        /// Holds the identity of the object
        /// </summary>
        int Id { get; }
    }
}
