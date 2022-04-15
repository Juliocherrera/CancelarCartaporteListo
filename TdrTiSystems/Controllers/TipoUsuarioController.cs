using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using TdrTiSystems.Filter;

namespace TdrTiSystems.Controllers
{
    
    public class TipoUsuarioController : Controller
    {
        [ServiceFilter(typeof(Seguridad))]
        public List<TipoUsuarioCLS> filtrarTipoUsuario(TipoUsuarioCLS obj)
        {
            TipoUsuarioBL oTipoUsuarioBL = new TipoUsuarioBL();
            return oTipoUsuarioBL.filtrarTipoUsuario(obj);
        }

        public IActionResult Index()
        {
            return View();
        }

        public int guardarTipoUsuario(TipoUsuarioCLS oTipoUsuarioCLS)
        {
            TipoUsuarioBL obj = new TipoUsuarioBL();
            return obj.guardarDatos(oTipoUsuarioCLS);
        }

        public TipoUsuarioCLS recuperarTipoUsuario(int iidtipousuario)
        {
            TipoUsuarioBL obj = new TipoUsuarioBL();
            return obj.recuperarTipoUsuario(iidtipousuario);
        }

        public List<TipoUsuarioCLS> listarTipoUsuario()
        {
            TipoUsuarioBL obj = new TipoUsuarioBL();
            return obj.listarTipoUsuario();
        }
    }

}
