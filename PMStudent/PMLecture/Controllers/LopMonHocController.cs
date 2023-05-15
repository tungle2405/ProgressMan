using CoreLib.Common;
using CoreLib.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMLecture.Context;
using PMLecture.Models;

namespace PMLecture.Controllers
{
    public class LopMonHocController : Controller
    {
        private readonly IConfiguration _configuration;
        public LopMonHocController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<LopMonHocViewModel> monHocInfos = new List<LopMonHocViewModel>();

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
                //Lấy ra các môn học để insert
                var listMonHoc = new MonHocContext().GetAllMonHoc();
                //Lấy ra các giảng viên để insert
                var listGiangVien = new GiangVienContext().GetAllGiangVien();
                //Kiểm tra xem có phải ADMIN hay PDT thì lấy hết ra tất cả lớp môn học
                //Còn nếu là Giảng Viên thì chỉ lấy ra mỗi giảng viên đó
                if(session == "ADMIN" || HttpContext.Session.GetString("user").Substring(0, 3) == "PDT")
                {
                    //Lấy ra tất cả lớp môn học
                    monHocInfos = new LopMonHocContext().GetAllLopMonHoc(session);
                }
                else
                {
                    //Lấy ra tất cả lớp môn học theo giảng viên
                    monHocInfos = new LopMonHocContext().GetLopMonHocTheoGiangVien(session);
                }
                

                //ViewBag.DonVi = donviInfo;
                ViewBag.AccInfo = accinfo;
                ViewBag.MonHocs = listMonHoc;
                ViewBag.GiangViens = listGiangVien;
                ViewBag.Side = "LopMonHoc";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(monHocInfos);

            //return View();
        }

        public ActionResult InsertLopMonHoc(LopMonHocViewModel lopMonHoc)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                DBConnection.GetSqlConnection(connectionString); //Mở

                var insertCheck = new LopMonHocContext().InsertLopMonHoc(lopMonHoc);
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
