using System.Data.Common;

namespace IdentityService.Repositories.Connection
{
    public interface IConnectionFactory
    {
        DbConnection GetOpenConnection();
    }
}