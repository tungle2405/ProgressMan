using CoreLib.DTO;
using PMLecture.Interfaces;
using PMLecture.Models;
using System.Data.SqlClient;
using System.Data;
using CoreLib.Common;
using System.Collections.Generic;

namespace PMLecture.Context
{
    public class MonHocContext : IMonHoc
    {
        public List<MonHocViewModel> GetAllMonHoc()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<MonHocViewModel> monHocList = new List<MonHocViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAllMonHoc", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MonHocViewModel monHoc = new MonHocViewModel();
                    monHoc.MaMonHoc = reader["MaMonHoc"].ToString().Trim();
                    monHoc.TenMonHoc = reader["TenMonHoc"].ToString().Trim();
                    monHoc.TongSoTiet = reader["TongSoTiet"].ToString().Trim();
                    monHoc.SoTietLyThuyet = reader["SoTietLyThuyet"].ToString().Trim();
                    monHoc.SoTietThucHanh = reader["SoTietThucHanh"].ToString().Trim();
                    monHoc.TrongSoBaiKT1 = Convert.ToInt32(reader["TrongSoBaiKT1"] == DBNull.Value ? -1 : reader["TrongSoBaiKT1"]);
                    monHoc.TrongSoBaiKT2 = Convert.ToInt32(reader["TrongSoBaiKT2"] == DBNull.Value ? -1 : reader["TrongSoBaiKT2"]);
                    monHoc.TrongSoBaiKT3 = Convert.ToInt32(reader["TrongSoBaiKT3"] == DBNull.Value ? -1 : reader["TrongSoBaiKT3"]);
                    monHoc.TrongSoDiemCC = Convert.ToInt32(reader["TrongSoDiemCC"] == DBNull.Value ? -1 : reader["TrongSoDiemCC"]);
                    monHocList.Add(monHoc);
                }

                return monHocList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MonHocViewModel GetMonHoc(string maMonHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                MonHocViewModel monHoc = new MonHocViewModel();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetMonHoc", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaMonHoc", maMonHoc);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    monHoc.MaMonHoc = reader["MaMonhoc"].ToString().Trim();
                    monHoc.TenMonHoc = reader["TenMonHoc"].ToString().Trim();
                    monHoc.TongSoTiet = reader["TongSoTiet"].ToString().Trim();
                    monHoc.SoTietLyThuyet = reader["SoTietLyThuyet"].ToString().Trim();
                    monHoc.SoTietThucHanh = reader["SoTietThucHanh"].ToString().Trim();
                    monHoc.TrongSoBaiKT1 = Convert.ToInt32(reader["TrongSoBaiKT1"] == DBNull.Value ? -1 : reader["TrongSoBaiKT1"]);
                    monHoc.TrongSoBaiKT2 = Convert.ToInt32(reader["TrongSoBaiKT2"] == DBNull.Value ? -1 : reader["TrongSoBaiKT2"]);
                    monHoc.TrongSoBaiKT3 = Convert.ToInt32(reader["TrongSoBaiKT3"] == DBNull.Value ? -1 : reader["TrongSoBaiKT3"]);
                    monHoc.TrongSoDiemCC = Convert.ToInt32(reader["TrongSoDiemCC"] == DBNull.Value ? -1 : reader["TrongSoDiemCC"]);
                }

                return monHoc;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetMaMonHoc(string maMonHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<string> monHocList = new List<string>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAllMaMH", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaMonHoc", maMonHoc);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string monHoc;
                    monHoc = reader["MaMonHoc"].ToString().Trim();
                    monHocList.Add(monHoc);
                }

                return monHocList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CResponseMessage InsertMonHoc(MonHocViewModel monHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                CResponseMessage resMess = new CResponseMessage();

                //luôn gán mã mới khi tạo thêm 1 môn học mới
                var listMH = GetMaMonHoc(monHoc.MaMonHoc);
                var lastElem = listMH.Count + 1;
                var maMonHoc = monHoc.MaMonHoc + lastElem.ToString(new string('0', 3));

                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_InsertMonHoc", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaMonHoc", maMonHoc);
                cmd.Parameters.AddWithValue("@TenMonHoc", monHoc.TenMonHoc);
                cmd.Parameters.AddWithValue("@TongSoTiet", (object)monHoc.TongSoTiet ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SoTietLyThuyet", monHoc.SoTietLyThuyet);
                cmd.Parameters.AddWithValue("@SoTietThucHanh", monHoc.SoTietThucHanh);
                cmd.Parameters.AddWithValue("@TrongSoBaiKT1", monHoc.TrongSoBaiKT1);
                cmd.Parameters.AddWithValue("@TrongSoBaiKT2", monHoc.TrongSoBaiKT2);
                cmd.Parameters.AddWithValue("@TrongSoBaiKT3", monHoc.TrongSoBaiKT3);
                cmd.Parameters.AddWithValue("@TrongSoDiemCC", monHoc.TrongSoDiemCC);

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
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public CResponseMessage UpdateMonHoc(MonHocViewModel monHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                CResponseMessage resMess = new CResponseMessage();

                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_UpdateMonHoc", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaMonHoc", monHoc.MaMonHoc);
                cmd.Parameters.AddWithValue("@TenMonHoc", monHoc.TenMonHoc);
                cmd.Parameters.AddWithValue("@TongSoTiet", (object)monHoc.TongSoTiet ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SoTietLyThuyet", monHoc.SoTietLyThuyet);
                cmd.Parameters.AddWithValue("@SoTietThucHanh", monHoc.SoTietThucHanh);
                cmd.Parameters.AddWithValue("@TrongSoBaiKT1", monHoc.TrongSoBaiKT1);
                cmd.Parameters.AddWithValue("@TrongSoBaiKT2", monHoc.TrongSoBaiKT2);
                cmd.Parameters.AddWithValue("@TrongSoBaiKT3", monHoc.TrongSoBaiKT3);
                cmd.Parameters.AddWithValue("@TrongSoDiemCC", monHoc.TrongSoDiemCC);

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

        public CResponseMessage DeleteMonHoc(MonHocViewModel monHoc)
        {
            throw new NotImplementedException();
        }
    }
}
