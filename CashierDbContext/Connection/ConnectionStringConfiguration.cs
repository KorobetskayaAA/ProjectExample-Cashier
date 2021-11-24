using CashierDB.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace CashierDB.Connection
{
    class ConnectionStringConfiguration
    {
        public string ConnectionString { get; set; }

        public ConnectionStringConfiguration(string appsettingsPath = "appsettings.json",
            string connectionStringName = "DefaultConnection",
            string environmentVaiableName = "CashierDb_ConnectionString",
            string userSecretsSection = "CashierDb")
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appsettingsPath, optional: true)
                .AddUserSecrets<CashierContext>()
                .Build();

            string userId = "", password = "", server = "";
            config.Providers.Any(p => p.TryGet($"{userSecretsSection}:UserId", out userId));
            config.Providers.Any(p => p.TryGet($"{userSecretsSection}:Password", out password));
            config.Providers.Any(p => p.TryGet($"{userSecretsSection}:Server", out server));

            ConnectionString = string.Format(config.GetConnectionString(connectionStringName)
                ?? Environment.GetEnvironmentVariable(environmentVaiableName),
                server,
                userId,
                password
            );
        }
    }
}
