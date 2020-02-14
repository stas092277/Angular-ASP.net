using Microsoft.EntityFrameworkCore;
using InGo.Models.Links;
using InGo.Models.Data.Links;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using InGo.Identity;
using InGo.Models.Identity;

namespace InGo.Models
{
    /// <summary>
    /// Provides access to database.
    /// </summary>
    public class IngoContext : IdentityDbContext<UserIdentity>
    {
        public virtual DbSet<User> UserProfiles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Badge> Badges { get; set; }

        public virtual DbSet<Like> Likes { get; set; }

        public virtual DbSet<Post> Posts { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public DbSet<Faq> Faqs { get; set; }

        public DbSet<FaqCategory> FaqCategories { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventCategory> EventCategories { get; set; }

        public DbSet<BadgePost> BadgePosts { get; set; }

        public DbSet<EventUser> EventUsers { get; set; }

        public DbSet<TagPost> TagPosts { get; set; }

        public DbSet<UserPostSave> UserPostSaves { get; set; }


        public IngoContext(DbContextOptions<IngoContext> optionsBuilder) : base(optionsBuilder)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<TagPost>().HasKey(tp => new { tp.TagId, tp.PostId });
            //modelBuilder.Entity<BadgePost>().HasKey(bp => new { bp.BadgeId, bp.PostId });
            //modelBuilder.Entity<EventUser>().HasKey(kp => new { kp.EventId, kp.UserId });
            //modelBuilder.Entity<UserPostSave>().HasKey(up => new { up.UserId, up.PostId });

            modelBuilder.Entity<Badge>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Comment>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Department>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Event>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EventCategory>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Faq>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<FaqCategory>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Like>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Post>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Tag>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<BadgePost>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EventUser>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<TagPost>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<UserPostSave>().HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Initialize();
            modelBuilder.InitializeId();
        }
    }
}

