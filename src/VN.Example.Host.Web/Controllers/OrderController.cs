using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VN.Example.Host.Web.Models;

namespace VN.Example.Host.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult CreateOrder()
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[2].ToString();

            var pageName = "Order";
            var userAgent = Request.Headers["User-Agent"];

            var result = new
            {
                ip,
                pageName,
                userAgent
            };

            return Ok(result);
        }

        public IActionResult Confirmation()
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
