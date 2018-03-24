using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MemoryCacheAspNetCore.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace MemoryCacheAspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private IMemoryCache _cache;
        public HomeController(IMemoryCache cache)
        {
            _cache = cache;
        }

        public JsonResult GetAllCustomers()
        {
            var customers = new List<CustomerViewModel>();
            if (!_cache.TryGetValue("CustomersList", out customers))
            {
                if (customers == null)
                {
                    customers = AllCustomer();
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
            // Keep in cache for this time, reset time if accessed.
            .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                _cache.Set("CustomersList", customers, cacheEntryOptions);
            }
            return Json(customers);
        }

        private List<CustomerViewModel> AllCustomer()
        {
            var customers = new List<CustomerViewModel>();
            customers.Add(new CustomerViewModel() { Id = 1, Name = "Kalpesh" });
            customers.Add(new CustomerViewModel() { Id = 2, Name = "Ajay" });
            return customers;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
