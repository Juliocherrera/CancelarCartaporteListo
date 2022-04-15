using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using CapaEntidad;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class PaginaDAL
    {
        public List<PaginaCLS> listarMenu(int iidtipousuario)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var bd = root.GetConnectionString("cn");

            List<PaginaCLS> lista = null;
            using (SqlConnection cn = new SqlConnection(bd))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspListarMenu", cn))
                    {
                        //Le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@iidtipousuario", iidtipousuario);
                        SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        if (drd != null)
                        {
                            lista = new List<PaginaCLS>();
                            int posId = drd.GetOrdinal("IIDPAGINA");
                            int posMensaje = drd.GetOrdinal("MENSAJE");
                            int posControlador = drd.GetOrdinal("CONTRALADOR");
                            int posAccion = drd.GetOrdinal("ACCION");
                            PaginaCLS oPaginaCLS;
                            while (drd.Read())
                            {
                                oPaginaCLS = new PaginaCLS();
                                oPaginaCLS.iidpagina = drd.IsDBNull(posId) ? 0 : drd.GetInt32(posId);
                                oPaginaCLS.mensaje = drd.IsDBNull(posMensaje) ? "" : drd.GetString(posMensaje);
                                oPaginaCLS.controlador = drd.IsDBNull(posControlador) ? "" : drd.GetString(posControlador);
                                oPaginaCLS.accion = drd.IsDBNull(posAccion) ? "" : drd.GetString(posAccion);
                                lista.Add(oPaginaCLS);
                            }
                            cn.Close();
                        }

                    }

                }
                catch (Exception ex)
                {
                    cn.Close();
                    //null para mi es error
                    lista = null;
                }

            }


            return lista;
        }
        public int guardarPagina(PaginaCLS oPaginaCLS)
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
                    using (SqlCommand cmd = new SqlCommand("uspGuardarPagina", cn))
                    {
                        //Indico que es Procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@iidpagina", oPaginaCLS.iidpagina);
                        cmd.Parameters.AddWithValue("@mensaje", oPaginaCLS.mensaje);
                        cmd.Parameters.AddWithValue("@controlador", oPaginaCLS.controlador);
                        cmd.Parameters.AddWithValue("@accion", oPaginaCLS.accion);
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

        public PaginaCLS recuperarPagina(int iidpagina)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var bd = root.GetConnectionString("cn");
            PaginaCLS oPaginaCLS = new PaginaCLS();
            using (SqlConnection cn = new SqlConnection(bd))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspRecuperarPagina", cn))
                    {
                        //Le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@iidpagina", iidpagina);

                        SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        if (drd != null)
                        {

                            int posId = drd.GetOrdinal("IIDPAGINA");
                            int posMensaje = drd.GetOrdinal("MENSAJE");
                            int posControlador = drd.GetOrdinal("CONTRALADOR");
                            int posAccion = drd.GetOrdinal("ACCION");
                            while (drd.Read())
                            {

                                oPaginaCLS.iidpagina = drd.IsDBNull(posId) ? 0 : drd.GetInt32(posId);
                                oPaginaCLS.mensaje = drd.IsDBNull(posMensaje) ? "" : drd.GetString(posMensaje);
                                oPaginaCLS.controlador = drd.IsDBNull(posControlador) ? "" : drd.GetString(posControlador);
                                oPaginaCLS.accion = drd.IsDBNull(posAccion) ? "" : drd.GetString(posAccion);
                            }
                            cn.Close();
                        }

                    }

                }
                catch (Exception ex)
                {
                    cn.Close();
                    //null para mi es error
                    oPaginaCLS = null;
                }


            }


            return oPaginaCLS;
        }

        //public List<TipoUsuarioCLS> filtrarTipoUsuario(TipoUsuarioCLS obj)
        //{
        //    IConfigurationBuilder builder = new ConfigurationBuilder();
        //    builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
        //    var root = builder.Build();
        //    var bd = root.GetConnectionString("cn");
        //    List<TipoUsuarioCLS> lista = null;
        //    using (SqlConnection cn = new SqlConnection(bd))
        //    {
        //        try
        //        {
        //            cn.Open();
        //            using (SqlCommand cmd = new SqlCommand("uspFiltrarTipousuario", cn))
        //            {
        //                //Le indico que es un procedure
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@nombre", obj.nombre == null ? "" : obj.nombre);
        //                cmd.Parameters.AddWithValue("@descripcion", obj.descripcion == null ? "" : obj.descripcion);
        //                SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
        //                if (drd != null)
        //                {
        //                    lista = new List<TipoUsuarioCLS>();
        //                    int posId = drd.GetOrdinal("IIDTIPOUSUARIO");
        //                    int posNombre = drd.GetOrdinal("NOMBRE");
        //                    int posDESCRIPCION = drd.GetOrdinal("DESCRIPCION");
        //                    TipoUsuarioCLS oTipoUsuarioCLS;
        //                    while (drd.Read())
        //                    {
        //                        oTipoUsuarioCLS = new TipoUsuarioCLS();
        //                        oTipoUsuarioCLS.iidtipousuario = drd.IsDBNull(posId) ? 0 : drd.GetInt32(posId);
        //                        oTipoUsuarioCLS.nombre = drd.IsDBNull(posNombre) ? "" : drd.GetString(posNombre);
        //                        oTipoUsuarioCLS.descripcion = drd.IsDBNull(posDESCRIPCION) ? "" : drd.GetString(posDESCRIPCION);
        //                        lista.Add(oTipoUsuarioCLS);
        //                    }
        //                    cn.Close();
        //                }

        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            cn.Close();
        //            //null para mi es error
        //            lista = null;
        //        }

        //    }


        //    return lista;
        //}

        public List<PaginaCLS> listarPagina()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var bd = root.GetConnectionString("cn");

            List<PaginaCLS> lista = null;
            using (SqlConnection cn = new SqlConnection(bd))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspListarPagina", cn))
                    {
                        //Le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        if (drd != null)
                        {
                            lista = new List<PaginaCLS>();
                            int posId = drd.GetOrdinal("IIDPAGINA");
                            int posMensaje = drd.GetOrdinal("MENSAJE");
                            int posControlador = drd.GetOrdinal("CONTRALADOR");
                            int posAccion = drd.GetOrdinal("ACCION");
                            PaginaCLS oPaginaCLS;
                            while (drd.Read())
                            {
                                oPaginaCLS = new PaginaCLS();
                                oPaginaCLS.iidpagina = drd.IsDBNull(posId) ? 0 : drd.GetInt32(posId);
                                oPaginaCLS.mensaje = drd.IsDBNull(posMensaje) ? "" : drd.GetString(posMensaje);
                                oPaginaCLS.controlador = drd.IsDBNull(posControlador) ? "" : drd.GetString(posControlador);
                                oPaginaCLS.accion = drd.IsDBNull(posAccion) ? "" : drd.GetString(posAccion);
                                lista.Add(oPaginaCLS);
                            }
                            cn.Close();
                        }

                    }

                }
                catch (Exception ex)
                {
                    cn.Close();
                    //null para mi es error
                    lista = null;
                }

            }


            return lista;
        }

        public int eliminarPagina(int iidpagina)
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
                    using (SqlCommand cmd = new SqlCommand("uspEliminarPagina", cn))
                    {
                        //Indico que es Procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@iidpagina", iidpagina);
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
    }
}