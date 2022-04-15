using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaNegocio;
using TdrTiSystems.Filter;
using System.Net.Http.Headers;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace TdrTiSystems.Controllers
{
    
    public class PMasivaController : Controller
    {
        [ServiceFilter(typeof(Seguridad))]
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ImportExcelFile(IFormFile FormFile)
        {
            var filename = ContentDispositionHeaderValue.Parse(FormFile.ContentDisposition).FileName.Trim('"');

            //get path
            var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            if (!Directory.Exists(MainPath))
            {
                Directory.CreateDirectory(MainPath);
            }
            var filePath = Path.Combine(MainPath, FormFile.FileName);
            using (System.IO.Stream stream = new FileStream(filePath, FileMode.Create))
            {
                await FormFile.CopyToAsync(stream);
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
            
            string cadena = @"Data source=DESKTOP-CV57FOU\SQLEXPRESS; Initial Catalog=BDFarmacia; User ID=jdev; Password=tdr123;Trusted_Connection=false;MultipleActiveResultSets=true";
            

            using (SqlConnection con = new SqlConnection(cadena))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name.
                    sqlBulkCopy.DestinationTableName = "students";

                    // Map the Excel columns with that of the database table, this is optional but good if you do
                    // 
                    sqlBulkCopy.ColumnMappings.Add("Id", "Id");
                    sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                    sqlBulkCopy.ColumnMappings.Add("Email", "Email");
                    sqlBulkCopy.ColumnMappings.Add("Class", "Class");

                    con.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    con.Close();
                }
            }
            //int resultado = cargarEnSQL(dt);

            
            ViewBag.Message = "File Imported and excel data saved into database";




            return View("Index");
        }
        //public int cargarEnSQL(DataTable tabla)
        //{

        //    int resultado = 0;
        //    try
        //    {
        //        //NOS CONECTAMOS CON LA BASE DE DATOS
        //        string cadena = @"Data source=DESKTOP-CV57FOU\SQLEXPRESS; Initial Catalog=DBCARGA; User ID=jdev; Password=tdr123;Trusted_Connection=false;MultipleActiveResultSets=true";
        //        using (SqlConnection cn = new SqlConnection(cadena))
        //        {
        //            SqlCommand cmd = new SqlCommand("usp_cargarxorden", cn);
        //            //cmd.Parameters.AddWithValue("@nombre", nombre);
        //            cmd.Parameters.Add("EstructuraCargaxOrden", SqlDbType.Structured).Value = tabla;
        //            cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;

        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cn.Open();
        //            cmd.ExecuteNonQuery();
        //            resultado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        string mensaje = ex.Message.ToString();
        //        resultado = 0;
        //    }

        //    return resultado;
        //}

    }
}
