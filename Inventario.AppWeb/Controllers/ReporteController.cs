using Microsoft.AspNetCore.Mvc;

namespace Inventario.AppWeb.Controllers
{
    public class ReporteController : Controller
    {
        public IActionResult Reporte()
        {
            return View();
        }
    }
}
