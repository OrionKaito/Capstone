using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Capstone.Data.Infrastructrure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private CapstoneEntities dbContext;
        private IDbContextTransaction transaction;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public CapstoneEntities DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }

        public void Commit()
        {
            try
            {
                DbContext.Commit();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void BeginTransaction()
        {
            transaction = DbContext.Database.BeginTransaction();
        }

        public void RollBack()
        {
            transaction.Rollback();
            transaction.Dispose();
        }

        public void CommitTransaction()
        {
            transaction.Commit();
        }
    }
}
