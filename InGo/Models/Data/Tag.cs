using InGo.Models.Links;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public virtual ICollection<TagPost> TagPosts { get; set; } = new List<TagPost>();


        public string Name { get; set; }

        public bool IsDeleted { get; set; }


        [NotMapped]
        [JsonIgnore]
        public object ViewModel => new
        {
            Id,
            Posts = TagPosts.Select(tp => tp.Post.ExternalViewModel),
            Name,
        };

        [NotMapped]
        [JsonIgnore]
        public object EmptyViewModel => new
        {
            Id,
            Name
        };
    }
}
