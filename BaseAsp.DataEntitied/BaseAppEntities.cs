using BaseAsp.DataEntitied.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAsp.DataEntitied
{
    public class BaseAppEntities : DbContext, IDbContext
    {
        public BaseAppEntities()
                : base("name=CurrentEntities")
        {
            OnInitialized();
        }

        public DbSet<AccountType> AccountTypes { get; set; }

        public void Rollback()
        {
            ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }

        void OnInitialized()
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 600;
        }
    }
}
