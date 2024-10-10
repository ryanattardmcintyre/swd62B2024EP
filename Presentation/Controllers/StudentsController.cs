using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Welcome()
        {
            return View();
        }
    }
}
