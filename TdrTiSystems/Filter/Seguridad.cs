using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CapaEntidad;

namespace TdrTiSystems.Filter
{
    public class Seguridad : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string usu = context.HttpContext.Session.GetString("persona");
           string cadenaMenu = context.HttpContext.Session.GetString("menu");
            if (cadenaMenu != null)
            {
                List<PaginaCLS> listamenu = JsonConvert.DeserializeObject<List<PaginaCLS>>(cadenaMenu);
                string nombreController = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
                string accionController = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;

                int cantidad = listamenu.Where(p => p.controlador.ToUpper() == nombreController.ToUpper() && p.accion.ToUpper() == accionController.ToUpper()).Count();
                if (cantidad==0)
                {
                    context.Result = new RedirectResult("Login");
                }
            }
          
            if (usu==null)
            {
                context.Result = new RedirectResult("Login");
            }
        }
    }
}
