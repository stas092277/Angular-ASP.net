using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models
{
    public class Like
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(16)")]
        public LikeType Type { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        // Entity that is liked.
        public int EntityId { get; set; }

        [ForeignKey("EntityId")]
        public virtual Post Post { get; set; }

        [ForeignKey("EntityId")]
        public virtual Comment Comment { get; set; }

        public bool IsDeleted { get; set; }
    }

    public enum LikeType
    {
        Post,
        Comment
    }
}
