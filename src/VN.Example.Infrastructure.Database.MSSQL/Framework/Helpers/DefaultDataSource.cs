using System;
using Microsoft.Extensions.Configuration;

namespace VN.Example.Infrastructure.Database.MSSQL
{
    public sealed class DefaultDataSource : IDataSource
    {
        readonly string _connstring;

        public DefaultDataSource(string connectionString)
        {
            _connstring = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public DefaultDataSource(IConfiguration configuration, string connectionStringName)
            : this(configuration.GetConnectionString(connectionStringName))
        { }

        public string GetConnectionString()
        {
            return _connstring;
        }
    }
}
