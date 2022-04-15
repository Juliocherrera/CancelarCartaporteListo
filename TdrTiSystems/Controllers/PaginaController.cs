using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using TdrTiSystems.Filter;

namespace TdrTiSystems.Controllers
{
	
	public class PaginaController : Controller
	{
		[ServiceFilter(typeof(Seguridad))]
		public IActionResult Index()
		{
			return View();
		}
		
		public List<PaginaCLS> listarPagina()
		{
			PaginaBL oPaginaBL = new PaginaBL();
			return oPaginaBL.listarPagina();
		}

		public PaginaCLS recuperarPagina(int iidpagina)
		{
			PaginaBL oPaginaDAL = new PaginaBL();
			return oPaginaDAL.recuperarPagina(iidpagina);

		}
		public int eliminarPagina(int iidpagina)
		{
			PaginaBL oPaginaDAL = new PaginaBL();
			return oPaginaDAL.eliminarPagina(iidpagina);
		}

		public int guardarPagina(PaginaCLS oPaginaCLS)
		{
			PaginaBL oPaginaDAL = new PaginaBL();
			return oPaginaDAL.guardarPagina(oPaginaCLS);
		}



	}
}
