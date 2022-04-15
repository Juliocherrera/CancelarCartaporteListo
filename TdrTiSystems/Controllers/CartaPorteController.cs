using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaNegocio;
using TdrTiSystems.Filter;

namespace TdrTiSystems.Controllers
{
    
    public class CartaPorteController : Controller
    {
        [ServiceFilter(typeof(Seguridad))]
        public IActionResult Index()
        {
            return View();
        }
        public List<CartaPorteCLS> listarCartaPorte()
        {
            CartaPorteBL obj = new CartaPorteBL();
            return obj.listarCartaPorte();
        }
        public List<CartaPorteCLS> filtrarCartaPorte(string segmento)
        {
            CartaPorteBL obj = new CartaPorteBL();
            return obj.filtrarCartaPorte(segmento);
        }
    }
}
