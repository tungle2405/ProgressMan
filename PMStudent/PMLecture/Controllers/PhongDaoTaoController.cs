using CoreLib.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PMLecture.Context;
using PMLecture.Models;

namespace PMLecture.Controllers
{
    public class PhongDaoTaoController : Controller
    {
        private readonly IConfiguration _configuration;
        public PhongDaoTaoController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<GiangVienViewModel> giangVienInfos = new List<GiangVienViewModel>();

            try
            {
                if (HttpContext.Session.GetString("user") == null || HttpContext.Session.GetString("user") != "ADMIN")
                {
                    return RedirectToAction("Index", "LoginGV");
                }

                var session = HttpContext.Session.GetString("user");

                DBConnection.GetSqlConnection(connectionString); //Mở

                var accInfo = new ThongTinTKContext().GetThongTin(session);
                giangVienInfos = new GiangVienContext().GetAllPhongDaoTao();

                ViewBag.AccInfo = accInfo;
                ViewBag.Side = "PDT";

                DBConnection.GetSqlConnection(connectionString); //Đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(giangVienInfos);

            //return View();
        }
    }
}
