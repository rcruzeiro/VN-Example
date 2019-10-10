using System;
using System.Collections.Generic;
using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using Couchbase.Core;

namespace VN.Example.Infrastructure.Database.Couchbase
{
    public abstract class BaseRepository : IDisposable
    {
        protected readonly IBucket Bucket;

        protected BaseRepository(CouchbaseOptions options)
        {
            var cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(options.ConnectionString) }
            });
            var authenticator = new PasswordAuthenticator(options.Username, options.Password);

            cluster.Authenticate(authenticator);

            Bucket = cluster.OpenBucket(options.Bucket);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Bucket != null) Bucket.Dispose();
            }
        }
    }
}
