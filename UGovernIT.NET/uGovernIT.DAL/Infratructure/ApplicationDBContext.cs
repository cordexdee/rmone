using System;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DAL
{
    public interface IApplicationDbContext
    {

    }

    public class CustomDbContext : IDisposable, IApplicationDbContext
    {
        public UserProfile CurrentUser { get; protected set; }
        public string TenantID { get; set; }
        public string TenantAccountId { get; set; }
        public string Database { get; }
        public string DatabaseCommon { get; }

        public static CustomDbContext Create(string dbConnection)
        {
            string dbUrl = string.Empty;
            if (!string.IsNullOrWhiteSpace(dbConnection))
            {
                dbUrl = dbConnection;
            }

            return new CustomDbContext(dbUrl);
        }

        public CustomDbContext(string database, string commonDatabase = null)
        {
            this.Database = database;
            this.DatabaseCommon = commonDatabase;
        }

        public void Dispose()
        {
        }
    }
}
