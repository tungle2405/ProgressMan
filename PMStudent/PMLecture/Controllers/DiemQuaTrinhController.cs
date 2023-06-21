using CoreLib.Common;
using Microsoft.AspNetCore.Mvc;
using PMLecture.Context;
using PMLecture.Models;

namespace PMLecture.Controllers
{
    public class DiemQuaTrinhController : Controller
    {
        private readonly IConfiguration _configuration;
        public DiemQuaTrinhController(IConfiguration Configuration)
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
                ViewBag.Side = "DiemQuaTrinh";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(monHocInfos);

            //return View();
        }

        public IActionResult IndexDiemQuaTrinhSV(string MaLopMonHoc)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<SinhVienViewModel> sinhVienInfos = new List<SinhVienViewModel>();
            List<DiemChuyenCanViewModel> diemCC = new List<DiemChuyenCanViewModel>();
            List<DiemQuaTrinhViewModel> diemQT = new List<DiemQuaTrinhViewModel>();

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
                diemQT = new DiemDanhContext().ListDiemQuaTrinh(MaLopMonHoc);

                //ViewBag.DonVi = donviInfo;
                ViewBag.AccInfo = accinfo;
                //ViewBag.LopNienChes = listLopNienChe;
                ViewBag.MaLopMonHoc = MaLopMonHoc;
                //ViewBag.GiangViens = listGiangVien;
                ViewBag.Side = "DiemQuaTrinh";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(diemQT);

            //return View();
        }
    }
}
