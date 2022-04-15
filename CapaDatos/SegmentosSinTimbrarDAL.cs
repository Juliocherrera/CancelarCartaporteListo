using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using CapaEntidad;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class SegmentosSinTimbrarDAL
    {
        public List<SegmentosSinTimbrarCLS> listarSegmentos()
        {
            //IConfigurationBuilder builder = new ConfigurationBuilder();
            //builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            //var root = builder.Build();
            string cadena = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //var bd = root.GetConnectionString("cn");
            List<SegmentosSinTimbrarCLS> lista = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspListarSegmentos", cn))
                    {
                        //Le indico que es del itpo procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        //Ejecutamos el procedimiento
                        SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        //Recorremos el arreglo
                        if (drd != null)
                        {
                            lista = new List<SegmentosSinTimbrarCLS>();
                            int possSegmento = drd.GetOrdinal("Segmento");
                            int possFolio = drd.GetOrdinal("Folio");
                            int possCl = drd.GetOrdinal("cliente");
                            int possFc = drd.GetOrdinal("Fechac");
                            int possFt = drd.GetOrdinal("Fechat");

                            //int possFecha = drd.GetOrdinal("Fecha");
                            //int possBillto = drd.GetOrdinal("ord_billto");
                            //int possUuid = drd.GetOrdinal("UUID");
                            //int possZip = drd.GetOrdinal("Pdf_xml_descarga");
                            //int possPdf = drd.GetOrdinal("Pdf_descargafactura");
                            //int possXml = drd.GetOrdinal("xlm_descargafactura");
                            SegmentosSinTimbrarCLS oSegmentosSinTimbrarCLS;
                            while (drd.Read())
                            {
                                oSegmentosSinTimbrarCLS = new SegmentosSinTimbrarCLS();
                                oSegmentosSinTimbrarCLS.segmento = drd.IsDBNull(possSegmento) ? 0 : drd.GetInt32(possSegmento);
                                oSegmentosSinTimbrarCLS.folio = drd.IsDBNull(possFolio) ? "<button type='button' class='btn btn-danger'><i class='far fa-times-circle'></i> | Sin Timbrar</button>" : "<button type='button' class='btn btn-success'> <i class='far fa-check-circle'></i> | " + drd.GetString(possFolio) + "</button>";
                                oSegmentosSinTimbrarCLS.cliente = drd.IsDBNull(possCl) ? "No existe" : drd.GetString(possCl);
                                oSegmentosSinTimbrarCLS.fechac = drd.IsDBNull(possFc) ? null : drd.GetDateTime(possFc);
                                oSegmentosSinTimbrarCLS.fechat = drd.IsDBNull(possFt) ? null : drd.GetDateTime(possFt);
                                
                                //oSegmentosSinTimbrarCLS.segmento = drd.IsDBNull(possSegmento) ? "<button type='button' class='btn btn-danger'>Sin Timbrar</button>" : "<button type='button' class='btn btn-success'>"+ drd.GetString(possSegmento) + "</button>";
                                //oSegmentosSinTimbrarCLS.uuid = drd.IsDBNull(possUuid) ? "No existe" : drd.GetString(possUuid);
                                //oCartaPorteCLS.pdf_xml_descarga = drd.IsDBNull(possZip) ? "No existe" : "<a href=https://canal1.xsa.com.mx:9050" + drd.GetString(possZip) + "><button type='button' class='btn btn-primary'><i class='fas fa-thumbs'></i> ZIP</button></a>";
                                //oCartaPorteCLS.pdf_descargafactura = drd.IsDBNull(possPdf) ? "No existe" : "<a href=" + drd.GetString(possPdf) + "><button type='button' class='btn btn-danger'>PDF</button></a>";
                                //oCartaPorteCLS.xlm_descargafactura = drd.IsDBNull(possXml) ? "No existe" : "<a href=" + drd.GetString(possXml) + "><button type='button' class='btn btn-success'>XML</button></a>";
                                lista.Add(oSegmentosSinTimbrarCLS);

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

        public List<SegmentosSinTimbrarCLS> filtrarCpS(string segmento, string cliente)
        {
            //IConfigurationBuilder builder = new ConfigurationBuilder();
            //builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            //var root = builder.Build();
            //var bd = root.GetConnectionString("cn");
            string cadena = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            List<SegmentosSinTimbrarCLS> lista = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspFiltradoCpS", cn))
                    {
                        //Le indico que es del itpo procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        //Esta linea define un parametro
                        cmd.Parameters.AddWithValue("@segmento", segmento == null ? "" : segmento);
                        cmd.Parameters.AddWithValue("@cliente", cliente == null ? "" : cliente);
                        //Ejecutamos el procedimiento
                        SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        //Recorremos el arreglo
                        if (drd != null)
                        {
                            lista = new List<SegmentosSinTimbrarCLS>();
                            int posSegmento = drd.GetOrdinal("Segmento");
                            int posFolio = drd.GetOrdinal("Folio");
                            int posCl = drd.GetOrdinal("cliente");
                            int posFc = drd.GetOrdinal("Fechac");
                            int posFt = drd.GetOrdinal("Fechat");
                            SegmentosSinTimbrarCLS oSegmentosSinTimbrarCLS;
                            while (drd.Read())
                            {
                                oSegmentosSinTimbrarCLS = new SegmentosSinTimbrarCLS();
                                oSegmentosSinTimbrarCLS.segmento = drd.IsDBNull(posSegmento) ? 0 : drd.GetInt32(posSegmento);
                                oSegmentosSinTimbrarCLS.folio = drd.IsDBNull(posFolio) ? "<button type='button' class='btn btn-danger'><i class='far fa-times-circle'></i> | Sin Timbrar</button>" : "<button type='button' class='btn btn-success'> <i class='far fa-check-circle'></i> | " + drd.GetString(posFolio) + "</button>";
                                oSegmentosSinTimbrarCLS.cliente = drd.IsDBNull(posCl) ? "No existe" : drd.GetString(posCl);
                                oSegmentosSinTimbrarCLS.fechac = drd.IsDBNull(posFc) ? null : drd.GetDateTime(posFc);
                                oSegmentosSinTimbrarCLS.fechat = drd.IsDBNull(posFt) ? null : drd.GetDateTime(posFt);

                                //oSegmentosSinTimbrarCLS.segmento = drd.IsDBNull(possSegmento) ? "<button type='button' class='btn btn-danger'>Sin Timbrar</button>" : "<button type='button' class='btn btn-success'>"+ drd.GetString(possSegmento) + "</button>";
                                //oSegmentosSinTimbrarCLS.uuid = drd.IsDBNull(possUuid) ? "No existe" : drd.GetString(possUuid);
                                //oCartaPorteCLS.pdf_xml_descarga = drd.IsDBNull(possZip) ? "No existe" : "<a href=https://canal1.xsa.com.mx:9050" + drd.GetString(possZip) + "><button type='button' class='btn btn-primary'><i class='fas fa-thumbs'></i> ZIP</button></a>";
                                //oCartaPorteCLS.pdf_descargafactura = drd.IsDBNull(possPdf) ? "No existe" : "<a href=" + drd.GetString(possPdf) + "><button type='button' class='btn btn-danger'>PDF</button></a>";
                                //oCartaPorteCLS.xlm_descargafactura = drd.IsDBNull(possXml) ? "No existe" : "<a href=" + drd.GetString(possXml) + "><button type='button' class='btn btn-success'>XML</button></a>";
                                lista.Add(oSegmentosSinTimbrarCLS);

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