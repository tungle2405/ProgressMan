using CoreLib.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMLecture.Models;
using PMStudentApi.Context;
using PMStudentApi.Model;
using PMStudentApi.Models;

namespace PMStudentApi.Controllers
{
    public class DiemDanhApiController : Controller
    {
        private readonly ILogger<LoginApiController> _logger;
        private readonly IConfiguration _configuration;
        public DiemDanhApiController(ILogger<LoginApiController> logger, IConfiguration Configuration)
        {
            _logger = logger;
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("api/getlopmonhoc")]
        [HttpPost]
        public string GetLopMonHoc([FromBody] string maSinhVien)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            //CResponeMessage cRespone = new CResponeMessage();
            DBConnection.GetSqlConnection(connectionString); //Mở

            var loginCheck = new DiemDanhApiContext().GetLopMonHocTheoSinhVien(maSinhVien);
            var result = JsonConvert.SerializeObject(loginCheck);

            DBConnection.GetSqlConnection(connectionString); //Đóng
            return result;
        }

        [Route("api/getalldiemdanh")]
        [HttpPost]
        public string GetAllDiemDanh([FromBody] LopMonHocViewModel lopMonHoc)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            //CResponeMessage cRespone = new CResponeMessage();
            DBConnection.GetSqlConnection(connectionString); //Mở

            var loginCheck = new DiemDanhApiContext().GetAllDiemDanhByMaSinhVien(lopMonHoc.MaSinhVien, lopMonHoc.MaLopMonHoc);
            var result = JsonConvert.SerializeObject(loginCheck);

            DBConnection.GetSqlConnection(connectionString); //Đóng
            return result;
        }
    }
}
