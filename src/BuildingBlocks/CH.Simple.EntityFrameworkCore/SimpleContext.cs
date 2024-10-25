using CH.Simple.Entities;
using CH.Simple.Entities.BaseEntities;
using CH.Simple.Utils;
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


        #region 重写savechanges 插入时自动写入时间字段
        public override int SaveChanges()
        {
            SetSystemField();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            SetSystemField();
            return base.SaveChangesAsync();
        }

        /// <summary>
        /// 系字段赋值
        /// </summary>
        private void SetSystemField()
        {
            var now = DateTime.Now;
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity)
                {
                    var entity = (BaseEntity)item.Entity;
                    switch (item.State)
                    {
                        //增
                        case EntityState.Added:
                            if (string.IsNullOrEmpty( entity.Id))
                            {
                                entity.Id =PKManager.UUID();
                            }
                            entity.Created = now;
                            break;
                        //改
                        case EntityState.Modified:
                            entity.Modified = now;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion

       


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

        #region 表
        public virtual DbSet<User> Users { get; set; }
        #endregion
    }
}
