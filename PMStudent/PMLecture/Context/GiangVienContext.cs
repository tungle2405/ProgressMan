using PMLecture.Interfaces;
using PMLecture.Models;
using CoreLib.DTO;
using System.Data.SqlClient;
using System.Data;
using CoreLib.Common;
using CoreLib.DAL;
using CoreLib.BUS;

namespace PMLecture.Context
{
    public class GiangVienContext : IGiangVien
    {
        public List<GiangVienViewModel> GetAllGiangVien()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<GiangVienViewModel> giangVienList = new List<GiangVienViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAllGiangVien", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    GiangVienViewModel giangVien = new GiangVienViewModel();
                    giangVien.MaGiangVien = reader["MaGiangVien"].ToString().Trim();
                    giangVien.HoTen = reader["HoTen"].ToString().Trim();
                    giangVien.GioiTinh = reader["GioiTinh"].ToString().Trim();
                    giangVien.TrinhDo = reader["TrinhDo"].ToString().Trim();
                    giangVien.ChuyenMon = reader["ChuyenMon"].ToString().Trim();
                    giangVien.MaPhanQuyen = Convert.ToString(reader["MaPhanQuyen"]).Trim();
                    giangVien.TenQuyen = reader["TenPhanQuyen"].ToString().Trim();
                    giangVien.MaDonVi = reader["MaDonVi"].ToString().Trim();
                    giangVien.TenDonVi = reader["TenDonVi"].ToString().Trim();
                    giangVienList.Add(giangVien);
                }

                return giangVienList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GiangVienViewModel> GetAllPhongDaoTao()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<GiangVienViewModel> giangVienList = new List<GiangVienViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAllPhongDaoTao", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    GiangVienViewModel giangVien = new GiangVienViewModel();
                    giangVien.MaGiangVien = reader["MaGiangVien"].ToString().Trim();
                    giangVien.HoTen = reader["HoTen"].ToString().Trim();
                    giangVien.GioiTinh = reader["GioiTinh"].ToString().Trim();
                    giangVien.TrinhDo = reader["TrinhDo"].ToString().Trim();
                    giangVien.ChuyenMon = reader["ChuyenMon"].ToString().Trim();
                    giangVien.MaPhanQuyen = Convert.ToString(reader["MaPhanQuyen"]).Trim();
                    giangVien.TenQuyen = reader["TenPhanQuyen"].ToString().Trim();
                    giangVien.MaDonVi = reader["MaDonVi"].ToString().Trim();
                    giangVien.TenDonVi = reader["TenDonVi"].ToString().Trim();
                    giangVien.Email = reader["TaiKhoan"].ToString().Trim();
                    giangVienList.Add(giangVien);
                }

                return giangVienList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetMaNhanVien(string maNV)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<string> nhanVienList = new List<string>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAll", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNhanVien", maNV);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string nhanVien;
                    nhanVien = reader["MaGiangVien"].ToString().Trim();
                    nhanVienList.Add(nhanVien);
                }

                return nhanVienList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DonViViewModel> GetDonVi()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<DonViViewModel> donviList = new List<DonViViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAllDonVi", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DonViViewModel donVi = new DonViViewModel();
                    donVi.MaDonVi = reader["MaDonVi"].ToString().Trim();
                    donVi.TenDonVi = reader["TenDonVi"].ToString().Trim();
                    donviList.Add(donVi);
                }

                return donviList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GiangVienViewModel GetGiangVien(string maGiangVien)
        {
            return new GiangVienViewModel();
        }

        public CResponseMessage InsertNhanVien(GiangVienViewModel giangVien)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                CResponseMessage resMess = new CResponseMessage();
                var listNV =  GetMaNhanVien(giangVien.MaDonVi);

                var lastElem = listNV.Count + 1;
                var maNhanVien = giangVien.MaDonVi + lastElem.ToString(new string('0', 4));
                var enPass = new CoreLib.DAL.HashCode().Encrypt(maNhanVien);
                var mailNV = "phongdaotao" + lastElem.ToString(new string('0', 4)) + "@tlu.edu.vn";

                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_InsertEmployee", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaGiangVien", maNhanVien);
                cmd.Parameters.AddWithValue("@HoTen", giangVien.HoTen);
                cmd.Parameters.AddWithValue("@GioiTinh", giangVien.GioiTinh);
                cmd.Parameters.AddWithValue("@TrinhDo", (object)giangVien.TrinhDo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyenMon", (object)giangVien.ChuyenMon ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MaDonVi", giangVien.MaDonVi);
                cmd.Parameters.AddWithValue("@MaPhanQuyen", giangVien.MaPhanQuyen);
                cmd.Parameters.AddWithValue("@TaiKhoan", giangVien.MaDonVi == "PDT" ? mailNV : giangVien.Email);
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
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public CResponseMessage UpdateGiangVien(GiangVienViewModel giangVien)
        {
            return new CResponseMessage();
        }

        public CResponseMessage DeleteGiangVien(string maGiangVien)
        {
            return new CResponseMessage();
        }


    }
}
