using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using M19G2.DAL.Persistence;

namespace M19G2.DAL
{
    public class BaseRepository<TEntity> : GenericRepository<TEntity> where TEntity : class, new()
    {
        public BaseRepository(M19G2Context context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<TEntity>();
        }

        public override IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public override TEntity GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        public override TEntity GetByIDs(object[] ids)
        {
            return _dbSet.Find(ids);
        }

        public override TEntity Insert(TEntity entity)
        {
            return _dbSet.Add(entity);
        }

        public override void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public override void Delete(TEntity entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public override void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            var entry = _dbContext.Entry(entityToUpdate);
            entry.State = EntityState.Modified;

            //_dbSet.Attach(entityToUpdate);
            //_dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public override void Attach(TEntity entity)
        {
            _dbSet.Attach(entity);
        }
    }
}
