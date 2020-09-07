using M19G2.DAL.Persistence;

namespace M19G2.DAL
{
    public static class RepositoryFactory
    {
        public static BaseRepository<TEntity> CreateRepository<TEntity>(M19G2Context dbContext)
            where TEntity : class, new() =>
            new BaseRepository<TEntity>(dbContext);
    }
}
