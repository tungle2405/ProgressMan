using CoreLib.Common;
using CoreLib.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMLecture.Context;
using PMLecture.Models;

namespace PMLecture.Controllers
{
    public class GiangVienController : Controller
    {
        private readonly IConfiguration _configuration;
        public GiangVienController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<GiangVienViewModel> giangVienInfos = new List<GiangVienViewModel>();

            try
            {
                if (HttpContext.Session.GetString("user") == null || HttpContext.Session.GetString("user") != "ADMIN"
                    && HttpContext.Session.GetString("user").Substring(0, 3) != "PDT")
                {
                    return RedirectToAction("Index", "LoginGV");
                }

                var session = HttpContext.Session.GetString("user");

                DBConnection.GetSqlConnection(connectionString); //mở

                //Lấy ra thông tin tài khoản
                var accinfo = new ThongTinTKContext().GetThongTin(session);
                //Lấy ra các đơn vị để insert
                var donviInfo = new GiangVienContext().GetDonVi();
                //Lấy ra tất cả giảng viên
                giangVienInfos = new GiangVienContext().GetAllGiangVien();

                ViewBag.DonVi = donviInfo;
                ViewBag.AccInfo = accinfo;
                ViewBag.side = "GiangVien";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(giangVienInfos);

            //return View();
        }

        public ActionResult InsertEmployee(GiangVienViewModel phongDaoTao)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                DBConnection.GetSqlConnection(connectionString); //Mở

                var insertCheck = new GiangVienContext().InsertNhanVien(phongDaoTao);
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

        public IActionResult GetAll()
        {
            return View();
        }
    }
}
