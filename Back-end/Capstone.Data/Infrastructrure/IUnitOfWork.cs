namespace Capstone.Data.Infrastructrure
{
    public interface IUnitOfWork
    {
        void Commit();
        void BeginTransaction();
        void RollBack();
        void CommitTransaction();
    }
}
