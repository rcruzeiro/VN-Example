using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace VN.Example.Infrastructure.Database.Couchbase
{
    public class CouchbaseOptions
    {
        protected string SectionName { get; }

        public string ConnectionString { get; protected set; }

        public string Host { get; protected set; }

        public string Port { get; protected set; }

        public string Bucket { get; protected set; }

        public string Username { get; protected set; }

        public string Password { get; protected set; }

        public CouchbaseOptions(string host,
                                string bucket,
                                string port = "8091",
                                string username = null,
                                string password = null)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
            Port = port;
            Bucket = bucket ?? throw new ArgumentNullException(nameof(bucket));
            Username = username;
            Password = password;

            CreateConnectionString();
        }

        public CouchbaseOptions(IConfiguration configuration, string sectionName = "CouchbaseOptions")
        {
            SectionName = sectionName;

            new ConfigureFromConfigurationOptions<CouchbaseOptions>(
                configuration.GetSection(SectionName)).Configure(this);

            CreateConnectionString();
        }

        private void CreateConnectionString()
        {
            ConnectionString = $"http://{Host}:{Port}/pools";
        }
    }
}
