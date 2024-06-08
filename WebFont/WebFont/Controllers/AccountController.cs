using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebFont.Models;

namespace WebFont.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController()
        {
            _httpClient = new HttpClient();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
            try
            {
                string apiUrl = "https://localhost:44380/api/UserPostData/Login";

                LoginAcc loginuser = new LoginAcc { Username = username, Password = HashPassword(username,password) };

                string json = JsonConvert.SerializeObject(loginuser);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, data);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    User user = JsonConvert.DeserializeObject<User>(responseData);

                    return View("Profile", user);
                }
                else
                {
                    return View("LoginFailed");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(string username, string password, string confirmPassword)
        {
            if (username == null || password == null)
            {
                ViewBag.Error = "Tên đăng nhập và mật khẩu không được để trống.";
                return View();
            }
            if (password!= confirmPassword)
            {
                ViewBag.Error = "Xác nhận mật khẩu bị sai.";
                return View();
            }

            List<UsernameResponse> lusername;
            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:44380/api/UserPostData/Get_UserName");
            string responsData = await response.Content.ReadAsStringAsync();
            lusername = JsonConvert.DeserializeObject<List<UsernameResponse>>(responsData);
            foreach (var userName in lusername)
            {
                if (username == userName.Username1)
                {
                    ViewBag.Error = "Tên đăng nhập đã tồn tại.";
                    return View();
                }
            }

            LoginAcc newUser = new LoginAcc();
            newUser.Username = username;
            newUser.Password = HashPassword(username,password);

            string json = JsonConvert.SerializeObject(newUser);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response1 = await _httpClient.PostAsync("https://localhost:44380/api/UserPostData/POST_CreateAccUser", data);

            if (response1.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            else
            {
                string errorMessage = await response1.Content.ReadAsStringAsync();
                ViewBag.Error = "Không thể tạo tài khoản: " + errorMessage;
                return View();
            }
        }




        public static string HashPassword(string username, string password)
        {
            string input = username + password;

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public class UsernameResponse
        {
            public string Username1 { get; set; }
        }


    }
}