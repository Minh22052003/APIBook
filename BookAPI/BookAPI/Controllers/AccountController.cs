using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Web.Mvc;
using BookAPI.Models;

namespace BookAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelBook _dbContext;

        public AccountController(ModelBook dbContext)
        {
            _dbContext = dbContext;
        }
        public AccountController()
        {
            _dbContext = new ModelBook();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string tenDangNhap, string matKhau)
        {
            if(tenDangNhap == "admin@gmail.com" && matKhau == "2205")
            {
                return View("Profile");
            }
            //Thêm xác thực
            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không chính xác.";
            return View();
        }
        public ActionResult Profile()
        {
            return View();
        }

        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return RedirectToAction("Index", "Home");
        //}
    }
}
