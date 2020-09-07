using M19G2.DAL.Entities;
using M19G2.DAL.Persistence;
using System;
using System.Data;
using System.Data.Entity;

namespace M19G2.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly M19G2Context _context;

        private DbContextTransaction _transaction;

        public UnitOfWork()
        {
            _context = new M19G2Context();
            //UserRepository = new UserRepository(_context);
        }

        private bool _disposed;

        #region AspNetRoles Repository

        private BaseRepository<AspNetRole> _aspNetRolesRepository;

        public BaseRepository<AspNetRole> AspNetRolesRepository =>
            _aspNetRolesRepository ?? (_aspNetRolesRepository = RepositoryFactory.CreateRepository<AspNetRole>(_context));

        #endregion

        //#region Users Repository

        //public readonly UserRepository UserRepository;

        //#endregion

        #region AspNetUsers Repository
        private BaseRepository<AspNetUser> _aspNetUsersRepository;
        public BaseRepository<AspNetUser> AspNetUsersRepository =>
            _aspNetUsersRepository ?? (_aspNetUsersRepository = RepositoryFactory.CreateRepository<AspNetUser>(_context));
        #endregion

        #region AspNetUserClaims Repository

        private BaseRepository<AspNetUserClaim> _aspNetUserClaimsRepository;

        public BaseRepository<AspNetUserClaim> AspNetUserClaimsRepository =>
            _aspNetUserClaimsRepository ?? (_aspNetUserClaimsRepository = RepositoryFactory.CreateRepository<AspNetUserClaim>(_context));

        #endregion

        #region AspNetUserLogins Repository

        private BaseRepository<AspNetUserLogin> _aspNetUserLoginsRepository;

        public BaseRepository<AspNetUserLogin> AspNetUserLoginsRepository =>
            _aspNetUserLoginsRepository ?? (_aspNetUserLoginsRepository = RepositoryFactory.CreateRepository<AspNetUserLogin>(_context));

        #endregion

        #region Dishes Repository

        private BaseRepository<Dish> dRepo;
        public BaseRepository<Dish> DishesRepository =>
            dRepo ?? (dRepo = RepositoryFactory.CreateRepository<Dish>(_context));

        #endregion

        #region Ingredients Repository

        private BaseRepository<Ingredient> iRepo;

        public BaseRepository<Ingredient> IngredientsRepository =>
            iRepo ?? (iRepo = RepositoryFactory.CreateRepository<Ingredient>(_context));

        #endregion

        #region Dish Type

        private BaseRepository<DishType> dishTypesRepo;

        public BaseRepository<DishType> DishTypeRepository =>
            dishTypesRepo ?? (dishTypesRepo = RepositoryFactory.CreateRepository<DishType>(_context));

        #endregion

        #region Delivery/Orders Repository

        private BaseRepository<Order> ordersRepo;

        public BaseRepository<Order> OrdersRepository =>
            ordersRepo ?? (ordersRepo = RepositoryFactory.CreateRepository<Order>(_context));

        #endregion

        #region UserAddresses

        private BaseRepository<UserAddress> addressesRepo;

        public BaseRepository<UserAddress> UserAddressesRepository =>
            addressesRepo ?? (addressesRepo = RepositoryFactory.CreateRepository<UserAddress>(_context));

        #endregion

        #region Images

        private BaseRepository<Image> imgRepo;

        public BaseRepository<Image> ImagesRepository =>
            imgRepo ?? (imgRepo = RepositoryFactory.CreateRepository<Image>(_context));
        
        #endregion

        #region Log4NetRepo
        private BaseRepository<Log4NetLog> log4NetRepo;

        public BaseRepository<Log4NetLog> Log4NetRepository =>
            log4NetRepo ?? (log4NetRepo = RepositoryFactory.CreateRepository<Log4NetLog>(_context));
        #endregion

        #region UserAccessRequestRepo
        private BaseRepository<UsersAccessRequest> userAccessRequestRepo;

        public BaseRepository<UsersAccessRequest> UserAccessRequestsRepository =>
            userAccessRequestRepo ?? (userAccessRequestRepo = RepositoryFactory.CreateRepository<UsersAccessRequest>(_context));
        #endregion

        #region AccessRequestStatusRepo
        private BaseRepository<AccessRequestStatus> accessRequestStatusRepo;

        public BaseRepository<AccessRequestStatus> AccessRequestStatusRepository =>
            accessRequestStatusRepo ?? (accessRequestStatusRepo = RepositoryFactory.CreateRepository<AccessRequestStatus>(_context));
        #endregion

        private BaseRepository<OrderQuantity> ocpRepo;
        public BaseRepository<OrderQuantity> OrderQuantitiesRepository =>
            ocpRepo ?? (ocpRepo = RepositoryFactory.CreateRepository<OrderQuantity>(_context));

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void CommitTransaction()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _transaction?.Rollback();
            }
            catch
            {
                //Do nothing
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
