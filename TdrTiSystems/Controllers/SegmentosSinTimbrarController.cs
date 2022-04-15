using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaNegocio;
using TdrTiSystems.Filter;

namespace TdrTiSystems.Controllers
{
    
    public class SegmentosSinTimbrarController : Controller
    {
        [ServiceFilter(typeof(Seguridad))]
        public IActionResult Index()
        {
            return View();
        }
        public List<SegmentosSinTimbrarCLS> listarSegmentos()
        {
            SegmentosSinTimbrarBL obj = new SegmentosSinTimbrarBL();
            return obj.listarSegmentos();
        }
        public List<SegmentosSinTimbrarCLS> filtrarCpS(string segmento,string cliente)
        {
            SegmentosSinTimbrarBL obj = new SegmentosSinTimbrarBL();
            return obj.filtrarCpS(segmento,cliente);
        }
    }
}
