using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class SucursalBL
    {
        public List<SucursalCLS> listarSucursal()
        {
            SucursalDAL obj = new SucursalDAL();
            return obj.listarSucursal();
        }
        public List<SucursalCLS> filtrarSucursal(string nombresucursal)
        {
            SucursalDAL obj = new SucursalDAL();
            return obj.filtrarSucursal(nombresucursal);
        }
        public int guardarSucursal(SucursalCLS oSucursalCLS)
        {
            SucursalDAL obj = new SucursalDAL();
            return obj.guardarSucursal(oSucursalCLS);
        }
    }
}