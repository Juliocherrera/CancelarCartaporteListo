using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using TdrTiSystems.Filter;

namespace TdrTiSystems.Controllers
{
    public class SucursalController : Controller
    {
        [ServiceFilter(typeof(Seguridad))]
        public IActionResult Index()
        {
            return View();
        }
        //Creamos un metodo que se comunique con la capa de negocio
        public List<SucursalCLS> listarSucursal()
        {
            SucursalBL obj = new SucursalBL();
            return obj.listarSucursal();
        }

        public List<SucursalCLS> filtrarSucursal(string nombresucursal)
        {
            SucursalBL obj = new SucursalBL();
            return obj.filtrarSucursal(nombresucursal);
        }

        public int GuardarDatos(SucursalCLS oSucursalCLS, IFormFile fotoEnviar)
        {
            byte[] buffer = null;
            string nombreFoto = "";
            using(MemoryStream ms = new MemoryStream())
            {
                fotoEnviar.CopyTo(ms);
                nombreFoto = fotoEnviar.FileName;
                buffer = ms.ToArray();
                oSucursalCLS.foto = buffer;
                oSucursalCLS.nombrefoto = nombreFoto;
            }
            SucursalBL obj = new SucursalBL();
            return obj.guardarSucursal(oSucursalCLS);
        }
    }
}
