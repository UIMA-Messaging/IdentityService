using System.Data.Common;

namespace ContactApi.Repository.Connection
{
    public interface IConnectionFactory
    {
        DbConnection GetOpenConnection();
    }
}