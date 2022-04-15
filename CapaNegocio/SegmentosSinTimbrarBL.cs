using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class SegmentosSinTimbrarBL
    {
        public List<SegmentosSinTimbrarCLS> listarSegmentos()
        {
            SegmentosSinTimbrarDAL obj = new SegmentosSinTimbrarDAL();
            return obj.listarSegmentos();
        }
        public List<SegmentosSinTimbrarCLS> filtrarCpS(string segmento,string cliente)
        {
            SegmentosSinTimbrarDAL obj = new SegmentosSinTimbrarDAL();
            return obj.filtrarCpS(segmento,cliente);
        }
    }
}