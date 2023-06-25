using CoreLib.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using PMStudent.Models;
using Microsoft.AspNetCore.Http;

namespace PMStudent.Controllers
{
    public class DiemDanhController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7267/api");
        HttpClient client;
        public DiemDanhController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public IActionResult Index()
        {

            List<LopMonHocViewModel> lopMonHoc = new List<LopMonHocViewModel>();

            try
            {
                if (HttpContext.Session.GetString("user") == null)
                {
                    return RedirectToAction("Index", "LoginGV");
                }

                var session = HttpContext.Session.GetString("user");

                string data = JsonConvert.SerializeObject(session);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = client.PostAsync(client.BaseAddress + "/getlopmonhoc", content).Result;
                string contents = response.Content.ReadAsStringAsync().Result;

                lopMonHoc = JsonConvert.DeserializeObject<List<LopMonHocViewModel>>(contents);

                if (lopMonHoc.Count != 0)
                {
                    ViewBag.AccInfo = lopMonHoc[0].MaSinhVien + " - " + lopMonHoc[0].TenSinhVien;
                }
                else
                {
                    ViewBag.AccInfo = session;
                }


                ViewBag.MaSinhVien = session;
                ViewBag.Side = "DiemDanh";

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(lopMonHoc);


        }

        public IActionResult IndexDiemDanh(string maLopMonHoc)
        {

            List<DiemDanhViewModel> diemDanh = new List<DiemDanhViewModel>();

            try
            {
                if (HttpContext.Session.GetString("user") == null)
                {
                    return RedirectToAction("Index", "LoginGV");
                }

                var session = HttpContext.Session.GetString("user");

                LopMonHocViewModel objDetails = new LopMonHocViewModel();
                objDetails.MaLopMonHoc = maLopMonHoc;
                objDetails.MaSinhVien = session;

                //var obj = Tuple.Create(session, maLopMonHoc);

                string data = JsonConvert.SerializeObject(objDetails);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = client.PostAsync(client.BaseAddress + "/getalldiemdanh", content).Result;
                string contents = response.Content.ReadAsStringAsync().Result;

                diemDanh = JsonConvert.DeserializeObject<List<DiemDanhViewModel>>(contents);

                if(diemDanh.Count != 0)
                {
                    ViewBag.AccInfo = diemDanh[0].MaSinhVien + " - " + diemDanh[0].TenSinhVien;
                }
                else
                {
                    ViewBag.AccInfo = session;
                }
                ViewBag.MaSinhVien = session;
                ViewBag.Side = "DiemDanh";

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(diemDanh);


        }

        [HttpGet]
        public ActionResult GetLopMonHocBySinhVien(string maSinhVien)
        {
            try
            {
                StringContent content = new StringContent(maSinhVien, Encoding.UTF8, "application/json");
                var response = client.PostAsync(client.BaseAddress + "/getlopmonhoc", content).Result;
                string contents = response.Content.ReadAsStringAsync().Result;

                List<LopMonHocViewModel> lopMonHoc = new List<LopMonHocViewModel>();
                lopMonHoc = JsonConvert.DeserializeObject<List<LopMonHocViewModel>>(contents);

                var result = JsonConvert.DeserializeObject(contents);
                return Json(result);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult GetDiemDanh(string maSinhVien)
        {
            try
            {
                StringContent content = new StringContent(maSinhVien, Encoding.UTF8, "application/json");
                var response = client.PostAsync(client.BaseAddress + "/getalldiemdanh", content).Result;
                string contents = response.Content.ReadAsStringAsync().Result;

                List<DiemDanhViewModel> diemDanh = new List<DiemDanhViewModel>();
                diemDanh = JsonConvert.DeserializeObject<List<DiemDanhViewModel>>(contents);

                var result = JsonConvert.DeserializeObject(contents);
                return Json(result);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
