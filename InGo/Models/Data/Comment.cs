using InGo.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models
{
    public class Comment : IAuthorEntity
    {

        public int Id { get; set; }

        public DateTime PublishDate { get; set; }
        public string Content { get; set; }
        public virtual Post Post { get; set; }
        public int? AuthorId { get; set; }
        public virtual User Author { get; set; }
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

        public bool IsDeleted { get; set; }


        [NotMapped]
        public object ViewModel => new
        {
            Id,
            PublishDate,
            Content,
            PostId = Post == null ? 0 : Post.Id,
            Author = Author == null ? null : Author.ViewModel,
            LikesCount = Likes.Count,
        };
    }
}
