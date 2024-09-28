﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;
using Sap.Data.Hana;
using System.Configuration;

namespace SAP_Core.DAL
{
    public class QuotationDAL : Connection, IDisposable
    {

        public ListAgencia GetCotizaciones(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<AgenciaBO> listAgencias = new List<AgenciaBO>();
            ListAgencia agencias = new ListAgencia();
            AgenciaBO agencia = new AgenciaBO();
            string strSQL = string.Format("CALL {0}.APP_AGENCIES ('{1}')", Utils.bd(),imei);

            try
            {

                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        agencia = new AgenciaBO();
                        agencia.AgencyID = reader["Agencia_ID"].ToString().ToUpper();
                        agencia.Agency = reader["Agencia"].ToString().ToUpper();
                        agencia.ZipCode = reader["Ubigeo_ID"].ToString();
                        agencia.LicTradNum = reader["LicTradNum"].ToString();
                        agencia.Street = reader["Street"].ToString().ToUpper();
                        listAgencias.Add(agencia);
                    }
                }
                agencias.Agencies = listAgencias;
                connection.Close();
            }
            catch (Exception)
            {

            }
            finally
            {
                if (connection.State.Equals("Open"))
                {
                    connection.Close();
                }
            }
            return agencias;
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
        ~QuotationDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    
    }
}