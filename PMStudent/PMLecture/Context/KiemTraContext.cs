using PMLecture.Interfaces;
using PMLecture.Models;
using System.Data.SqlClient;
using System.Data;
using CoreLib.Common;
using CoreLib.DTO;

namespace PMLecture.Context
{
    public class KiemTraContext : IKiemTra
    {
        public List<BuoiHocViewModel> GetAllBuoiKiemTra()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<BuoiHocViewModel> buoiHocList = new List<BuoiHocViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetBuoiKiemTra", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BuoiHocViewModel buoiHoc = new BuoiHocViewModel();
                    buoiHoc.MaBuoiHoc = reader["MaBuoiKiemTra"].ToString().Trim();
                    buoiHoc.BuoiHocSo = reader["BuoiKiemtra"].ToString().Trim();
                    buoiHocList.Add(buoiHoc);
                }

                return buoiHocList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BuoiHocViewModel> GetAllBuoiKiemTra(string maLopMonHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<BuoiHocViewModel> buoiHocList = new List<BuoiHocViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetBuoiKiemTraByLMH", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLopMonHoc", maLopMonHoc);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BuoiHocViewModel buoiHoc = new BuoiHocViewModel();
                    buoiHoc.MaBuoiHoc = reader["MaBuoiKiemTra"].ToString().Trim();
                    buoiHoc.BuoiHocSo = reader["BuoiKiemtra"].ToString().Trim();
                    buoiHocList.Add(buoiHoc);
                }

                return buoiHocList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CResponseMessage ThemDiemKiemTraSinhVien(KiemTraViewModel listKiemTra)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                CResponseMessage resMess = new CResponseMessage();
                var listBuoiKT = GetAllBuoiKiemTra(listKiemTra.MaLopMonHoc);
                foreach (var item in listBuoiKT)
                {
                    if (item.MaBuoiHoc == listKiemTra.MaBuoiKiemTra)
                    {
                        resMess.Code = -1;
                        resMess.Message = "Bài kiểm tra này đã tồn tại trong db vui lòng chọn bài kiểm tra tiếp theo!";
                        resMess.Data = "";

                        return resMess;
                    }
                }

                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_ThemDiemKiemTra", sqlcon);

                foreach (var item in listKiemTra.sinhVienKiemTras)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaGiangVien", listKiemTra.MaGiangVien);
                    cmd.Parameters.AddWithValue("@MaMonHoc", listKiemTra.MaMonHoc);
                    cmd.Parameters.AddWithValue("@MaLopMonHoc", listKiemTra.MaLopMonHoc);
                    cmd.Parameters.AddWithValue("@MaBuoiKiemTra", listKiemTra.MaBuoiKiemTra);

                    cmd.Parameters.AddWithValue("@MaSinhVien", item.MaSinhVien);
                    cmd.Parameters.AddWithValue("@Diem", item.DiemKiemTra);

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

                    cmd.Parameters.Clear();
                }

                return resMess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
