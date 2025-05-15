using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{

    public partial class Context : DbContext
    {
        public Context() { }
        public Context(DbContextOptions<Context> options) : base(options) { }

        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<ContactTag> ContactsTag { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Contact
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Phone);
                entity.Property(e => e.Email);
                entity.Property(e => e.Address);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
            });

            // Tag
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Color);
            });

            // ContactTag
            modelBuilder.Entity<ContactTag>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne<Contact>()
                      .WithMany()
                      .HasForeignKey(e => e.ContactId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Tag>()
                      .WithMany()
                      .HasForeignKey(e => e.TagId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.Password).IsRequired();
            });
        }
    }
}