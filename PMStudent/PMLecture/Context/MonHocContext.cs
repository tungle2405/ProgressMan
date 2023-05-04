using CoreLib.DTO;
using PMLecture.Interfaces;
using PMLecture.Models;
using System.Data.SqlClient;
using System.Data;
using CoreLib.Common;

namespace PMLecture.Context
{
    public class MonHocContext : IMonHoc
    {
        public List<MonHocViewModel> GetAllMonHoc()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            string connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");

            try
            {
                List<MonHocViewModel> monHocList = new List<MonHocViewModel>();
                var sqlcon = DBConnection.GetSqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GV_SP_GetAllMonHoc", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MonHocViewModel monHoc = new MonHocViewModel();
                    monHoc.MaMonHoc = reader["MaMonHoc"].ToString().Trim();
                    monHoc.TenMonHoc = reader["TenMonHoc"].ToString().Trim();
                    monHoc.TongSoTiet = reader["TongSoTiet"].ToString().Trim();
                    monHoc.SoTietLyThuyet = reader["SoTietLyThuyet"].ToString().Trim();
                    monHoc.SoTietThucHanh = reader["SoTietThucHanh"].ToString().Trim();
                    monHocList.Add(monHoc);
                }

                return monHocList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MonHocViewModel GetMonHoc(string maMonHoc)
        {
            throw new NotImplementedException();
        }

        public CResponseMessage InsertMonHoc(MonHocViewModel monHoc)
        {
            throw new NotImplementedException();
        }

        public CResponseMessage UpdateMonHoc(MonHocViewModel monHoc)
        {
            throw new NotImplementedException();
        }

        public CResponseMessage DeleteMonHoc(MonHocViewModel monHoc)
        {
            throw new NotImplementedException();
        }
    }
}
