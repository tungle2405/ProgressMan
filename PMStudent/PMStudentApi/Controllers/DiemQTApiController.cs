using CoreLib.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMStudentApi.Context;
using PMStudentApi.Models;

namespace PMStudentApi.Controllers
{
    public class DiemQTApiController : Controller
    {
        private readonly ILogger<LoginApiController> _logger;
        private readonly IConfiguration _configuration;
        public DiemQTApiController(ILogger<LoginApiController> logger, IConfiguration Configuration)
        {
            _logger = logger;
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("api/getdiemquatrinh")]
        [HttpPost]
        public string GetDiemQuaTrinh([FromBody] LopMonHocViewModel lopMonHoc)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            //CResponeMessage cRespone = new CResponeMessage();
            DBConnection.GetSqlConnection(connectionString); //Mở

            var loginCheck = new DiemDanhApiContext().GetDiemQuaTrinh(lopMonHoc.MaSinhVien, lopMonHoc.MaLopMonHoc);
            var result = JsonConvert.SerializeObject(loginCheck);

            DBConnection.GetSqlConnection(connectionString); //Đóng
            return result;
        }
    }
}
