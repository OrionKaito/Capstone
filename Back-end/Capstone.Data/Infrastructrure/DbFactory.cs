namespace Capstone.Data.Infrastructrure
{
    public class DbFactory : Disposable, IDbFactory
    {
        CapstoneEntities dbContext;

        public CapstoneEntities Init()
        {
            return dbContext ?? (dbContext = new CapstoneEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
