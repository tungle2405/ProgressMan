using CoreLib.DTO;
using PMStudentApi.Model;
using System.Data.SqlClient;
using System.Data;
using CoreLib.Common;
using PMLecture.Interfaces;
using PMStudentApi.Models;

namespace PMStudentApi.Context
{
    public class DiemDanhApiContext : IDiemDanh
    {
        /// <summary>
        /// Hàm này sử dụng để kiểm tra login
        /// </summary>
        /// <param name="maSinhVien"></param>
        /// <returns></returns>
        public List<DiemDanhViewModel> GetAllDiemDanhByMaSinhVien(string maSinhVien, string maLopMonHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<DiemDanhViewModel> listChiTietDD = new List<DiemDanhViewModel>();

                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("SV_SP_GetAllDiemDanhBySinhVien", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSinhVien", maSinhVien);
                cmd.Parameters.AddWithValue("@MaLopMonHoc", maLopMonHoc);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DiemDanhViewModel chiTietDD = new DiemDanhViewModel();
                    chiTietDD.ID = reader["IDDiemDanh"].ToString().Trim();
                    chiTietDD.MaSinhVien = reader["MaSinhVien"].ToString().Trim();
                    chiTietDD.TenLopMonHoc = reader["TenLopMonHoc"].ToString().Trim();
                    chiTietDD.TenGiangVien = reader["TenGV"].ToString().Trim();
                    chiTietDD.TenSinhVien = reader["TenSV"].ToString().Trim();
                    chiTietDD.GioBatDau = reader["GioBD"].ToString().Trim();
                    chiTietDD.GioKetThuc = reader["GioKT"].ToString().Trim();
                    chiTietDD.BuoiHocSo = reader["BuoiHocSo"].ToString().Trim();
                    chiTietDD.NgayDiemDanh = Convert.ToDateTime(reader["NgayDiemDanh"]);
                    chiTietDD.TrangThai = Convert.ToBoolean(reader["TrangThai"]);
                    listChiTietDD.Add(chiTietDD);
                }

                return listChiTietDD;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LopMonHocViewModel> GetLopMonHocTheoSinhVien(string maSinhVien)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<LopMonHocViewModel> lopMonHocList = new List<LopMonHocViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("SV_SP_GetAllLopMonHoc", sqlcon);
                cmd.Parameters.AddWithValue("@MaSinhVien", maSinhVien);
                cmd.CommandType = CommandType.StoredProcedure;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    LopMonHocViewModel lopMonHoc = new LopMonHocViewModel();
                    lopMonHoc.MaLopMonHoc = reader["MaLopMonHoc"].ToString().Trim();
                    lopMonHoc.TenLopMonHoc = reader["TenLopMonHoc"].ToString().Trim();
                    lopMonHoc.MaSinhVien = reader["MaSinhVien"].ToString().Trim();
                    lopMonHoc.TenSinhVien = reader["HoTen"].ToString().Trim();
                    lopMonHoc.HocKy = reader["HocKy"].ToString().Trim();
                    lopMonHoc.NamHoc = reader["NamHoc"].ToString().Trim();
                    lopMonHoc.LopNienCheSV = reader["LopNienChe"].ToString().Trim();
                    lopMonHoc.HoatDongSV = reader["HoatDong"].ToString().Trim();
                    lopMonHocList.Add(lopMonHoc);
                }

                return lopMonHocList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
