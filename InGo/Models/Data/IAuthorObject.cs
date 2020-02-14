using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models.Data
{
    public interface IAuthorEntity
    {
        int? AuthorId { get; set; }

        User Author { get; set; }
    }
}
