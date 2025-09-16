using Microsoft.AspNetCore.Mvc;
using RouletteBetting.Models;

namespace RouletteBetting.Controllers
{
    public class TestDbController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestDbController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var count = _context.Users.Count();
                return Content($"Conexión exitosa. Usuarios en la BD: {count}");
            }
            catch (Exception ex)
            {
                return Content($"Error al conectar: {ex.Message}");
            }
        }
    }
}
