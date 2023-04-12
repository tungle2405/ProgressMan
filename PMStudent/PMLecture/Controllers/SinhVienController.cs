using CoreLib.Common;
using Microsoft.AspNetCore.Mvc;
using PMLecture.Context;
using PMLecture.Models;

namespace PMLecture.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly IConfiguration _configuration;
        public SinhVienController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<SinhVienViewModel> sinhVienInfos = new List<SinhVienViewModel>();

            try
            {
                if (HttpContext.Session.GetString("user") == null || HttpContext.Session.GetString("user") != "ADMIN"
                    && HttpContext.Session.GetString("user").Substring(0, 2) != "NV")
                {
                    return RedirectToAction("Index", "LoginGV");
                }

                var session = HttpContext.Session.GetString("user");

                DBConnection.GetSqlConnection(connectionString); //Mở

                var accInfo = new ThongTinTKContext().GetThongTin(session);
                sinhVienInfos = new SinhVienContext().GetAllSinhVien();

                ViewBag.AccInfo = accInfo;
                ViewBag.Side = "SinhVien";

                DBConnection.GetSqlConnection(connectionString); //Đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(sinhVienInfos);

            //return View();
        }
    }
}
