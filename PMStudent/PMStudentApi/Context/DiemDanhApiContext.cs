using CoreLib.DTO;
using PMStudentApi.Model;
using System.Data.SqlClient;
using System.Data;
using CoreLib.Common;
using PMLecture.Interfaces;
using PMStudentApi.Models;
using PMLecture.Models;

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

        public DiemQuaTrinhViewModel GetDiemQuaTrinh(string maSinhVien, string maLopMonHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                DiemQuaTrinhViewModel diemQT = new DiemQuaTrinhViewModel();

                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("SV_SP_GetDiemQuaTrinh", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSinhVien", maSinhVien);
                cmd.Parameters.AddWithValue("@MaLopMonHoc", maLopMonHoc);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    diemQT.MaSinhVien = reader["MaSinhVien"].ToString().Trim();
                    diemQT.TenSinhVien = reader["TenSV"].ToString().Trim();
                    diemQT.TenGiangVien = reader["TenGV"].ToString().Trim();
                    diemQT.LopNienChe = reader["LopNienChe"].ToString().Trim();
                    diemQT.MaLopMonHoc = reader["MaLopMonHoc"].ToString().Trim();
                    diemQT.TenLopMonHoc = reader["TenLopMonHoc"].ToString().Trim();
                    diemQT.SoBuoiCoMat = Convert.ToInt32(reader["TongCoMat"]);
                    diemQT.SoBuoiVang = Convert.ToInt32(reader["TongVangMat"]);
                    diemQT.DiemKiemTraBuoi1 = Convert.ToDouble(reader["BaiKiemTra1"] == DBNull.Value ? -1 : reader["BaiKiemTra1"]);
                    diemQT.DiemKiemTraBuoi2 = Convert.ToDouble(reader["BaiKiemTra2"] == DBNull.Value ? -1 : reader["BaiKiemTra2"]);
                    diemQT.DiemKiemTraBuoi3 = Convert.ToDouble(reader["BaiKiemTra3"] == DBNull.Value ? -1 : reader["BaiKiemTra3"]);
                    diemQT.DiemChuyenCan = (10 - diemQT.SoBuoiVang);
                    if(diemQT.DiemKiemTraBuoi1 == -1 || diemQT.DiemKiemTraBuoi2 == -1 || diemQT.DiemKiemTraBuoi3 == -1)
                    {
                        diemQT.DiemQuaTrinh = -1;
                    }
                    else
                    {
                        diemQT.DiemQuaTrinh = Math.Round((((diemQT.DiemKiemTraBuoi1 * 0.2) + (diemQT.DiemKiemTraBuoi2 * 0.4) + (diemQT.DiemKiemTraBuoi1 * 0.4)) * 0.8) + (diemQT.DiemChuyenCan * 0.2), 2);
                    }
                }

                return diemQT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SinhVienViewModel GetThongTinSV(string maSinhVien)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("SV_SP_GetThongTinTK", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSinhVien", maSinhVien);

                var reader = cmd.ExecuteReader();

                SinhVienViewModel sinhVien = new SinhVienViewModel();

                while (reader.Read())
                {
                    sinhVien.MaSinhVien = reader["MaSinhVien"].ToString().Trim();
                    sinhVien.HoTen = reader["HoTen"].ToString().Trim();
                    sinhVien.LopNienChe = reader["LopNienChe"].ToString().Trim();
                    sinhVien.HoatDong = Convert.ToInt32(reader["HoatDong"]);
                }

                return sinhVien;
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
