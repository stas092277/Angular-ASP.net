using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models
{
    public class Department
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();

        [NotMapped]
        public object ViewModel => new
        {
            Id,
            Name
        };


        [NotMapped]
        public object UserViewModel => new
        {
            Id,
            Name,
            Users = Users.Select(u => u.ViewModel)
        };
    }
}
