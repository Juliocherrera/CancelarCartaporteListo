using System;
namespace CapaEntidad
{
    public class TipoUsuarioCLS
    {
        public int iidtipousuario { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public List<int> iidpaginas { get; set; }

    }
}