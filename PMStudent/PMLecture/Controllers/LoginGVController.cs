using CoreLib.Common;
using CoreLib.DTO;
using CoreLib.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PMLecture.Context;
using PMLecture.Models;
using System.Text;

namespace PMLecture.Controllers
{
    public class LoginGVController : Controller
    {
        private readonly ILogger<LoginGVController> _logger;
        private readonly IConfiguration _configuration;
        public LoginGVController(ILogger<LoginGVController> logger, IConfiguration Configuration)
        {
            _logger = logger;
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            try
            {
                if (HttpContext.Session.GetString("user") != null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                //encrypt password
                login.MatKhau = (login.MatKhau != null) ? new CoreLib.DAL.HashCode().Encrypt(login.MatKhau) : null;

                DBConnection.GetSqlConnection(connectionString); //Mở

                var loginCheck = new LoginGVContext().CheckLogin(login);
                var contents = JsonConvert.SerializeObject(loginCheck);

                DBConnection.GetSqlConnection(connectionString); //Đóng

                CResponseMessage crMess = new CResponseMessage();
                crMess = JsonConvert.DeserializeObject<CResponseMessage>(contents);
                if (crMess.Code == 0)
                {
                    HttpContext.Session.SetString("user", crMess.Data.Trim());
                    //Session["user"] = crMess.Data;
                    return Json(contents);
                }
                else
                {
                    return Json(contents);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            //Session.Clear();
            return RedirectToAction("Index", "LoginGV");
        }
    }
}
