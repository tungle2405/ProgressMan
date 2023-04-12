﻿using CoreLib.Common;
using Microsoft.AspNetCore.Mvc;
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
                    && HttpContext.Session.GetString("user").Substring(0, 2) != "NV")
                {
                    return RedirectToAction("Index", "LoginGV");
                }

                var session = HttpContext.Session.GetString("user");

                DBConnection.GetSqlConnection(connectionString); //mở

                var accinfo = new ThongTinTKContext().GetThongTin(session);
                giangVienInfos = new GiangVienContext().GetAllGiangVien();

                ViewBag.accinfo = accinfo;
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

        public IActionResult GetAll()
        {
            return View();
        }
    }
}