using CoreLib.Common;
using CoreLib.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMLecture.Context;
using PMLecture.Models;

namespace PMLecture.Controllers
{
    public class MonHocController : Controller
    {
        private readonly IConfiguration _configuration;
        public MonHocController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<MonHocViewModel> monHocInfos = new List<MonHocViewModel>();

            try
            {
                if (HttpContext.Session.GetString("user") == null || HttpContext.Session.GetString("user") != "ADMIN")
                {
                    return RedirectToAction("Index", "LoginGV");
                }

                var session = HttpContext.Session.GetString("user");

                DBConnection.GetSqlConnection(connectionString); //mở

                //Lấy ra thông tin tài khoản
                var accinfo = new ThongTinTKContext().GetThongTin(session);
                //Lấy ra các mã môn học để insert
                var listMaMonHoc = new MaNganhViewModel().GetMaMonHoc();
                //Lấy ra tất cả môn học
                monHocInfos = new MonHocContext().GetAllMonHoc();

                //ViewBag.DonVi = donviInfo;
                ViewBag.AccInfo = accinfo;
                ViewBag.MaMonHocs = listMaMonHoc;
                ViewBag.Side = "MonHoc";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(monHocInfos);

            //return View();
        }

        public ActionResult InsertMonHoc(MonHocViewModel monHoc)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                DBConnection.GetSqlConnection(connectionString); //Mở

                var insertCheck = new MonHocContext().InsertMonHoc(monHoc);
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
