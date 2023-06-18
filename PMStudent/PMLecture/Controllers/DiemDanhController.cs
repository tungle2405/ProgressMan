using CoreLib.Common;
using CoreLib.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMLecture.Context;
using PMLecture.Models;

namespace PMLecture.Controllers
{
    public class DiemDanhController : Controller
    {
        private readonly IConfiguration _configuration;
        public DiemDanhController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<LopMonHocViewModel> monHocInfos = new List<LopMonHocViewModel>();

            try
            {
                if (HttpContext.Session.GetString("user") == null)
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
                var listGiangVien = new GiangVienContext().GetAllGiangVien().Where(x => x.HoatDong == 0);

                //Lấy ra tất cả lớp môn học theo giảng viên
                monHocInfos = new LopMonHocContext().GetLopMonHocTheoGiangVien(session);

                //ViewBag.DonVi = donviInfo;
                ViewBag.AccInfo = accinfo;
                ViewBag.MonHocs = listMonHoc;
                ViewBag.GiangViens = listGiangVien;
                ViewBag.Side = "DiemDanh";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(monHocInfos);

            //return View();
        }

        public IActionResult IndexSVHocLopMH(string MaLopMonHoc)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<SinhVienViewModel> sinhVienInfos = new List<SinhVienViewModel>();

            try
            {
                if (HttpContext.Session.GetString("user") == null)
                {
                    return RedirectToAction("Index", "LoginGV");
                }

                var session = HttpContext.Session.GetString("user");

                DBConnection.GetSqlConnection(connectionString); //mở

                //Lấy ra thông tin tài khoản
                var accinfo = new ThongTinTKContext().GetThongTin(session);
                //Lấy ra các lớp niên chế để insert
                //var listLopNienChe = new SinhVienContext().GetAllSinhVien().GroupBy(a => a.LopNienChe,
                //    (key, group) => new { LopNienChe = key }).ToList();
                //chuyển list thành chuỗi array
                //var arrayStringLopNC = JsonConvert.SerializeObject(listLopNienChe);
                //ViewBag.arrayListLopNC = Json(arrayStringLopNC);

                //Lấy ra ca học và buổi học
                var caHocInfo = new DiemDanhContext().GetAllCaHoc();
                var buoiHocInfo = new DiemDanhContext().GetAllBuoiHoc();
                //Lấy ra tất cả sinh viên của lớp môn học
                sinhVienInfos = new LopMonHocContext().GetAllSinhVienHocLopMonHoc(MaLopMonHoc);


                ViewBag.CaHocInfos = caHocInfo;
                ViewBag.BuoiHocInfos = buoiHocInfo;
                ViewBag.AccInfo = accinfo;
                //ViewBag.LopNienChes = listLopNienChe;
                ViewBag.MaLopMonHoc = MaLopMonHoc;
                //ViewBag.GiangViens = listGiangVien;
                ViewBag.Side = "DiemDanh";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(sinhVienInfos);

            //return View();
        }

        public ActionResult DiemDanhSinhVien(DiemDanhViewModel diemDanh)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                DBConnection.GetSqlConnection(connectionString); //Mở

                var diemDanhCheck = new DiemDanhContext().DiemDanhSinhVien(diemDanh);
                var contents = JsonConvert.SerializeObject(diemDanhCheck);

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

            //return View();
        }
    }
}
