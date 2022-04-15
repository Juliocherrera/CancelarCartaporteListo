using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace TdrTiSystems.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public List<PaginaCLS> listarMenu(int iidtipousuario)
        {
            PaginaBL oPaginaBL = new PaginaBL();
            return oPaginaBL.listarMenu(iidtipousuario);
        }
        public IActionResult Cerrarsesion()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("persona");
            return RedirectToAction("Index");
        }
        public PersonaCLS login(string usuario, string contra)
        {
            PersonaBL oPersonaBL = new PersonaBL();
            PersonaCLS oPersonaCLS= oPersonaBL.login(usuario, contra);
            if (oPersonaCLS.iidusuario != 0)
            {
                string objCadena = JsonConvert.SerializeObject(oPersonaCLS);
                HttpContext.Session.SetString("persona", objCadena);
                int iidtipousuario = oPersonaCLS.iidtipousuario;
                List<PaginaCLS> listapagina = listarMenu(iidtipousuario);
                string objLista = JsonConvert.SerializeObject(listapagina);
                HttpContext.Session.SetString("menu", objLista);
            }
            else
            {
                HttpContext.Session.Remove("persona");
            }
            return oPersonaCLS;
        }
    }
}
