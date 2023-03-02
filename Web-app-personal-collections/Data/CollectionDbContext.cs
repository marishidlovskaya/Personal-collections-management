using Microsoft.EntityFrameworkCore;
using Web_app_personal_collections.Data;
using Web_app_personal_collections.Models.Entities;

namespace Web_app_personal_collections.Data
{
    public class CollectionDbContext: DbContext
    {
        public CollectionDbContext(DbContextOptions<CollectionDbContext> options)
          : base(options)
        {
        }

        public DbSet<User> Users { get; set; } 
        public DbSet<Collection> Collections { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<CollectionConfiq> collectionConfiqs { get; set; }



    }
}
