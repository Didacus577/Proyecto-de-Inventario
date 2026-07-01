using Microsoft.AspNetCore.Mvc;
using Inventario.BLL.Interfaces;
using Inventario.AppWeb.Models.ViewModels;
using AutoMapper;


namespace Inventario.AppWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;

        public DashboardController(IDashboardService dashboardService, IMapper mapper)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
        }

      
        public IActionResult Dashborard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TopSalidas()
        {
            var lista = await _dashboardService.TopSalidas();

           
            var data = lista.Select(m => _mapper.Map<VMMovimiento>(m)).ToList();

            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> TopEntradas()
        {
            var lista = await _dashboardService.TopEntradas();

            var data = lista.Select(m => _mapper.Map<VMMovimiento>(m)).ToList();

            return Json(new { data });
        }
    }
}
