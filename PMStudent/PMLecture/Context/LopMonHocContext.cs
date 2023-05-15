using CoreLib.DTO;
using PMLecture.Interfaces;
using PMLecture.Models;
using System.Data.SqlClient;
using System.Data;
using CoreLib.Common;

namespace PMLecture.Context
{
    public class LopMonHocContext : ILopMonHoc
    {
        public List<LopMonHocViewModel> GetAllLopMonHoc(string maPhanQuyen)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                if(maPhanQuyen == "ADMIN" || maPhanQuyen.Substring(0, 3) == "PDT")
                {
                    List<LopMonHocViewModel> lopMonHocList = new List<LopMonHocViewModel>();
                    var sqlcon = DBConnection.GetSqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand("GV_SP_GetAllLopMonHoc", sqlcon);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        LopMonHocViewModel lopMonHoc = new LopMonHocViewModel();
                        lopMonHoc.MaLopMonHoc = reader["MaLopMonHoc"].ToString().Trim();
                        lopMonHoc.TenLopMonHoc = reader["TenLopMonHoc"].ToString().Trim();
                        lopMonHoc.MaGiangVien = reader["MaGiangVien"].ToString().Trim();
                        lopMonHoc.TenGiangVien = reader["HoTen"].ToString().Trim();
                        lopMonHoc.HocKy = reader["HocKy"].ToString().Trim();
                        lopMonHoc.NamHoc = reader["NamHoc"].ToString().Trim();
                        lopMonHoc.MaMonHoc = reader["MaMonHoc"].ToString().Trim();
                        lopMonHoc.SoSinhVien = reader["SoSinhVien"].ToString().Trim();
                        lopMonHoc.TongSoTiet = reader["TongSoTiet"].ToString().Trim();
                        lopMonHoc.SoTietLyThuyet = reader["SoTietLyThuyet"].ToString().Trim();
                        lopMonHoc.SoTietThucHanh = reader["SoTietThucHanh"].ToString().Trim();
                        lopMonHocList.Add(lopMonHoc);
                    }

                    return lopMonHocList;
                }
                else
                {
                    return new List<LopMonHocViewModel>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LopMonHocViewModel GetLopMonHoc(string maLopMonHoc)
        {
            throw new NotImplementedException();
        }

        public List<LopMonHocViewModel> GetLopMonHocTheoGiangVien(string maGiangVien)
        {
            throw new NotImplementedException();
        }

        public List<string> GetMaLopMonHoc(string maLopMonHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<string> monHocList = new List<string>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAllMaLopMH", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLopMonHoc", maLopMonHoc);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string monHoc;
                    monHoc = reader["MaLopMonHoc"].ToString().Trim();
                    monHocList.Add(monHoc);
                }

                return monHocList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CResponseMessage InsertLopMonHoc(LopMonHocViewModel lopMonHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                CResponseMessage resMess = new CResponseMessage();

                //luôn gán mã mới khi tạo thêm 1 lớp môn học mới
                var listMH = GetMaLopMonHoc(lopMonHoc.MaMonHoc);
                var lastElem = listMH.Count + 1;
                var maLopMonHoc = lopMonHoc.MaMonHoc + "_" + lastElem.ToString(new string('0', 3));

                //tên lớp môn học được gán bằng tên môn học - học kỳ - 2 số cuối của năm học
                var getTenMonHoc = new MonHocContext().GetAllMonHoc().FirstOrDefault(x => x.MaMonHoc == lopMonHoc.MaMonHoc).TenMonHoc;
                var tenLopMonHoc = getTenMonHoc + "-" + lopMonHoc.HocKy + "-" + lopMonHoc.NamHoc.Substring(lopMonHoc.NamHoc.Length - 2);

                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_InsertLopMonHoc", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLopMonHoc", maLopMonHoc);
                cmd.Parameters.AddWithValue("@TenLopMonHoc", tenLopMonHoc);
                cmd.Parameters.AddWithValue("@MaGiangVien", lopMonHoc.MaGiangVien);
                cmd.Parameters.AddWithValue("@HocKy", lopMonHoc.HocKy);
                cmd.Parameters.AddWithValue("@NamHoc", lopMonHoc.NamHoc);
                cmd.Parameters.AddWithValue("@MaMonHoc", lopMonHoc.MaMonHoc);
                cmd.Parameters.AddWithValue("@SoSinhVien", lopMonHoc.SoSinhVien);

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

        public CResponseMessage UpdateLopMonHoc(LopMonHocViewModel lopMonHoc)
        {
            throw new NotImplementedException();
        }

        public CResponseMessage DeleteLopMonHoc(LopMonHocViewModel lopMonHoc)
        {
            throw new NotImplementedException();
        }
    }
}
