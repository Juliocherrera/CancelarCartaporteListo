
using System.Text;
using XSystem.Security.Cryptography;

namespace CapaUsuario
{
    public class GenericLH
    {
        public static string cifrarCadena(string cadena)
        {
            //SHA256Managed sha = new SHA256Managed();
            ////Bytes
            //byte[] bytecadena = Encoding.Default.GetBytes(cadena);
            //byte[] bytecifrado = sha.ComputeHash(bytecadena);
            //return BitConverter.ToString(bytecifrado).Replace("-", "");

            //Base64 como en la cartaporte
            byte[] bytecadena = Encoding.UTF8.GetBytes(cadena);
            return Convert.ToBase64String(bytecadena);
        }

    }
}