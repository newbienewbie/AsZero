using AsZero.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace AsZero.DbContexts
{
    public class AsZeroDbContext : DbContext
    {
        public AsZeroDbContext(DbContextOptions<AsZeroDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedDatabase(modelBuilder);
        }

        private static void SeedDatabase(ModelBuilder modelBuilder)
        {
        }


        #region 用户相关
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        #endregion

    }
}
