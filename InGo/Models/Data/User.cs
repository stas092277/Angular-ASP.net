using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using InGo.Identity;
using InGo.Models.Data.Links;
using InGo.Models.Links;

namespace InGo.Models
{
    public class User
    {
        public int Id { get; set; }

        public string IdentityId { get; set; }

        public UserIdentity Identity { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual string Type { get; set; }

        public string Email { get; set; }

        public string About { get; set; }

        public string ImgUrl { get; set; }

        public bool IsDeleted { get; set; }


        public virtual Department Department { get; set; }
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Badge> Badges { get; set; } = new List<Badge>();
        public virtual ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();

        public virtual ICollection<UserPostSave> UserPosts { get; set; } = new List<UserPostSave>();

        [NotMapped]
        public object ViewModel => new
        {
            Id,
            FirstName,
            LastName,
            Type,
            About,
            Email,
            ImgUrl,
            Departmen = Department == null ? null :  Department.Name,
            SavedPosts = UserPosts.Select(up => up.Post.ExternalViewModel).ToList(),
        };


    }

    public static class UserType
    {
        public const string Intern = "Стажёр";
        public const string Mentor = "Ментор";
        public const string Moderator = "Модератор";
        public const string Admin = "Администратор";
    }
}
