using Microsoft.AspNetCore.Mvc;

namespace ExcelFileRead.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index(string email, string password)
        {
            if(email == "admin@amfco.net")
            {
                if (password == "amfco@123")
                {
                    //HttpContext.Session.SetString("UserInfo", "admin@amfco.net");
                    return RedirectToAction("Index", "Home");
                }
            }
            
            return View();

        }

    }
}
