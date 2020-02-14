using InGo.Models.Links;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace InGo.Models
{
    public class Badge
    {
        public int Id { get; set; }

        public string PostDescription { get; set; }

        public string UserDescription { get; set; }

        public string ImgUrl { get; set; }

        public bool IsDeleted { get; set; }

        [JsonIgnore]
        public virtual ICollection<BadgePost> BadgePosts { get; set; } = new List<BadgePost>();
    }
}
