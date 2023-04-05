using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Common
{
    public class Helper
    {
        private IConfiguration _config;

        public Helper(IConfiguration config)
        {
            _config = config;
        }

        public string GetConnectionString(string connectionString)
        {
            string conn = _config.GetConnectionString(connectionString);
            return conn;
        }
    }
}
