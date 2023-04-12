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

        public SinhVienViewModel GetSinhVien(string maGiangVien)
        {
            return new SinhVienViewModel();
        }

        public CResponseMessage AddSinhVien(SinhVienViewModel giangVien)
        {
            return new CResponseMessage();
        }

        public CResponseMessage UpdateSinhVien(SinhVienViewModel giangVien)
        {
            return new CResponseMessage();
        }

        public CResponseMessage DeleteSinhVien(string maGiangVien)
        {
            return new CResponseMessage();
        }
    }
}
