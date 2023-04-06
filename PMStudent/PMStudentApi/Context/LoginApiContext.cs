using CoreLib.DTO;
using PMStudentApi.Interfaces;
using PMStudentApi.Model;
using System.Data.Common;
using System.Data;
using CoreLib.Common;
using System.Data.SqlClient;

namespace PMStudentApi.Context
{
    public class LoginApiContext : ILogin
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        /// <summary>
        /// Hàm này sử dụng để kiểm tra login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public CResponseMessage CheckLogin(LoginApiModel login)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                CResponseMessage resMess = new CResponseMessage();

                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("SV_SP_CheckLogin", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@taikhoan", ((object)login.TaiKhoan) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@matkhau", ((object)login.MatKhau) ?? DBNull.Value);

                cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 100);
                cmd.Parameters["@Code"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 100);
                cmd.Parameters["@Message"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Data", SqlDbType.NVarChar, 100);
                cmd.Parameters["@Data"].Direction = ParameterDirection.Output;

                var reader = cmd.ExecuteNonQuery();
                resMess.Code = Convert.ToInt32(cmd.Parameters["@Code"].Value);
                resMess.Message = Convert.ToString(cmd.Parameters["@Message"].Value);
                resMess.Data = Convert.ToString(cmd.Parameters["@Data"].Value);

                return resMess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
