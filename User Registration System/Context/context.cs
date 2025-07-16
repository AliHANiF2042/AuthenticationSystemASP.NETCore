using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security;
using System.Xml.Linq;
using User_Registration_System.Models;

namespace User_Registration_System.Context
{
    public class context : DbContext
    {
        public context(DbContextOptions<context> options) : base(options)
        {

        }

        #region Entity
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region Tables
        public DbSet<User> Users { get; set; }
        #endregion

    }
}
