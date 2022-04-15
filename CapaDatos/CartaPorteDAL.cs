using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using CapaEntidad;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CartaPorteDAL
    {
        public List<CartaPorteCLS> listarCartaPorte()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var bd = root.GetConnectionString("cn");
            string cadena = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            List<CartaPorteCLS> lista = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspListarCartaPorte2", cn))
                    {
                        //Le indico que es del itpo procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        //Ejecutamos el procedimiento
                        SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        //Recorremos el arreglo
                        if (drd != null)
                        {
                            lista = new List<CartaPorteCLS>();
                            int possFolio = drd.GetOrdinal("Folio");
                            int possFecha = drd.GetOrdinal("Fecha");
                            int possBillto = drd.GetOrdinal("ord_billto");
                            int possSerie = drd.GetOrdinal("Serie");
                            int possUuid = drd.GetOrdinal("UUID");
                            int possZip = drd.GetOrdinal("Pdf_xml_descarga");
                            int possPdf = drd.GetOrdinal("Pdf_descargafactura");
                            int possXml = drd.GetOrdinal("xlm_descargafactura");
                            CartaPorteCLS oCartaPorteCLS;
                            while (drd.Read())
                            {
                                oCartaPorteCLS = new CartaPorteCLS();
                                oCartaPorteCLS.folio = drd.IsDBNull(possFolio) ? "No existe" : drd.GetString(possFolio);
                                oCartaPorteCLS.fecha = drd.IsDBNull(possFecha) ? null : drd.GetDateTime(possFecha);
                                oCartaPorteCLS.ord_billto = drd.IsDBNull(possBillto) ? "No existe" : drd.GetString(possBillto);
                                oCartaPorteCLS.serie = drd.IsDBNull(possSerie) ? "No existe" : drd.GetString(possSerie);
                                oCartaPorteCLS.uuid = drd.IsDBNull(possUuid) ? "No existe" : drd.GetString(possUuid);
                                oCartaPorteCLS.pdf_xml_descarga = drd.IsDBNull(possZip) ? "No existe" : "<a href=https://canal1.xsa.com.mx:9050" + drd.GetString(possZip) + "><button type='button' class='btn btn-primary'><i class='fas fa-file-archive'></i></button></a>";
                                oCartaPorteCLS.pdf_descargafactura = drd.IsDBNull(possPdf) ? "No existe" : "<a href=" + drd.GetString(possPdf) + "><button type='button' class='btn btn-danger'><i class='fas fa-file-pdf'></i></button></a>";
                                oCartaPorteCLS.xlm_descargafactura = drd.IsDBNull(possXml) ? "No existe" : "<a href=" + drd.GetString(possXml) + "><button type='button' class='btn btn-success'><i class='far fa-file-code'></i></button></a>";
                                lista.Add(oCartaPorteCLS);

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

        public List<CartaPorteCLS> filtrarCartaPorte(string segmento)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var bd = root.GetConnectionString("cn");
            string cadena = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            List<CartaPorteCLS> lista = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspFiltradoCartaPorte2", cn))
                    {
                        //Le indico que es del itpo procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        //Esta linea define un parametro
                        cmd.Parameters.AddWithValue("@segmento", segmento == null ? "" : segmento);
                        //Ejecutamos el procedimiento
                        SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        //Recorremos el arreglo
                        if (drd != null)
                        {
                            lista = new List<CartaPorteCLS>();
                            int posFolio = drd.GetOrdinal("Folio");
                            int posFecha = drd.GetOrdinal("Fecha");
                            int posBillto = drd.GetOrdinal("ord_billto");
                            int posSerie = drd.GetOrdinal("Serie");
                            int posUuid = drd.GetOrdinal("UUID");
                            int posZip = drd.GetOrdinal("Pdf_xml_descarga");
                            int posPdf = drd.GetOrdinal("Pdf_descargafactura");
                            int posXml = drd.GetOrdinal("xlm_descargafactura");
                            CartaPorteCLS oCartaPorteCLS;
                            while (drd.Read())
                            {
                                oCartaPorteCLS = new CartaPorteCLS();
                                oCartaPorteCLS.folio = drd.IsDBNull(posFolio) ? "No existe" : drd.GetString(posFolio);
                                oCartaPorteCLS.fecha = drd.IsDBNull(posFecha) ? null : drd.GetDateTime(posFecha);
                                oCartaPorteCLS.ord_billto = drd.IsDBNull(posBillto) ? "No existe" : drd.GetString(posBillto);
                                oCartaPorteCLS.serie = drd.IsDBNull(posSerie) ? "No existe" : drd.GetString(posSerie);
                                oCartaPorteCLS.uuid = drd.IsDBNull(posUuid) ? "No existe" : drd.GetString(posUuid);
                                oCartaPorteCLS.pdf_xml_descarga = drd.IsDBNull(posZip) ? "No existe" : "<a href=https://canal1.xsa.com.mx:9050" + drd.GetString(posZip) + "><button type='button' class='btn btn-primary'><i class='fas fa-file-archive'></i></button></a>";
                                oCartaPorteCLS.pdf_descargafactura = drd.IsDBNull(posPdf) ? "No existe" : "<a href=" + drd.GetString(posPdf) + "><button type='button' class='btn btn-danger'><i class='fas fa-file-pdf'></i></button></a>";
                                oCartaPorteCLS.xlm_descargafactura = drd.IsDBNull(posXml) ? "No existe" : "<a href=" + drd.GetString(posXml) + "><button type='button' class='btn btn-success'><i class='far fa-file-code'></i></button></a>";
                                lista.Add(oCartaPorteCLS);

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