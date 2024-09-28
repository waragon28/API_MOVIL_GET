
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;
using Sap.Data.Hana;
using System.Data;
using SAP_Core.Utils;

namespace SAP_Core.DAL
{
    public class ComisionesDAL : Connection,IDisposable
    {
        public ListaComisiones GetComision(string imei, string anio, string mes)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaComisiones comisiones = new ListaComisiones();
            List<ComisionesBO> listComisiones = new List<ComisionesBO>();
            ComisionesBO comision = null;
            string strSQL = string.Format("CALL {0}.APP_COMMISSIONS ('{1}','{2}','{3}')", DataSource.bd(), imei, anio, mes);
            
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comision = new ComisionesBO();
                        comision.Variable = reader["Variable"].ToString();
                        comision.Uom = reader["UDM"].ToString();
                        comision.Advance = Convert.ToDecimal(reader["Avance"].ToString());
                        comision.Quota = Convert.ToDecimal(reader["Cuota"].ToString());
                        comision.Percentage = Convert.ToDecimal(reader["Porcentaje"].ToString());
                        comision.CodeColor = reader["CodeColor"].ToString();
                        comision.HideData = reader["HideData"].ToString();
                        comision.Peso =Convert.ToInt32(reader["Peso"].ToString());
                        comision.Combinada = Convert.ToDecimal(reader["Combinada"].ToString());
                        comision.Escala = Convert.ToDecimal(reader["Escala"].ToString());
                        comision.Comision = Convert.ToDecimal(reader["Comision"].ToString());
                        comision.Premio = Convert.ToDecimal(reader["Premio"].ToString());
                        comision.Total = Convert.ToDecimal(reader["Total"].ToString());
                        comision.Premio_Pregunta = reader["Premio?"].ToString();
                        comision.VariableComisional_Pregunta = reader["VariableComisional?"].ToString();
                        comision.Detalle = reader["Detalle?"].ToString();

                        listComisiones.Add(comision);
                    }
                }
                comisiones.Commissions = listComisiones;
                connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetComision - " + ex.Message +" "+
                    imei+" "+ anio+" "+mes);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return comisiones;
        }
        public CommissionDetals GetDetalleComision(string imei, string Variable)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            CommissionDetals commissionDetals = new CommissionDetals();

            List<ListComisionDetalle> listComisiones = new List<ListComisionDetalle>();
            ListComisionDetalle comision = null;
            string strSQL = string.Format("CALL {0}.APP_COMMISSIONS_DETAIL ('{1}','{2}')", DataSource.bd(), imei,Variable);

            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comision = new ListComisionDetalle();
                        comision.CardCode = reader["CardCode"].ToString();
                        comision.CardName = reader["CardName"].ToString();
                        comision.Category = reader["Category"].ToString();
                        comision.NextVisitDate = reader["NextVisitDate"].ToString();
                        comision.LastPurchaseDate = reader["LastPurchaseDate"].ToString();
                        comision.Covered = reader["Covered?"].ToString();

                        listComisiones.Add(comision);
                    }
                    commissionDetals.listComisionDetalles = listComisiones;
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "GetDetalleComision - " + ex.Message + " " +
                    imei + " " + Variable );
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return commissionDetals;
        }


        #region Disposable




        private bool disposing = false;
        /// <summary>
        /// Método de IDisposable para desechar la clase.
        /// </summary>
        public void Dispose()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        /// <summary>
        /// Método sobrecargado de Dispose que será el que
        /// libera los recursos, controla que solo se ejecute
        /// dicha lógica una vez y evita que el GC tenga que
        /// llamar al destructor de clase.
        /// </summary>
        /// <param name=”b”></param>
        protected virtual void Dispose(bool b)
        {
            // Si no se esta destruyendo ya…
            {
                if (!disposing)

                    // La marco como desechada ó desechandose,
                    // de forma que no se puede ejecutar este código
                    // dos veces.
                    disposing = true;
                // Indico al GC que no llame al destructor
                // de esta clase al recolectarla.
                GC.SuppressFinalize(this);
                // … libero los recursos… 
            }
        }




        /// <summary>
        /// Destructor de clase.
        /// En caso de que se nos olvide “desechar” la clase,
        /// el GC llamará al destructor, que tambén ejecuta la lógica
        /// anterior para liberar los recursos.
        /// </summary>
        ~ComisionesDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
