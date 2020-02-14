using InGo.Models.Data;
using InGo.Models.Data.Links;
using InGo.Models.Links;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace InGo.Models
{
    public class Post : IAuthorEntity
    {
        public int Id { get; set; }

        public int? AuthorId { get; set; }

        public virtual User Author { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

        public virtual ICollection<TagPost> TagPosts { get; set; } = new List<TagPost>();


        public virtual ICollection<BadgePost> BadgePosts { get; set; } = new List<BadgePost>();

        public virtual ICollection<UserPostSave> UserPosts { get; set; } = new List<UserPostSave>();

        public DateTime PublishDate { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsDeleted { get; set; }


        public object ViewModel(int userId) => new
        {
            Id,
            Author = Author?.ViewModel,
            CommentsCount = Comments.Count,
            LikesCount = Likes.Count,
            Tags = TagPosts.Select(tp => tp.Tag.EmptyViewModel),
            Badges = BadgePosts.Select(bp => bp.Badge),
            PublishDate,
            Title,
            Content,
            SavesCount = UserPosts.Count,
            Liked = Likes.Any(l => l.UserId == userId),
            Saved = UserPosts.Any(up => up.UserId == userId)
        };

        public object CommentsViewModel(int userId) => new
        {
            Id,
            Author = Author?.ViewModel,
            Comments = Comments.Select(c => c.ViewModel),
            LikesCount = Likes.Count,
            Tags = TagPosts.Select(tp => tp.Tag.EmptyViewModel),
            Badges = BadgePosts.Select(bp => bp.Badge),
            PublishDate,
            Title,
            Content,
            SavesCount = UserPosts.Count,
            Liked = Likes.Any(l => l.UserId == userId),
            Saved = UserPosts.Any(up => up.UserId == userId)
        };

        public object ExternalViewModel => new
        {
            Id,
            Author = Author?.ViewModel,
            CommentsCount = Comments.Count,
            LikesCount = Likes.Count,
            Tags = TagPosts.Select(tp => tp.Tag.EmptyViewModel),
            Badges = BadgePosts.Select(bp => bp.Badge),
            PublishDate,
            Title,
            Content,
            SavesCount = UserPosts.Count,
        };
    }
}
