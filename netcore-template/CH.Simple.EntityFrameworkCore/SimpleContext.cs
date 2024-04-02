using CH.Simple.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CH.Simple.EntityFrameworkCore
{
    public class SimpleContext: DbContext
    {
        public SimpleContext() { }
        public SimpleContext(DbContextOptions<SimpleContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => !string.IsNullOrEmpty(x.Namespace) && x.BaseType != null && x.Name.EndsWith("EntityTypeConfiguration"));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);

        }
    }
}
