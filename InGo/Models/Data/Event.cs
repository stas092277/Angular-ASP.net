using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InGo.Models.Links;

namespace InGo.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public virtual EventCategory EventCategory { get; set; }
        public int EventCategoryId { get; set; }
        public virtual ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();
    }
}