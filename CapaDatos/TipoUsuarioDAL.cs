﻿using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using CapaEntidad;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class TipoUsuarioDAL
    {
        public int guardarDatos(TipoUsuarioCLS oTipoUsuarioCLS)
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
                    using (SqlTransaction transaction = cn.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand("uspGuardarTipousuario", cn, transaction))
                        {
                            //Indico que es Procedure
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@iidtipousuario", oTipoUsuarioCLS.iidtipousuario);
                            cmd.Parameters.AddWithValue("@nombre", oTipoUsuarioCLS.nombre);
                            cmd.Parameters.AddWithValue("@descripcion", oTipoUsuarioCLS.descripcion);
                            SqlParameter parametro = null;
                            if (oTipoUsuarioCLS.iidtipousuario == 0)
                            {
                                parametro = cmd.Parameters.Add("@@identity", SqlDbType.Int);
                                parametro.Direction = ParameterDirection.ReturnValue;
                            }
                            //Insert , Update y Delete (El numero de filas afectadas)
                            rpta = cmd.ExecuteNonQuery();

                            if (rpta == 1 && oTipoUsuarioCLS.iidtipousuario == 0)
                            {
                                //Obtiene el Id que se genera al insertar (>1)
                                oTipoUsuarioCLS.iidtipousuario = (int)parametro.Value;
                            }

                            //Si es 1 es que es Ok , si es 0 es que no se realizo
                        }
                        using (SqlCommand cmd = new SqlCommand("uspDeshabilitarTipoUsuarioPagina", cn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@iidtipousuario", oTipoUsuarioCLS.iidtipousuario);
                            rpta = cmd.ExecuteNonQuery();
                        }
                        //Bucle
                        for (int i = 0; i < oTipoUsuarioCLS.iidpaginas.Count; i++)
                        {
                            using (SqlCommand cmd = new SqlCommand("uspGuardarPaginaTipoUsuario", cn, transaction))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@iidtipousuario", oTipoUsuarioCLS.iidtipousuario);
                                cmd.Parameters.AddWithValue("@iidpagina", oTipoUsuarioCLS.iidpaginas[i]);
                                rpta = cmd.ExecuteNonQuery();
                            }
                        }
                        //Ejecuta
                        transaction.Commit();
                        cn.Close();
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

        public TipoUsuarioCLS recuperarTipoUsuario(int iidtipousuario)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var bd = root.GetConnectionString("cn");
            TipoUsuarioCLS oTipoUsuarioCLS = null;
            using (SqlConnection cn = new SqlConnection(bd))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspRecuperarTipousuario", cn))
                    {
                        //Le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@iidtipousuario", iidtipousuario);

                        SqlDataReader drd = cmd.ExecuteReader();
                        if (drd != null)
                        {

                            int posId = drd.GetOrdinal("IIDTIPOUSUARIO");
                            int posNombre = drd.GetOrdinal("NOMBRE");
                            int posDESCRIPCION = drd.GetOrdinal("DESCRIPCION");

                            while (drd.Read())
                            {
                                oTipoUsuarioCLS = new TipoUsuarioCLS();
                                oTipoUsuarioCLS.iidtipousuario = drd.IsDBNull(posId) ? 0 : drd.GetInt32(posId);
                                oTipoUsuarioCLS.nombre = drd.IsDBNull(posNombre) ? "" : drd.GetString(posNombre);
                                oTipoUsuarioCLS.descripcion = drd.IsDBNull(posDESCRIPCION) ? "" : drd.GetString(posDESCRIPCION);

                            }

                        }
                        if (drd.NextResult())
                        {
                            int posIdPagina = drd.GetOrdinal("IIDPAGINA");
                            oTipoUsuarioCLS.iidpaginas = new List<int>();
                            while (drd.Read())
                            {
                                int idpagina = drd.IsDBNull(posIdPagina) ? 0 : drd.GetInt32(posIdPagina);
                                oTipoUsuarioCLS.iidpaginas.Add(idpagina);

                            }
                        }
                        cn.Close();
                    }

                }
                catch (Exception ex)
                {
                    cn.Close();
                    //null para mi es error
                    oTipoUsuarioCLS = null;
                }


            }


            return oTipoUsuarioCLS;
        }

        public List<TipoUsuarioCLS> filtrarTipoUsuario(TipoUsuarioCLS obj)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var bd = root.GetConnectionString("cn");
            List<TipoUsuarioCLS> lista = null;
            using (SqlConnection cn = new SqlConnection(bd))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspFiltrarTipousuario", cn))
                    {
                        //Le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@nombre", obj.nombre == null ? "" : obj.nombre);
                        cmd.Parameters.AddWithValue("@descripcion", obj.descripcion == null ? "" : obj.descripcion);
                        SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        if (drd != null)
                        {
                            lista = new List<TipoUsuarioCLS>();
                            int posId = drd.GetOrdinal("IIDTIPOUSUARIO");
                            int posNombre = drd.GetOrdinal("NOMBRE");
                            int posDESCRIPCION = drd.GetOrdinal("DESCRIPCION");
                            TipoUsuarioCLS oTipoUsuarioCLS;
                            while (drd.Read())
                            {
                                oTipoUsuarioCLS = new TipoUsuarioCLS();
                                oTipoUsuarioCLS.iidtipousuario = drd.IsDBNull(posId) ? 0 : drd.GetInt32(posId);
                                oTipoUsuarioCLS.nombre = drd.IsDBNull(posNombre) ? "" : drd.GetString(posNombre);
                                oTipoUsuarioCLS.descripcion = drd.IsDBNull(posDESCRIPCION) ? "" : drd.GetString(posDESCRIPCION);
                                lista.Add(oTipoUsuarioCLS);
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

        public List<TipoUsuarioCLS> listarTipoUsuario()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var bd = root.GetConnectionString("cn");

            List<TipoUsuarioCLS> lista = null;
            using (SqlConnection cn = new SqlConnection(bd))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspListarTipoUsuario", cn))
                    {
                        //Le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        if (drd != null)
                        {
                            lista = new List<TipoUsuarioCLS>();
                            int posId = drd.GetOrdinal("IIDTIPOUSUARIO");
                            int posNombre = drd.GetOrdinal("NOMBRE");
                            int posDESCRIPCION = drd.GetOrdinal("DESCRIPCION");
                            TipoUsuarioCLS oTipoUsuarioCLS;
                            while (drd.Read())
                            {
                                oTipoUsuarioCLS = new TipoUsuarioCLS();
                                oTipoUsuarioCLS.iidtipousuario = drd.IsDBNull(posId) ? 0 : drd.GetInt32(posId);
                                oTipoUsuarioCLS.nombre = drd.IsDBNull(posNombre) ? "" : drd.GetString(posNombre);
                                oTipoUsuarioCLS.descripcion = drd.IsDBNull(posDESCRIPCION) ? "" : drd.GetString(posDESCRIPCION);
                                lista.Add(oTipoUsuarioCLS);
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

    }
}