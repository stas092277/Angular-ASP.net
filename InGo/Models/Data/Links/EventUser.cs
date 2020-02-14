using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models.Links
{
    public class EventUser
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
    
        public bool IsDeleted { get; set; }
    }
}
