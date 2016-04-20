using System.Web.Mvc;

namespace CustomAppDomainWebSample.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string plugins)
        {
            string textFromPlugin = string.Empty;
            using (PluginsActivator activator = new PluginsActivator())
            {
                var classFromPlugin = activator.LoadAssembly(plugins);
                textFromPlugin = classFromPlugin.DoSomething();
            }

            return View(model:textFromPlugin);
        }
    }
}