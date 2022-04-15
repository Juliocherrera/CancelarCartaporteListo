using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CartaPorteBL
    {
        public List<CartaPorteCLS> listarCartaPorte()
        {
            CartaPorteDAL obj = new CartaPorteDAL();
            return obj.listarCartaPorte();
        }
        public List<CartaPorteCLS> filtrarCartaPorte(string segmento)
        {
            CartaPorteDAL obj = new CartaPorteDAL();
            return obj.filtrarCartaPorte(segmento);
        }
    }
}