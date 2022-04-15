using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using CapaEntidad;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class SucursalDAL
    {
        public int guardarSucursal(SucursalCLS oSucursalCLS)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var bd = root.GetConnectionString("cn");
            //error
            //Rpta 0 va a ser error
            int rpta = 0;
            using (SqlConnection cn = new SqlConnection(bd))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspGuardarSucursal", cn))
                    {
                        //Indico que es Procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@iidsucursal", oSucursalCLS.iidsucursal);
                        cmd.Parameters.AddWithValue("@nombre", oSucursalCLS.nombre);
                        cmd.Parameters.AddWithValue("@direccion", oSucursalCLS.direccion);
                        cmd.Parameters.AddWithValue("@fotosucursal", oSucursalCLS.foto);
                        cmd.Parameters.AddWithValue("@nombrefotosucursal", oSucursalCLS.nombrefoto);
                        rpta = cmd.ExecuteNonQuery();

                    }
                }

                catch (Exception ex)
                {
                    cn.Close();
                    rpta = 0;
                }


            }
            return rpta;

        }
        public List<SucursalCLS> listarSucursal()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var bd = root.GetConnectionString("cn");
            List<SucursalCLS> lista = null;
            using (SqlConnection cn = new SqlConnection(bd))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspListarSucursal", cn))
                    {
                        //Le indico que es del itpo procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        //Ejecutamos el procedimiento
                        SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        //Recorremos el arreglo
                        if (drd != null)
                        {
                            lista = new List<SucursalCLS>();
                            int posId = drd.GetOrdinal("IIDSUCURSAL");
                            int posNombre = drd.GetOrdinal("NOMBRE");
                            int posDireccion = drd.GetOrdinal("DIRECCION");
                            SucursalCLS oSucursalCLS;
                            while (drd.Read())
                            {
                                oSucursalCLS = new SucursalCLS();
                                oSucursalCLS.iidsucursal = drd.IsDBNull(posId) ? 0 : drd.GetInt32(0);
                                oSucursalCLS.nombre = drd.IsDBNull(posNombre) ? "" : drd.GetString(posNombre);
                                oSucursalCLS.direccion = drd.IsDBNull(posDireccion) ? "" : drd.GetString(posDireccion);
                                lista.Add(oSucursalCLS);

                            }
                            cn.Close();

                        }
                    }
                }
                catch (Exception ex)
                {

                    cn.Close();
                    lista = null;
                }
            }

            return lista;

        }

        public List<SucursalCLS> filtrarSucursal(string nombresucursal)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var bd = root.GetConnectionString("cn");
            List<SucursalCLS> lista = null;
            using (SqlConnection cn = new SqlConnection(bd))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspFiltrarSucursal", cn))
                    {
                        //Le indico que es del itpo procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        //Esta linea define un parametro
                        cmd.Parameters.AddWithValue("@nombresucursal", nombresucursal == null ? "" : nombresucursal);
                        //Ejecutamos el procedimiento
                        SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        //Recorremos el arreglo
                        if (drd != null)
                        {
                            lista = new List<SucursalCLS>();
                            int posId = drd.GetOrdinal("IIDSUCURSAL");
                            int posNombre = drd.GetOrdinal("NOMBRE");
                            int posDireccion = drd.GetOrdinal("DIRECCION");
                            SucursalCLS oSucursalCLS;
                            while (drd.Read())
                            {
                                oSucursalCLS = new SucursalCLS();
                                oSucursalCLS.iidsucursal = drd.IsDBNull(posId) ? 0 : drd.GetInt32(0);
                                oSucursalCLS.nombre = drd.IsDBNull(posNombre) ? "" : drd.GetString(posNombre);
                                oSucursalCLS.direccion = drd.IsDBNull(posDireccion) ? "" : drd.GetString(posDireccion);
                                lista.Add(oSucursalCLS);

                            }
                            cn.Close();

                        }
                    }
                }
                catch (Exception ex)
                {

                    cn.Close();
                    lista = null;
                }
            }

            return lista;

        }
    }
}