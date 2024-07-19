using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;


namespace ExcelFileRead.Controllers
{
    public class AccountController : Controller
    {


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}


