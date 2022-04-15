using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using CapaEntidad;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class ECMasivaDAL
    {
        //public List<ECMasivaCLS> listarMasiva()
        //{
        //    IConfigurationBuilder builder = new ConfigurationBuilder();
        //    builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
        //    var root = builder.Build();
        //    var bd = root.GetConnectionString("cn");
        //    List<ECMasivaCLS> lista = null;
        //    using (SqlConnection cn = new SqlConnection(bd))
        //    {
        //        try
        //        {
        //            cn.Open();
        //            using (SqlCommand cmd = new SqlCommand("uspListarMasiva", cn))
        //            {
        //                //Le indico que es del itpo procedure
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                //Ejecutamos el procedimiento
        //                SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
        //                //Recorremos el arreglo
        //                if (drd != null)
        //                {
        //                    lista = new List<ECMasivaCLS>();
        //                    int posId = drd.GetOrdinal("Ai_orden");
        //                    int posCode = drd.GetOrdinal("Av_cmd_code");
        //                    int posdesc = drd.GetOrdinal("Av_cmd_description");
        //                    int posCount = drd.GetOrdinal("Af_count");
        //                    int posCunit = drd.GetOrdinal("Av_countunit");
        //                    int posWeig = drd.GetOrdinal("Af_weight");
        //                    int posWunit = drd.GetOrdinal("Av_weightunit");
        //                    ECMasivaCLS oECMasivaCLS;
        //                    while (drd.Read())
        //                    {
        //                        oECMasivaCLS = new ECMasivaCLS();
        //                        oECMasivaCLS.Ai_orden = drd.IsDBNull(posId) ? 0 : drd.GetInt32(0);
        //                        oECMasivaCLS.Av_cmd_code = drd.IsDBNull(posCode) ? "" : drd.GetString(posCode);
        //                        oECMasivaCLS.Av_cmd_description = drd.IsDBNull(posdesc) ? "" : drd.GetString(posdesc);
        //                        oECMasivaCLS.Af_count = drd.IsDBNull(posCount) ? 0 : drd.GetDecimal(posCount);
        //                        oECMasivaCLS.Av_countunit = drd.IsDBNull(posCunit) ? "" : drd.GetString(posCunit);
        //                        oECMasivaCLS.Af_weight = drd.IsDBNull(posWeig) ? 0 : drd.GetDecimal(posWeig);
        //                        oECMasivaCLS.Av_weightunit = drd.IsDBNull(posWunit) ? "" : drd.GetString(posWunit);
        //                        lista.Add(oECMasivaCLS);

        //                    }
        //                    cn.Close();

        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            cn.Close();
        //            lista = null;
        //        }
        //    }

        //    return lista;

        //}
       

        public List<ECMasivaCLS> listarMasiva()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var bd = root.GetConnectionString("cn");
            List<ECMasivaCLS> lista = null;
            using (SqlConnection cn = new SqlConnection(bd))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspListarMasiva2", cn))
                    {
                        //Le indico que es del itpo procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        //Ejecutamos el procedimiento
                        SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        //Recorremos el arreglo
                        if (drd != null)
                        {
                            lista = new List<ECMasivaCLS>();
                            int posId = drd.GetOrdinal("Ai_orden");
                            int posCode = drd.GetOrdinal("Av_cmd_code");
                            int posdesc = drd.GetOrdinal("Av_cmd_description");
                            int posCount = drd.GetOrdinal("Af_count");
                            int posCunit = drd.GetOrdinal("Av_countunit");
                            int posWeig = drd.GetOrdinal("Af_weight");
                            int posWunit = drd.GetOrdinal("Av_weightunit");
                            ECMasivaCLS oECMasivaCLS;
                            while (drd.Read())
                            {
                                oECMasivaCLS = new ECMasivaCLS();
                                oECMasivaCLS.Ai_orden = drd.IsDBNull(posId) ? 0 : drd.GetInt32(0);
                                oECMasivaCLS.Av_cmd_code = drd.IsDBNull(posCode) ? "" : drd.GetString(posCode);
                                oECMasivaCLS.Av_cmd_description = drd.IsDBNull(posdesc) ? "" : drd.GetString(posdesc);
                                oECMasivaCLS.Af_count = drd.IsDBNull(posCount) ? 0 : drd.GetDecimal(posCount);
                                oECMasivaCLS.Av_countunit = drd.IsDBNull(posCunit) ? "" : drd.GetString(posCunit);
                                oECMasivaCLS.Af_weight = drd.IsDBNull(posWeig) ? 0 : drd.GetDecimal(posWeig);
                                oECMasivaCLS.Av_weightunit = drd.IsDBNull(posWunit) ? "" : drd.GetString(posWunit);
                                lista.Add(oECMasivaCLS);

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