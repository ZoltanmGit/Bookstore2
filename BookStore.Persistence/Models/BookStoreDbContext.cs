using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Persistence.Models
{
    public class BookStoreDbContext : IdentityDbContext<BaseUser, IdentityRole<int>, int>
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookVolume> BookVolumes { get; set; }
        public virtual DbSet<Lending> Lendings { get; set; }

        public BookStoreDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BaseUser>().ToTable("Users");
        }

    }
}
