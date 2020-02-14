using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models
{
    public class EventCategory
    {
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
