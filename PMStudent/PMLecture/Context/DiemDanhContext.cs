using PMLecture.Interfaces;
using PMLecture.Models;
using System.Data.SqlClient;
using System.Data;
using CoreLib.Common;
using CoreLib.DTO;
using System;

namespace PMLecture.Context
{
    public class DiemDanhContext : IDiemDanh
    {
        public List<CaHocViewModel> GetAllCaHoc()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<CaHocViewModel> caHocList = new List<CaHocViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAllCaHoc", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CaHocViewModel caHoc = new CaHocViewModel();
                    caHoc.MaCaHoc = reader["MaCaHoc"].ToString().Trim();
                    caHoc.GioBD = reader["GioBD"].ToString().Trim();
                    caHoc.GioKT = reader["GioKT"].ToString().Trim();
                    caHoc.BuoiHoc = reader["BuoiHoc"].ToString().Trim();
                    caHocList.Add(caHoc);
                }

                return caHocList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BuoiHocViewModel> GetAllBuoiHoc()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<BuoiHocViewModel> buoiHocList = new List<BuoiHocViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAllBuoiHoc", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BuoiHocViewModel buoiHoc = new BuoiHocViewModel();
                    buoiHoc.MaBuoiHoc = reader["MaBuoiHoc"].ToString().Trim();
                    buoiHoc.BuoiHocSo = reader["BuoiHocSo"].ToString().Trim();
                    buoiHocList.Add(buoiHoc);
                }

                return buoiHocList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BuoiHocViewModel> GetAllBuoiHoc(string maLopMonHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<BuoiHocViewModel> buoiHocList = new List<BuoiHocViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetBuoiHocByLopMonHoc", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLopMonHoc", maLopMonHoc);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BuoiHocViewModel buoiHoc = new BuoiHocViewModel();
                    buoiHoc.MaBuoiHoc = reader["MaBuoiHoc"].ToString().Trim();
                    buoiHoc.BuoiHocSo = reader["BuoiHocSo"].ToString().Trim();
                    buoiHocList.Add(buoiHoc);
                }

                return buoiHocList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new List<BuoiHocViewModel>();
        }

        public List<XemLaiDDViewModel> GetDiemDanhByBuoiHoc(string maLopMonHoc, string? maBuoiHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<XemLaiDDViewModel> diemDanhList = new List<XemLaiDDViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetDiemDanhByBuoiHoc", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLopMonHoc", maLopMonHoc);
                cmd.Parameters.AddWithValue("@MaBuoiHoc", maBuoiHoc ?? "");

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    XemLaiDDViewModel diemDanh = new XemLaiDDViewModel();
                    diemDanh.ID = reader["IDDiemDanh"].ToString().Trim();
                    diemDanh.MaSinhVien = reader["MaSinhVien"].ToString().Trim();
                    diemDanh.TenSinhVien = reader["TenSinhVien"].ToString().Trim();
                    diemDanh.NgayDiemDanh = Convert.ToDateTime(reader["NgayDiemDanh"]);
                    diemDanh.MaBuoiHoc = reader["MaBuoiHoc"].ToString().Trim();
                    diemDanh.BuoiHocSo = reader["BuoiHocSo"].ToString().Trim();
                    diemDanh.GioBatDau = reader["GioBatDau"].ToString().Trim();
                    diemDanh.GioKetThuc = reader["GioKetThuc"].ToString().Trim();
                    diemDanh.TrangThai = Convert.ToBoolean(reader["TrangThai"]);
                    diemDanhList.Add(diemDanh);
                }

                return diemDanhList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CResponseMessage DiemDanhSinhVien(DiemDanhViewModel listDiemDanh)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                CResponseMessage resMess = new CResponseMessage();
                var listBuoiHoc = GetAllBuoiHoc(listDiemDanh.MaLopMonHoc);
                foreach (var item in listBuoiHoc)
                {
                    if(item.MaBuoiHoc == listDiemDanh.MaBuoiHoc)
                    {
                        resMess.Code = -1;
                        resMess.Message = "Buổi học này đã tồn tại trong db vui lòng chọn buổi học tiếp theo!";
                        resMess.Data = "";

                        return resMess;
                    }
                }

                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_DiemDanhSinhVien", sqlcon);

                foreach(var item in listDiemDanh.sinhVienDiemDanhs)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaGiangVien", listDiemDanh.MaGiangVien);
                    cmd.Parameters.AddWithValue("@MaLopMonHoc", listDiemDanh.MaLopMonHoc);
                    cmd.Parameters.AddWithValue("@MaMonHoc", listDiemDanh.MaMonHoc);
                    cmd.Parameters.AddWithValue("@NgayDiemDanh", listDiemDanh.NgayDiemDanh);
                    cmd.Parameters.AddWithValue("@MaBuoiHoc", listDiemDanh.MaBuoiHoc);
                    cmd.Parameters.AddWithValue("@MaCaHoc", listDiemDanh.MaCaHoc);

                    cmd.Parameters.AddWithValue("@MaSinhVien", item.MaSinhVien);
                    cmd.Parameters.AddWithValue("@TrangThai", item.TrangThai ? 0 : 1);

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

                    if (resMess.Code == -1)
                    {
                        var resetDiemDanh = HuyBoDD(listDiemDanh);
                        return resetDiemDanh;
                    }else if(resMess.Code == -2) {
                        return resMess;
                    }


                }

                return resMess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DiemChuyenCanViewModel> ListDiemChuyenCan(string maLopMonHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<DiemChuyenCanViewModel> diemChuyenCanList = new List<DiemChuyenCanViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetDiemChuyenCanSV", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLopMonHoc", maLopMonHoc);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DiemChuyenCanViewModel diemCC = new DiemChuyenCanViewModel();
                    diemCC.MaSinhVien = reader["MaSinhVien"].ToString().Trim();
                    diemCC.TenSinhVien = reader["HoTen"].ToString().Trim();
                    diemCC.LopNienChe = reader["LopNienChe"].ToString().Trim();
                    diemCC.MaLopMonHoc = reader["MaLopMonHoc"].ToString().Trim();
                    diemCC.SoBuoiCoMat = Convert.ToInt32(reader["TongCoMat"]);
                    diemCC.SoBuoiVang = Convert.ToInt32(reader["TongVangMat"]);
                    diemCC.DiemChuyenCan = (10 - diemCC.SoBuoiVang) * 0.2;
                    diemChuyenCanList.Add(diemCC);
                }

                return diemChuyenCanList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DiemQuaTrinhViewModel> ListDiemQuaTrinh(string maLopMonHoc)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<DiemQuaTrinhViewModel> diemQuaTrinhList = new List<DiemQuaTrinhViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetDiemQuaTrinh", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLopMonHoc", maLopMonHoc);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DiemQuaTrinhViewModel diemQT = new DiemQuaTrinhViewModel();
                    diemQT.MaSinhVien = reader["MaSinhVien"].ToString().Trim();
                    diemQT.TenSinhVien = reader["HoTen"].ToString().Trim();
                    diemQT.LopNienChe = reader["LopNienChe"].ToString().Trim();
                    diemQT.MaLopMonHoc = reader["MaLopMonHoc"].ToString().Trim();
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
                    diemQuaTrinhList.Add(diemQT);
                }

                return diemQuaTrinhList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CResponseMessage HuyBoDD(DiemDanhViewModel listDiemDanh)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                CResponseMessage resMess = new CResponseMessage();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_HuyBoDD", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaGiangVien", listDiemDanh.MaGiangVien);
                cmd.Parameters.AddWithValue("@MaLopMonHoc", listDiemDanh.MaLopMonHoc);
                cmd.Parameters.AddWithValue("@NgayDiemDanh", listDiemDanh.NgayDiemDanh);
                cmd.Parameters.AddWithValue("@MaBuoiHoc", listDiemDanh.MaBuoiHoc);
                cmd.Parameters.AddWithValue("@MaCaHoc", listDiemDanh.MaCaHoc);

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
