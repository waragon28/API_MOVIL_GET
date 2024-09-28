﻿using Sap.Data.Hana;
using SAP_Core.BO;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.DAL
{
    public class OcurrenciasDAL : Connection, IDisposable
    {
        public ListOcurrencies GetOcurrenciasDispatch(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<OcurrencyBO> listOcurrencias = new List<OcurrencyBO>();
            ListOcurrencies ocurrencias = new ListOcurrencies();
            OcurrencyBO ocurrencia = new OcurrencyBO();
            string strSQL = string.Format("CALL {0}.APP_OCURRENCY ('{1}')", DataSource.bd(), imei);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ocurrencia = new OcurrencyBO();
                    ocurrencia.Value = reader["Value"].ToString().ToUpper();
                    ocurrencia.Dscription = reader["Dscription"].ToString().ToUpper();
                    listOcurrencias.Add(ocurrencia);
                }
            }
            ocurrencias.Ocurrencies = listOcurrencias;
            connection.Close();

            }
            catch (Exception)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return ocurrencias;

        }
        
        public ListOcurrencies GetOcurrenciasFreeTransfer(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<OcurrencyBO> listOcurrencias = new List<OcurrencyBO>();
            ListOcurrencies ocurrencias = new ListOcurrencies();
            OcurrencyBO ocurrencia = new OcurrencyBO();
            string strSQL = string.Format("CALL {0}.APP_TYPE_TRANSGRAT('{1}')", DataSource.bd(), imei);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ocurrencia = new OcurrencyBO();
                    ocurrencia.Value = reader["Value"].ToString().ToUpper();
                    ocurrencia.Dscription = reader["Dscription"].ToString().ToUpper();
                    listOcurrencias.Add(ocurrencia);
                }
            }
            ocurrencias.Ocurrencies = listOcurrencias;
            connection.Close();

            }
            catch (Exception e)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return ocurrencias;

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
        ~OcurrenciasDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
