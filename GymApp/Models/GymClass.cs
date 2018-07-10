using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class GymClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime => StartTime + Duration;
        public string Description { get; set; }

        //navigational properties
        public virtual ICollection<ApplicationUser> AttendingMembers { get; set; }
    }
}