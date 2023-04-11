using System.Data.Common;

namespace IdentityService.Repository.Connection
{
    public interface IConnectionFactory
    {
        DbConnection GetOpenConnection();
    }
}