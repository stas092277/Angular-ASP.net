using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models.Links
{
    public class BadgePost
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public int BadgeId { get; set; }

        public virtual Badge Badge { get; set; }

        public bool IsDeleted { get; set; }
    }
}
