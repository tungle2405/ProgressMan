using CoreLib.Common;
using CoreLib.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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

                //Lấy ra thông tin tài khoản
                var accInfo = new ThongTinTKContext().GetThongTin(session);
                //Lấy ra các đơn vị để insert
                var donviInfo = new GiangVienContext().GetDonVi();
                //Lấy ra tất cả nhân viên phòng đào tạo
                giangVienInfos = new GiangVienContext().GetAllPhongDaoTao();

                ViewBag.AccInfo = accInfo;
                ViewBag.DonVi = donviInfo;
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
    }
}
