using CoreLib.DTO;
using PMLecture.Interfaces;
using System.Data.SqlClient;
using System.Data;
using CoreLib.Common;
using PMLecture.Models;

namespace PMLecture.Context
{
    public class ThongTinTKContext : IThongTinTK
    {
        public GiangVienViewModel GetThongTin(string maNhanVien)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                GiangVienViewModel nhanVien = new GiangVienViewModel();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_ThongTinTaiKhoan", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    nhanVien.MaGiangVien = reader["MaGiangVien"].ToString().Trim();
                    nhanVien.HoTen = reader["HoTen"].ToString().Trim();
                    nhanVien.GioiTinh = reader["GioiTinh"].ToString().Trim();
                    nhanVien.TrinhDo = reader["TrinhDo"].ToString().Trim();
                    nhanVien.ChuyenMon = reader["ChuyenMon"].ToString().Trim();
                    nhanVien.MaPhanQuyen = Convert.ToString(reader["MaPhanQuyen"]).Trim();
                    nhanVien.TenQuyen = reader["TenPhanQuyen"].ToString().Trim();
                    nhanVien.MaDonVi = reader["MaDonVi"].ToString().Trim();
                    nhanVien.TenDonVi = reader["MaGiangVien"].ToString().Trim();
                }

                return nhanVien;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
