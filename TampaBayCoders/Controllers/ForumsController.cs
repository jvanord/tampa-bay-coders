using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TampaBayCoders.Controllers
{
	[Authorize]
    public class ForumsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}