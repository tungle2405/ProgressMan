using CoreLib.DAL;
using CoreLib.DTO;
using Microsoft.AspNetCore.Mvc;
using PMStudentApi.Model;
using System.Text;
using HashCode = CoreLib.DAL.HashCode;
using Newtonsoft.Json;
using PMStudent.Models;

namespace PMStudent.Controllers
{
    public class LoginController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7267/api");
        HttpClient client;
        public LoginController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        //[Area("LoginSV")]
        public IActionResult Index()
        {
            try
            {
                if (HttpContext.Session.GetString("user") != null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel user)
        {
            try
            {
                //encrypt password
                user.MatKhau = (user.MatKhau != null) ? new HashCode().Encrypt(user.MatKhau) : null;

                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = client.PostAsync(client.BaseAddress + "/login", content).Result;
                string contents = response.Content.ReadAsStringAsync().Result;

                CResponseMessage crMess = new CResponseMessage();
                crMess = JsonConvert.DeserializeObject<CResponseMessage>(contents);
                if (crMess.Code == 0)
                {
                    HttpContext.Session.SetString("user", crMess.Data);
                    //Session["user"] = crMess.Data;
                    return Json(contents);
                }
                else
                {
                    return Json(contents);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            //Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
