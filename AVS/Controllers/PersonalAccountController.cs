﻿using AVS.Models.AddressModels;
using AVS.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AVS.Controllers
{
    //[Authorize(Policy = "UserPolicy")]
    public class PersonalAccountController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Message()
        {
            return View();
        }
    }
}
