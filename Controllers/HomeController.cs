using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using encdec.Models;
using Microsoft.AspNetCore.DataProtection;

namespace encdec.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataProtector _protector;

        private List<Employee> _employees;

        public HomeController(ILogger<HomeController> logger,
                            IDataProtectionProvider provider)
        {
            _logger = logger;
            _protector = provider.CreateProtector("mysecretkey");

            _employees = new List<Employee>{
                new Employee{Id = 1, FullName="First Employee"},
                new Employee{Id = 2, FullName="Second Employee"},
                new Employee{Id = 3, FullName="Third Employee"}
            };

            _employees = _employees.Select(x =>
            {
                x.DisplayId = _protector.Protect(x.Id.ToString());
                return x;
            }).ToList();
        }

        public IActionResult Index()
        {
            return View(_employees);
        }

        public IActionResult Details(string id)
        {
            var originalId = Convert.ToInt32(_protector.Unprotect(id));

            var employee = _employees.FirstOrDefault(x => x.Id == originalId);

            return View(employee);
        }

        [HttpPost]
        public IActionResult Encrypt(string data)
        {
            TempData["Result"] = _protector.Protect(data);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Decrypt(string data)
        {
            TempData["Result"] = _protector.Unprotect(data);
            return RedirectToAction(nameof(Index));
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
