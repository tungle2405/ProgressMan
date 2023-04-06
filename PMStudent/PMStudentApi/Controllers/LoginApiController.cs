using CoreLib.Common;
using CoreLib.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMStudentApi.Context;
using PMStudentApi.Model;

namespace PMStudentApi.Controllers
{
    public class LoginApiController : Controller
    {
        private readonly ILogger<LoginApiController> _logger;
        private readonly IConfiguration _configuration;
        public LoginApiController(ILogger<LoginApiController> logger, IConfiguration Configuration)
        {
            _logger = logger;
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        //string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        // GET: LoginSV
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [Route("api/login")]
        [HttpPost]
        public string Login([FromBody] LoginApiModel login)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            //CResponeMessage cRespone = new CResponeMessage();
            DBConnection.GetSqlConnection(connectionString); //Mở

            var loginCheck = new LoginApiContext().CheckLogin(login);
            var result = JsonConvert.SerializeObject(loginCheck);

            DBConnection.GetSqlConnection(connectionString); //Đóng
            return result;
        }
    }
}
