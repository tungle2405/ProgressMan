using CoreLib.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PMLecture.Context;
using PMLecture.Interfaces;
using PMLecture.Models;

namespace PMLecture.Controllers
{
    public class DiemChuyenCanController : Controller
    {
        private readonly IConfiguration _configuration;
        public DiemChuyenCanController(IConfiguration Configuration)
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
                //Kiểm tra xem có phải ADMIN hay PDT thì lấy hết ra tất cả lớp môn học
                //Còn nếu là Giảng Viên thì chỉ lấy ra mỗi giảng viên đó
                if (session == "ADMIN" || HttpContext.Session.GetString("user").Substring(0, 3) == "PDT")
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
                ViewBag.Session = session;
                ViewBag.Side = "DiemChuyenCan";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(monHocInfos);

            //return View();
        }

        public IActionResult IndexDiemChuyenCanSV(string MaLopMonHoc)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<SinhVienViewModel> sinhVienInfos = new List<SinhVienViewModel>();
            List<DiemChuyenCanViewModel> diemCC = new List<DiemChuyenCanViewModel>();

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
                
                //Lấy ra tất cả sinh viên của lớp môn học
                sinhVienInfos = new LopMonHocContext().GetAllSinhVienHocLopMonHoc(MaLopMonHoc);

                //Lấy ra điểm của tất cả sinh viên trong lớp môn học
                diemCC = new DiemDanhContext().ListDiemChuyenCan(MaLopMonHoc);

                //ViewBag.DonVi = donviInfo;
                ViewBag.AccInfo = accinfo;
                //ViewBag.LopNienChes = listLopNienChe;
                ViewBag.MaLopMonHoc = MaLopMonHoc;
                //ViewBag.GiangViens = listGiangVien;
                ViewBag.Side = "DiemChuyenCan";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(diemCC);

            //return View();
        }

        public IActionResult IndexXemLaiDiemCC()
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
                //Kiểm tra xem có phải ADMIN hay PDT thì lấy hết ra tất cả lớp môn học
                //Còn nếu là Giảng Viên thì chỉ lấy ra mỗi giảng viên đó
                if (session == "ADMIN" || HttpContext.Session.GetString("user").Substring(0, 3) == "PDT")
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
                ViewBag.Session = session;
                ViewBag.Side = "XemLaiDiemCC";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(monHocInfos);

            //return View();
        }

        public IActionResult IndexXemLaiDiemCCLopMH(string MaLopMonHoc, string? MaBuoiHoc)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<XemLaiDDViewModel> listDiemDanh = new List<XemLaiDDViewModel>();

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

                //Lấy ra ca học và buổi học
                var buoiHocInfo = new DiemDanhContext().GetAllBuoiHoc();

                //Lấy ra danh sách sinh viên đã điểm danh theo buổi học
                listDiemDanh = new DiemDanhContext().GetDiemDanhByBuoiHoc(MaLopMonHoc, MaBuoiHoc);

                ViewBag.BuoiHocInfos = buoiHocInfo;
                ViewBag.AccInfo = accinfo;
                ViewBag.MaLopMonHoc = MaLopMonHoc;
                ViewBag.Side = "DiemChuyenCan";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(listDiemDanh);

            //return View();
        }

        public List<XemLaiDDViewModel> GetDiemDanhByBuoiHoc(string MaLopMonHoc, string? MaBuoiHoc)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<XemLaiDDViewModel> listDiemDanh = new List<XemLaiDDViewModel>();

            try
            {
                var session = HttpContext.Session.GetString("user");

                DBConnection.GetSqlConnection(connectionString); //mở

                //Lấy ra thông tin tài khoản
                var accinfo = new ThongTinTKContext().GetThongTin(session);

                //Lấy ra ca học và buổi học
                var buoiHocInfo = new DiemDanhContext().GetAllBuoiHoc();

                //Lấy ra danh sách sinh viên đã điểm danh theo buổi học
                listDiemDanh = new DiemDanhContext().GetDiemDanhByBuoiHoc(MaLopMonHoc, MaBuoiHoc);

                ViewBag.BuoiHocInfos = buoiHocInfo;
                ViewBag.AccInfo = accinfo;
                ViewBag.Side = "DiemChuyenCan";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listDiemDanh;
        }
    }
}
