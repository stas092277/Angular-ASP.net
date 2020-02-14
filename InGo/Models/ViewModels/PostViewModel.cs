using InGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public virtual User Author { get; set; }

        public int CommentsCount { get; set; }

        public int LikesCount { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public virtual ICollection<Badge> Badges { get; set; } = new List<Badge>();

        public DateTime PublishDate { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsDeleted { get; set; }

        public PostViewModel(Post post)
        {

        }
    }
}
