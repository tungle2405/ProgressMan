using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMLecture.Models;
using PMStudent.Models;
using System.Diagnostics;
using System.Text;

namespace PMStudent.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        Uri baseAddress = new Uri("https://localhost:7267/api");
        HttpClient client;
        public HomeController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public IActionResult Index()
        {
            try
            {
                SinhVienViewModel sinhVien = new SinhVienViewModel();

                if (HttpContext.Session.GetString("user") == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                var session = HttpContext.Session.GetString("user");

                string data = JsonConvert.SerializeObject(session);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = client.PostAsync(client.BaseAddress + "/getthongtinsv", content).Result;
                string contents = response.Content.ReadAsStringAsync().Result;

                sinhVien = JsonConvert.DeserializeObject<SinhVienViewModel>(contents);

                if(sinhVien == null)
                {
                    ViewBag.AccInfo = "";
                }

                ViewBag.AccInfo = sinhVien.MaSinhVien + " - " + sinhVien.HoTen;
                ViewBag.MaSinhVien = session;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}