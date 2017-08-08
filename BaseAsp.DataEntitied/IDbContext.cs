using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BaseAsp.DataEntitied
{
    public interface IDbContext
    {
        DbChangeTracker ChangeTracker { get; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        // IQueryable<TEntity> Find<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
        void Rollback();
    }
}