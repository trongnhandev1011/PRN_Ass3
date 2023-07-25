using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Diagnostics;
using ODataBookStore.Web.Models;

namespace ODataBookStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string endpoint = "https://localhost:1433";

        public HomeController(ILogger<HomeController> logger)
        {
            

            _logger = logger;
        }

        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Book");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel request)
        {
            if(User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Book");
            }
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage res = await httpClient.PostAsJsonAsync<LoginModel>(endpoint + "/auth/login", request);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var token = res.Content.ReadFromJsonAsync<LoginResponse>().Result.AccessToken;
                    Response.Cookies.Append("token", token);
                    return RedirectToAction("Index", "Book");
                }
                TempData["Message"] = "username or password is not correct!";
                return View(request);
            }
        }

        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Book");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                TempData["Message"] = "Password and confirm password are not match";
                return View();
            }

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Book");
            }
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage res = await httpClient.PostAsJsonAsync<RegisterModel>(endpoint + "/auth/register", request);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction("Login");
                }
                return View(request);
            }
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return RedirectToAction("Login");
        }
    }

    public class LoginResponse
    {
        public string AccessToken { get; set;}
    }
}
