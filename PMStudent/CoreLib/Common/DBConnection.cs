using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Common
{
    public static class DBConnection
    {
        public static SqlConnection GetSqlConnection(string sqlCon)
        {
            var sqlConn = new SqlConnection(sqlCon);

            if (sqlConn.State == System.Data.ConnectionState.Closed)
            {
                sqlConn.Open();
            }
            else
            {
                sqlConn.Close();
            }

            return sqlConn;
        }
    }
}
