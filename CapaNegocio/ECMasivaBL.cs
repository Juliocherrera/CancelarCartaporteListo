using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class ECMasivaBL
    {
        public List<ECMasivaCLS> listarMasiva()
        {
            ECMasivaDAL obj = new ECMasivaDAL();
            return obj.listarMasiva();
        }
       

    }
}