using CoreLib.DTO;
using PMLecture.Interfaces;
using PMLecture.Models;
using System.Data.SqlClient;
using System.Data;
using CoreLib.Common;

namespace PMLecture.Context
{
    public class SinhVienContext : ISinhVien
    {
        public List<SinhVienViewModel> GetAllSinhVien()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<SinhVienViewModel> sinhVienList = new List<SinhVienViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAllSinhVien", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SinhVienViewModel sinhVien = new SinhVienViewModel();
                    sinhVien.MaSinhVien = reader["MaSinhVien"].ToString().Trim();
                    sinhVien.HoTen = reader["HoTen"].ToString().Trim();
                    sinhVien.GioiTinh = reader["GioiTinh"].ToString().Trim();
                    sinhVien.LopNienChe = reader["LopNienChe"].ToString().Trim();
                    sinhVienList.Add(sinhVien);
                }

                return sinhVienList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public SinhVienViewModel GetSinhVien(string maSinhVien)
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                SinhVienViewModel sinhVien = new SinhVienViewModel();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetSinhVien", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSinhVien", maSinhVien);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sinhVien.MaSinhVien = reader["MaSinhVien"].ToString().Trim();
                    sinhVien.HoTen = reader["HoTen"].ToString().Trim();
                    sinhVien.GioiTinh = reader["GioiTinh"].ToString().Trim();
                    sinhVien.LopNienChe = reader["LopNienChe"].ToString().Trim();
                    sinhVien.Email = reader["TaiKhoan"].ToString().Trim();
                }

                return sinhVien;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetMaSinhVien(string maSV)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<string> sinhVienList = new List<string>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAllMaSV", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSinhVien", maSV);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string MSV;
                    MSV = reader["MaSinhVien"].ToString().Trim();
                    sinhVienList.Add(MSV);
                }

                return sinhVienList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CResponseMessage InsertSinhVien(SinhVienViewModel sinhVien)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                CResponseMessage resMess = new CResponseMessage();

                string namVaoTruong = DateTime.Today.Year.ToString().Substring(2, 2);
                string soNamHoc = "5";
                string maNganh = sinhVien.MaNganh.ToString();
                string maSVChuaDD = namVaoTruong + soNamHoc + maNganh;

                var listSV = GetMaSinhVien(maSVChuaDD);

                var lastElem = listSV.Count + 1;
                var maSinhVien = maSVChuaDD + lastElem.ToString(new string('0', 4));
                sinhVien.Email = maSinhVien + "@e.tlu.edu.vn";
                var enPass = new CoreLib.DAL.HashCode().Encrypt(maSinhVien);

                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_InsertSinhVien", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSinhVien", maSinhVien);
                cmd.Parameters.AddWithValue("@HoTen", sinhVien.HoTen);
                cmd.Parameters.AddWithValue("@GioiTinh", sinhVien.GioiTinh);
                cmd.Parameters.AddWithValue("@LopNienChe", sinhVien.LopNienChe);
                cmd.Parameters.AddWithValue("@TaiKhoan", sinhVien.Email);
                cmd.Parameters.AddWithValue("@MatKhau", enPass);

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

        public CResponseMessage UpdateSinhVien(SinhVienViewModel sinhVien)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                CResponseMessage resMess = new CResponseMessage();

                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_UpdateSinhVien", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSinhVien", sinhVien.MaSinhVien);
                cmd.Parameters.AddWithValue("@HoTen", sinhVien.HoTen);
                cmd.Parameters.AddWithValue("@GioiTinh", sinhVien.GioiTinh);
                cmd.Parameters.AddWithValue("@LopNienChe", sinhVien.LopNienChe);

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

        public CResponseMessage DeleteSinhVien(string maGiangVien)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                CResponseMessage resMess = new CResponseMessage();

                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_DeleteSinhVien", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSinhVien", maGiangVien);

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
