using PMLecture.Interfaces;
using PMLecture.Models;
using CoreLib.DTO;
using System.Data.SqlClient;
using System.Data;
using CoreLib.Common;

namespace PMLecture.Context
{
    public class GiangVienContext : IGiangVien
    {
        public List<GiangVienViewModel> GetAll()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<GiangVienViewModel> giangVienList = new List<GiangVienViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAll", sqlcon);
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

        public GiangVienViewModel GetGiangVien(string maGiangVien)
        {
            return new GiangVienViewModel();
        }

        public CResponseMessage AddGiangVien(GiangVienViewModel giangVien)
        {
            return new CResponseMessage();
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
