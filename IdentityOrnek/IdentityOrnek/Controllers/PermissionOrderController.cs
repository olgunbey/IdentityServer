using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityOrnek.Controllers
{
    public class PermissionOrderController : Controller
    {
        [Authorize(Policy = "OrderPermissionReadAndDelete")]
        public IActionResult Permission()
        {
            return View();
        }
    }
}
