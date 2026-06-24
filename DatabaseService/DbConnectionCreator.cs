using System.Data;

namespace DatabaseService
{
    public abstract class DbConnectionCreator
    {
        public abstract Task<IDbConnection> CreateAsync();
    }
}
