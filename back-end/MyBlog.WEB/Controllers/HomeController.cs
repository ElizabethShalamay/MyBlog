using System.Web.Mvc;

namespace MyBlog.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View(); // redirect to help page
        }
    }
}
