﻿using aspnet_mvc_example.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspnet_mvc_example.Controllers
{
    [Authorize]
    public class EmployerController : Controller
    {
        // GET: Employer
        [RoleAuthenticationFilter(Roles = "ER")]
        public ActionResult Index()
        {
            return View();
        }
    }
}