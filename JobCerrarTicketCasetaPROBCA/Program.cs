using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCerrarTicketCasetaPROBCA
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionControlAcceso"].ToString());
                DateTime Fecha = DateTime.Now;
                string query = " SELECT	" +
                                    " P.RegistroPersonasID, P.RegistroAccesoID, P.Nombre, P.Gafete, P.FechaEntrada, P.FechaSalida " +
                                " FROM	" +
                                  " RegistroAccesoPersonas P " +
                                    " INNER JOIN RegistroAccesoSteelgo S ON P.RegistroAccesoID = S.RegistroAccesoID	" +
                                " WHERE	" +
                                    " P.EsEmpleado = 1 AND P.Salida = 0 AND P.Chofer = 0 AND P.Placa IS NULL AND P.FechaSalida IS NULL	";
                SqlCommand cmd = new SqlCommand(query, conexion);
                if (conexion.State == ConnectionState.Closed)
                    conexion.Open();

                DataTable tabla = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(query, conexion);
                da.Fill(tabla);
                conexion.Close();//CIERRO PRIMERA CONEXION
                                 /*CREO STORED PROCEDURE PARA DESACTIVAR LOS TICKETS*/
                if (tabla.Rows.Count > 0)
                {
                    string Stored = "dbo.DesactivarTickets";
                    SqlCommand cmd2 = new SqlCommand(Stored, conexion);
                    if (conexion.State == ConnectionState.Closed)
                        conexion.Open();
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Add(new SqlParameter("@Tabla", tabla));
                    cmd2.Parameters.Add("@RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd2.ExecuteNonQuery();
                    //int result = Convert.ToInt32(cmd2.Parameters["@RESULTADO"].Value);
                    //if (result == 1)
                    //{
                    //    Console.WriteLine("Correcto");
                    //}else
                    //{
                    //    Console.WriteLine("ERROR");
                    //}                    
                    conexion.Close();
                }                
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
                //Console.ReadLine();
            }
            catch (Exception EX)
            {
                Console.WriteLine("Error DESACTIVAR TICKET PROBCA: " + EX.Message);
            }            
        }      
    }
}
