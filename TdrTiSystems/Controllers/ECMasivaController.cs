using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using TdrTiSystems.Filter;
using System.Net.Http.Headers;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using Microsoft.AspNetCore.Http;
using TdrTiSystems.Models;
using Grpc.Core;
using System.Xml;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.Extensions.Hosting.Internal;

namespace TdrTiSystems.Controllers
{
    public class ECMasivaController : Controller
    {
        [ServiceFilter(typeof(Seguridad))]
        
        public IActionResult Index()
        {
            

            return View();
        }
        //public IActionResult MyIndex()
        //{
        //    var demoJs = new DemoJs();
        //    demoJs.JsFunction = "$( window ).load(function() { DisparaAlert();}); ";
        //    return View(demoJs);
        //}
        public List<ECMasivaCLS> listarMasiva()
        {
            ECMasivaBL obj = new ECMasivaBL();
            return obj.listarMasiva();
        }

        public async Task<IActionResult> RepeDatos(IFormFile excelCargar)
        {
            byte[] buffer = null;
            string nombreFoto = "";
            using (MemoryStream ms = new MemoryStream())
            {
                excelCargar.CopyTo(ms);
                nombreFoto = excelCargar.FileName;
                buffer = ms.ToArray();

            }
            var filename = excelCargar.FileName.Trim();

            //get path
            var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            if (!Directory.Exists(MainPath))
            {
                Directory.CreateDirectory(MainPath);
            }
            var filePath = Path.Combine(MainPath, filename);
            using (System.IO.Stream stream = new FileStream(filePath, FileMode.Create))
            {
                await excelCargar.CopyToAsync(stream);
            }

            //get extension
            string extension = Path.GetExtension(filename);
            string conString = string.Empty;
            switch (extension)
            {
                case ".xls": //Excel 97-03.
                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                case ".xlsx": //Excel 07 and above.
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }
            DataTable dt = new DataTable();
            //dt.Columns.Add("Id", typeof(int));
            conString = string.Format(conString, filePath);
            using (OleDbConnection connExcel = new OleDbConnection(conString))
            {
                using (OleDbCommand cmdExcel = new OleDbCommand())
                {
                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                    {
                        cmdExcel.Connection = connExcel;

                        //Get the name of First Sheet.
                        connExcel.Open();
                        DataTable dtExcelSchema;
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        //Read Data from First Sheet.
                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        odaExcel.SelectCommand = cmdExcel;
                        odaExcel.Fill(dt);
                        
                        connExcel.Close();
                        
                    }
                }
            }
            string cadena = @"Data source=DESKTOP-CV57FOU\SQLEXPRESS; Initial Catalog=BDFarmacia; User ID=jdev; Password=tdr123;Trusted_Connection=false;MultipleActiveResultSets=true";


            using (SqlConnection con = new SqlConnection(cadena))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name.
                    sqlBulkCopy.DestinationTableName = "TCME";

                    // Map the Excel columns with that of the database table, this is optional but good if you do
                    // 
                    sqlBulkCopy.ColumnMappings.Add("Ai_orden", "Ai_orden");
                    sqlBulkCopy.ColumnMappings.Add("Av_cmd_code", "Av_cmd_code");
                    sqlBulkCopy.ColumnMappings.Add("Av_cmd_description", "Av_cmd_description");
                    sqlBulkCopy.ColumnMappings.Add("Af_count", "Af_count");
                    sqlBulkCopy.ColumnMappings.Add("Av_countunit", "Av_countunit");
                    sqlBulkCopy.ColumnMappings.Add("Av_description_parts", "Av_description_parts");
                    sqlBulkCopy.ColumnMappings.Add("Af_weight", "Af_weight");
                    sqlBulkCopy.ColumnMappings.Add("Av_weightunit", "Av_weightunit");
                    sqlBulkCopy.ColumnMappings.Add("Av_description_units", "Av_description_units");
                    con.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    con.Close();
                }
            }
            //int resultado = cargarEnSQL2(dt);
            return View("Index");
            
           
                
            
        }

        public int limpiarHistorial()
        {


            int resultado = 1;
            //NOS CONECTAMOS CON LA BASE DE DATOS
            string cadena = @"Data source=DESKTOP-CV57FOU\SQLEXPRESS; Initial Catalog=BDFarmacia; User ID=jdev; Password=tdr123;Trusted_Connection=false;MultipleActiveResultSets=true";
            using (SqlConnection cn = new SqlConnection(cadena))
            {

                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_limpiar", cn);
                    //cmd.Parameters.AddWithValue("@nombre", nombre);
                    //cmd.Parameters.Add("EsX", SqlDbType.Structured).Value = tabla;
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;


                    resultado = cmd.ExecuteNonQuery();
                    resultado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception)
                {

                    cn.Close();
                    resultado = 1;
                }
            }

            return resultado;

        }
        public int cargarEnSQL()
        {

            
            int resultado = 1;
                //NOS CONECTAMOS CON LA BASE DE DATOS
                string cadena = @"Data source=DESKTOP-CV57FOU\SQLEXPRESS; Initial Catalog=BDFarmacia; User ID=jdev; Password=tdr123;Trusted_Connection=false;MultipleActiveResultSets=true";
                using (SqlConnection cn = new SqlConnection(cadena))
                {

                    try
                    {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand("usp_cargarXz", cn);
                        //cmd.Parameters.AddWithValue("@nombre", nombre);
                        //cmd.Parameters.Add("EsX", SqlDbType.Structured).Value = tabla;
                        cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;

                        cmd.CommandType = CommandType.StoredProcedure;


                        resultado = cmd.ExecuteNonQuery();
                        resultado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

                    }
                    catch (Exception)
                    {

                        cn.Close();
                        resultado = 1;
                    }
                }
           
            return resultado;

        }
        public int cargarEnSQL2()
        {

            int resultado = 1;
            try
            {
                //NOS CONECTAMOS CON LA BASE DE DATOS
                string cadena = @"Data source=DESKTOP-CV57FOU\SQLEXPRESS; Initial Catalog=BDFarmacia; User ID=jdev; Password=tdr123;Trusted_Connection=false;MultipleActiveResultSets=true";
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("usp_cargarX", cn);
                    //cmd.Parameters.AddWithValue("@nombre", nombre);
                    //cmd.Parameters.Add("EsX", SqlDbType.Structured).Value = tabla;
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);



                }
            }
            catch (Exception ex)
            {
                resultado = 1;


            }

            return resultado;
        }
        public async Task<IActionResult> GuardarDatos(IFormFile excelCargar)
        {
            byte[] buffer = null;
            string nombreFoto = "";
            using (MemoryStream ms = new MemoryStream())
            {
                excelCargar.CopyTo(ms);
                nombreFoto = excelCargar.FileName;
                buffer = ms.ToArray();

            }
            var filename = excelCargar.FileName.Trim();

            //get path
            var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            if (!Directory.Exists(MainPath))
            {
                Directory.CreateDirectory(MainPath);
            }
            var filePath = Path.Combine(MainPath, filename);
            using (System.IO.Stream stream = new FileStream(filePath, FileMode.Create))
            {
                await excelCargar.CopyToAsync(stream);
            }

            //get extension
            string extension = Path.GetExtension(filename);
            string conString = string.Empty;
            switch (extension)
            {
                case ".xls": //Excel 97-03.
                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                case ".xlsx": //Excel 07 and above.
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }
            DataTable dt = new DataTable();
            conString = string.Format(conString, filePath);
            using (OleDbConnection connExcel = new OleDbConnection(conString))
            {
                using (OleDbCommand cmdExcel = new OleDbCommand())
                {
                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                    {
                        cmdExcel.Connection = connExcel;

                        //Get the name of First Sheet.
                        connExcel.Open();
                        DataTable dtExcelSchema;
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        //Read Data from First Sheet.
                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        odaExcel.SelectCommand = cmdExcel;
                        odaExcel.Fill(dt);
                        connExcel.Close();
                    }
                }
            }



            //string cadena = @"Data source=DESKTOP-CV57FOU\SQLEXPRESS; Initial Catalog=BDFarmacia; User ID=jdev; Password=tdr123;Trusted_Connection=false;MultipleActiveResultSets=true";


            //using (SqlConnection con = new SqlConnection(cadena))
            //{
            //    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
            //    {
            //        //Set the database table name.
            //        sqlBulkCopy.DestinationTableName = "students";

            //        // Map the Excel columns with that of the database table, this is optional but good if you do
            //        // 
            //        sqlBulkCopy.ColumnMappings.Add("Id", "Id");
            //        sqlBulkCopy.ColumnMappings.Add("Name", "Name");
            //        sqlBulkCopy.ColumnMappings.Add("Email", "Email");
            //        sqlBulkCopy.ColumnMappings.Add("Class", "Class");
            //        sqlBulkCopy.ColumnMappings.Add("Pesos", "Pesos");
            //        con.Open();
            //        sqlBulkCopy.WriteToServer(dt);
            //        con.Close();
            //    }
            //}
            //int resultado = cargarEnSQL2(dt);

            return View("Index");

        }

        //private IHostingEnvironment Environment;

        //public ECMasivaController(IHostingEnvironment _environment)
        //{
        //    this.Environment = _environment;
        //}
        //public string ReadXMLFile()
        //{
        //    string xml = "";
        //    int resultado = 1;
        //    try
        //    {
        //        string filename = "";
        //        string cadena = @"Data source=DESKTOP-CV57FOU\SQLEXPRESS; Initial Catalog=DBCARGA; User ID=jdev; Password=tdr123;Trusted_Connection=false;MultipleActiveResultSets=true";
        //        using (SqlConnection cn = new SqlConnection(cadena))
        //        {
        //            SqlCommand cmd = new SqlCommand("usp_sayerxml", cn);
        //            cmd.Parameters.Add("Filename", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cn.Open();
        //            cmd.ExecuteNonQuery();
        //            filename = Convert.ToString(cmd.Parameters["Filename"].Value);
        //            DataSet doc = new DataSet();
        //            var path = Path.Combine(Environment.WebRootPath, "Xml", "" + filename + "");
                    
        //            DataSet objds = new DataSet();
        //            objds.ReadXml(path);
        //            //DataTable res = objds;

        //            for (int i = 0; i < objds.Tables.Count; i++)
        //            {
        //                DataTable respu = objds.Tables[i];
        //                SqlCommand cmd2 = new SqlCommand("uspsxml"+i, cn);
        //                //cmd2.Parameters.AddWithValue("@res", res);
        //                cmd2.Parameters.Add("EstructuraCargaXmls_" + i, SqlDbType.Structured).Value = respu;
        //                cmd2.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
        //                cmd2.Parameters.Add("Xml", SqlDbType.Xml).Direction = ParameterDirection.Output;
        //                cmd2.CommandType = CommandType.StoredProcedure;
        //                cmd2.ExecuteNonQuery();
        //                resultado = Convert.ToInt32(cmd2.Parameters["Resultado"].Value);
        //                xml = Convert.IsDBNull(xml) ? (string)cmd2.Parameters["Xml"].Value : (string)cmd2.Parameters["Xml"].Value;
        //            }

        //            //SqlCommand cmd2 = new SqlCommand("uspsxml", cn);
        //            ////cmd2.Parameters.AddWithValue("@res", res);
        //            //cmd2.Parameters.Add("EstructuraCargaXmls", SqlDbType.Structured).Value = res;
        //            //cmd2.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            //cmd2.CommandType = CommandType.StoredProcedure;
        //            //cmd2.ExecuteNonQuery();
        //            //resultado = Convert.ToInt32(cmd2.Parameters["Resultado"].Value);

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        resultado = 1;
        //    }
        //    return xml;
           
        // }

        //public string ReadXMLFileT()
        //{
        //    int resultado = 0;
        //    string s = "";
        //    try
        //    {
        //        //string filename = "";
        //        string cadena = @"Data source=DESKTOP-CV57FOU\SQLEXPRESS; Initial Catalog=DBCARGA; User ID=jdev; Password=tdr123;Trusted_Connection=false;MultipleActiveResultSets=true";
        //        using (SqlConnection cn = new SqlConnection(cadena))
        //        {
        //            //SqlCommand cmd = new SqlCommand("usp_sayerxml", cn);
        //            //cmd.Parameters.Add("Filename", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
        //            //cmd.CommandType = CommandType.StoredProcedure;
        //            //cn.Open();
        //            //cmd.ExecuteNonQuery();
        //            //filename = Convert.ToString(cmd.Parameters["Filename"].Value);
        //            //DataSet doc = new DataSet();
        //            //var path = Path.Combine(Environment.WebRootPath, "Xml", "" + filename + "");

        //            //DataSet objds = new DataSet();
        //            //objds.ReadXml(path);
        //            ////DataTable res = objds;

        //            //for (int i = 0; i < objds.Tables.Count; i++)
        //            //{
        //                //DataTable respu = objds.Tables[i];
        //                SqlCommand cmd2 = new SqlCommand("usptractores", cn);
        //                //cmd2.Parameters.AddWithValue("@res", res);
        //                //cmd2.Parameters.Add("EstructuraCargaXmls_" + i, SqlDbType.Structured).Value = respu;
        //                //cmd2.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
        //                //cmd2.Parameters.Add("Xml", SqlDbType.Xml).Direction = ParameterDirection.Output;
        //                cmd2.CommandType = CommandType.StoredProcedure;
        //            cn.Open();
        //            using (var xmlReader = cmd2.ExecuteXmlReader())
        //            {
        //                while (xmlReader.Read())
        //                {
        //                    s = xmlReader.ReadOuterXml();
        //                }
        //            }
                    



        //            //SqlCommand cmd2 = new SqlCommand("uspsxml", cn);
        //            ////cmd2.Parameters.AddWithValue("@res", res);
        //            //cmd2.Parameters.Add("EstructuraCargaXmls", SqlDbType.Structured).Value = res;
        //            //cmd2.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            //cmd2.CommandType = CommandType.StoredProcedure;
        //            //cmd2.ExecuteNonQuery();
        //            //resultado = Convert.ToInt32(cmd2.Parameters["Resultado"].Value);

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        resultado = 1;
        //    }
        //    return s;

        //}

        //public string XmlC(DataTable tabla)
        //{
        //    string cadena = @"Data source=DESKTOP-CV57FOU\SQLEXPRESS; Initial Catalog=DBCARGA; User ID=jdev; Password=tdr123;Trusted_Connection=false;MultipleActiveResultSets=true";
        //    using (SqlConnection cn = new SqlConnection(cadena))
        //    {


        //    }
        //        return "Prueba";
        //}

    }
}
