using CoreLib.Common;
using CoreLib.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
                    && HttpContext.Session.GetString("user").Substring(0, 2) != "NV" && HttpContext.Session.GetString("user").Substring(0, 3) != "PDT")
                {
                    return RedirectToAction("Index", "LoginGV");
                }

                var session = HttpContext.Session.GetString("user");

                DBConnection.GetSqlConnection(connectionString); //Mở

                var accInfo = new ThongTinTKContext().GetThongTin(session);
                sinhVienInfos = new SinhVienContext().GetAllSinhVien();
                var listNganh = new MaNganhViewModel().GetMaNganh();

                ViewBag.AccInfo = accInfo;
                ViewBag.Nganhs = listNganh;
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

        public ActionResult InsertSinhVien(SinhVienViewModel sinhVien)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                DBConnection.GetSqlConnection(connectionString); //Mở

                var insertCheck = new SinhVienContext().InsertSinhVien(sinhVien);
                var contents = JsonConvert.SerializeObject(insertCheck);

                DBConnection.GetSqlConnection(connectionString); //Đóng

                CResponseMessage crMess = new CResponseMessage();
                crMess = JsonConvert.DeserializeObject<CResponseMessage>(contents);
                if (crMess.Code == 0)
                {
                    return Json(contents);
                }

                return Json(contents);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
