using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models
{
    public class FaqCategory
    {
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
        public virtual ICollection<Faq> Faqs { get; set; } = new List<Faq>();
    }
}
