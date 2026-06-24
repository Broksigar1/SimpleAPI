namespace DatabaseService.Repositories
{
    public abstract class Repository
    {
        protected readonly DbConnectionCreator _connectionCreator;
        
        protected Repository(DbConnectionCreator connectionCreator)
        {
            _connectionCreator = connectionCreator;
        }
    }
}
