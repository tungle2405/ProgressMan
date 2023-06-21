using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMStudent.Models;
using System.Text;

namespace PMStudent.Controllers
{
    public class DiemQuaTrinhController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7267/api");
        HttpClient client;
        public DiemQuaTrinhController()
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
                ViewBag.Side = "DiemQuaTrinh";

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(lopMonHoc);


        }

        public IActionResult IndexDiemQuaTrinh(string maLopMonHoc)
        {

            DiemQuaTrinhViewModel diemQT = new DiemQuaTrinhViewModel();
            List<DiemQuaTrinhViewModel> listDiemQT = new List<DiemQuaTrinhViewModel>();

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
                var response = client.PostAsync(client.BaseAddress + "/getdiemquatrinh", content).Result;
                string contents = response.Content.ReadAsStringAsync().Result;

                diemQT = JsonConvert.DeserializeObject<DiemQuaTrinhViewModel>(contents);
                listDiemQT.Add(diemQT);

                if (diemQT != null)
                {
                    ViewBag.AccInfo = diemQT.MaSinhVien + " - " + diemQT.TenSinhVien;
                }
                else
                {
                    ViewBag.AccInfo = session;
                }
                ViewBag.MaSinhVien = session;
                ViewBag.Side = "DiemQuaTrinh";

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(listDiemQT);


        }
    }
}
