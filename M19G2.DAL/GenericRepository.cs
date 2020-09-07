using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using M19G2.DAL.Persistence;

namespace M19G2.DAL
{
    public abstract class GenericRepository<TEntity> where TEntity : class
    {
        internal M19G2Context _dbContext { get; set; }

        internal DbSet<TEntity> _dbSet;

        public abstract IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        public abstract TEntity GetByID(object id);

        public abstract TEntity GetByIDs(object[] ids);

        public abstract TEntity Insert(TEntity entity);

        public abstract void Delete(object id);

        public abstract void Delete(TEntity entityToDelete);

        public abstract void Update(TEntity entityToUpdate);

        public abstract void Attach(TEntity entity);
    }
}
