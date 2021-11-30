using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleCashier.DB
{
    static class ConnectionManager
    {
        public static string GetConnectionString(string connectionStringName = "DefaultConnection",
            string environmentVariableName = "DefaultConnection")
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();
            string userId = "", password = "";
            config.Providers.Any(p => p.TryGet("Db:UserId", out userId));
            config.Providers.Any(p => p.TryGet("Db:Password", out password));
            return string.Format(
                config.GetConnectionString(connectionStringName),
                userId, password
            ) ?? Environment.GetEnvironmentVariable(environmentVariableName);
        }
    }
}
