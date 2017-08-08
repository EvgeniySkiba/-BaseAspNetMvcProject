using BaseAsp.DAL.Abstract;
using BaseAsp.DataEntitied;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseAsp.DAL.Concrete
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly Lazy<Guid> _guid = new Lazy<Guid>(() => Guid.NewGuid());

        protected DbContext DbContext;
        protected DbSet<TEntity> DbSet;

        protected ObjectContext ObjectContext;
        protected ObjectSet<TEntity> ObjectSet;

        public GenericRepository(IDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "Context cannot be null");
            }

            DbContext = context as DbContext;

            if (DbContext != null)
            {
                DbContext.Configuration.LazyLoadingEnabled = false;

                DbSet = DbContext.Set<TEntity>();

                ObjectSet = CoreObjectContext.CreateObjectSet<TEntity>();
            }
        }

        public bool NoTracking { get; set; }

        public ObjectContext CoreObjectContext
        {
            get { return ((IObjectContextAdapter)DbContext).ObjectContext; }
        }

        public TEntity Add(TEntity entity)
        {
            ObjectSet.AddObject(entity);
            Save();
            return entity;
        }

        public void AddWithoutSave(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Attach(TEntity entity)
        {
            ObjectSet.Attach(entity);
        }

        public void ChangeState(TEntity entity, EntityState state)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            if (entity != null)
            {
                DbContext.Entry(entity).State = EntityState.Deleted;
                Save();
            }
        }

        public void DeleteMany(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                if (entity != null)
                {
                    DbContext.Entry(entity).State = EntityState.Deleted;
                }
            Save();
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return ObjectSet.Any(predicate);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool overWriteFromDb = true)
        {
            IQueryable<TEntity> retVal;

            MergeOption currentMergeOption = ObjectSet.MergeOption;
            if (overWriteFromDb)
            {
                // Pull back from DB and overwrite what's currently in the context's cache...
                ObjectSet.MergeOption = MergeOption.OverwriteChanges;
            }

            if (NoTracking)
            {
                retVal = ObjectSet.AsNoTracking().Where(predicate);
            }
            else
            {
                retVal = ObjectSet.Where(predicate);
            }

            if (overWriteFromDb)
            {
                // Reset to previous
                ObjectSet.MergeOption = currentMergeOption;
            }
            return retVal;
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return ObjectSet.First(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            IEnumerable<TEntity> retVal = DbSet.AsEnumerable();
            return retVal;
        }

        public DbContext GetContext()
        {
            return DbContext;
        }

        public DbSet<TEntity> GetDbSet()
        {
            return DbSet;
        }

        public IQueryable<TEntity> GetQuery()
        {
            return ObjectSet.AsQueryable();
        }

        public TEntity Refresh(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Refresh(TEntity entity)
        {
            DbContext.Entry(entity).Reload();
        }

        public void Reset()
        {
            IEnumerable<ObjectStateEntry> objectStateEntries =
                  CoreObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Deleted |
                                                                             EntityState.Modified | EntityState.Unchanged);
            foreach (ObjectStateEntry objectStateEntry in objectStateEntries)
                CoreObjectContext.Detach(objectStateEntry.Entity);
        }

        public void Save()
        {
            try
            {
                DbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
                {
                    // Logger.Error(
                    //     string.Format(
                    //         "{0} Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", mn,
                    //        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    //foreach (DbValidationError ve in eve.ValidationErrors)
                    //    Logger.Error(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName,
                    //         ve.ErrorMessage));
                }
                throw;
            }
        }

        public TEntity Save(TEntity entity)
        {
            DbEntityEntry<TEntity> entry = DbContext.Entry(entity);
            if (entry != null && DbContext.ChangeTracker.Entries().Any(e => e.Equals(entry)))
            {
                DbContext.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                ObjectSet.AddObject(entity);
            }
            Save();
            return entity;
        }

        public void SaveMany(IEnumerable<TEntity> entities)
        {
            DbContext.ChangeTracker.DetectChanges();
            DbContext.Configuration.AutoDetectChangesEnabled = false;
            foreach (TEntity entity in entities)
            {
                // First check the entity is already tracked in the context...
                DbEntityEntry<TEntity> entry = DbContext.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    DbSet.Add(entity);
                }
            }

            Save();
            DbContext.Configuration.AutoDetectChangesEnabled = true;
        }
    }
}
