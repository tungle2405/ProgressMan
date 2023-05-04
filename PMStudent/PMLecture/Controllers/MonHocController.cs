﻿using CoreLib.Common;
using Microsoft.AspNetCore.Mvc;
using PMLecture.Context;
using PMLecture.Models;

namespace PMLecture.Controllers
{
    public class MonHocController : Controller
    {
        private readonly IConfiguration _configuration;
        public MonHocController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<MonHocViewModel> monHocInfos = new List<MonHocViewModel>();

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
                //Lấy ra các đơn vị để insert
                //var donviInfo = new GiangVienContext().GetDonVi();
                //Lấy ra tất cả giảng viên
                monHocInfos = new MonHocContext().GetAllMonHoc();

                //ViewBag.DonVi = donviInfo;
                ViewBag.AccInfo = accinfo;
                ViewBag.Side = "MonHoc";

                DBConnection.GetSqlConnection(connectionString); //đóng
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LoginGV");
            }

            return View(monHocInfos);

            //return View();
        }
    }
}