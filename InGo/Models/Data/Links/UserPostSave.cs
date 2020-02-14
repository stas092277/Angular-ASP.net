using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models.Data.Links
{
    public class UserPostSave
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public bool IsDeleted { get; set; }


        public Post Post { get; set; }

        public User User { get; set; }
    }
}
