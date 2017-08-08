using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseAsp.DAL
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        DbContext GetContext();
        DbSet<TEntity> GetDbSet();
        TEntity First(Expression<Func<TEntity, bool>> predicate);
        TEntity Add(TEntity entity);
        void AddWithoutSave(TEntity entity);
        void Attach(TEntity entity);
        void Delete(TEntity entity);
        void DeleteMany(IEnumerable<TEntity> entities);
        TEntity Save(TEntity entity);
        void Save();
        IQueryable<TEntity> GetQuery();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool overWriteFromDb = true);
        bool Exists(Expression<Func<TEntity, bool>> predicate);

        void ChangeState(TEntity entity, EntityState state);

        void Reset();
        void SaveMany(IEnumerable<TEntity> entities);

    }
}
