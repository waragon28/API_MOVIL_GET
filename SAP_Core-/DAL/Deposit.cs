
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sap.Data.Hana;
using SAP_Core.BO;
using System.Configuration;

namespace SAP_Core.DAL
{
    public class DepositDAL : Connection, IDisposable
    {
        public ListDeposit GetCobranzaC (string imei, string fecIni, string fecFin)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListDeposit depositos = new ListDeposit();
            List<DepositBO2> listaDepositos = new List<DepositBO2>();
            DepositBO2 deposito = new DepositBO2();
            string strSQL = string.Format("CALL {0}.APP_DEPOSITS ('{1}','{2}','{3}')", Utils.bd(), imei, fecIni, fecFin);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);
            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    deposito = new DepositBO2();
                    deposito.Code = reader["Code"].ToString();
                    deposito.U_VIS_BankID = reader["U_VIS_BankID"].ToString();
                    deposito.BankName = reader["BankName"].ToString();
                    deposito.U_VIS_IncomeType = reader["U_VIS_IncomeType"].ToString();
                    deposito.U_VIS_Deposit = reader["U_VIS_Deposit"].ToString();
                    deposito.U_VIS_Date = reader["U_VIS_Date"].ToString();
                    deposito.U_VIS_DeferredDate = reader["U_VIS_DeferredDate"].ToString();
                    deposito.U_VIS_Banking = reader["U_VIS_Banking"].ToString();
                    deposito.U_VIS_UserID = reader["U_VIS_UserID"].ToString();
                    deposito.U_VIS_SlpCode = reader["U_VIS_SlpCode"].ToString();
                    deposito.U_VIS_AmountDeposit = Convert.ToDecimal(reader["U_VIS_AmountDeposit"].ToString());
                    deposito.U_VIS_Status = reader["U_VIS_Status"].ToString();
                    deposito.U_VIS_Comments = reader["U_VIS_Comments"].ToString();
                    deposito.U_VIS_CancelReason = reader["U_VIS_CancelReason"].ToString();
                    deposito.U_VIS_DirectDeposit = reader["U_VIS_DirectDeposit"].ToString();
                    deposito.U_VIS_POSPay = reader["U_VIS_POSPay"].ToString();
                    listaDepositos.Add(deposito);
                }
            }
            depositos.Deposits = listaDepositos;
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

            return depositos;
        }
        public DepositBO2 GetCobranza(string imei, string banco, string deposito)
        {
            HanaDataReader reader;
            HanaConnection connection=GetConnection();
            DepositBO2 deposit = new DepositBO2();
            string strSQL  = string.Format("CALL {0}.APP_DEPOSITS_BANK ('{1}','{2}','{3}')", Utils.bd(), imei, banco, deposito);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    deposit.Code = reader["Code"].ToString();
                    deposit.U_VIS_BankID = reader["U_VIS_BankID"].ToString();
                    deposit.BankName = reader["BankName"].ToString();
                    deposit.U_VIS_IncomeType = reader["U_VIS_IncomeType"].ToString();
                    deposit.U_VIS_Deposit = reader["U_VIS_Deposit"].ToString();
                    deposit.U_VIS_Date = reader["U_VIS_Date"].ToString();
                    deposit.U_VIS_DeferredDate = reader["U_VIS_DeferredDate"].ToString();
                    deposit.U_VIS_Banking = reader["U_VIS_Banking"].ToString();
                    deposit.U_VIS_UserID = reader["U_VIS_UserID"].ToString();
                    deposit.U_VIS_SlpCode = reader["U_VIS_SlpCode"].ToString();
                    deposit.U_VIS_AmountDeposit = Convert.ToDecimal(reader["U_VIS_AmountDeposit"].ToString());
                    deposit.U_VIS_Status = reader["U_VIS_Status"].ToString();
                    deposit.U_VIS_Comments = reader["U_VIS_Comments"].ToString();
                    deposit.U_VIS_CancelReason = reader["U_VIS_CancelReason"].ToString();
                    deposit.U_VIS_DirectDeposit = reader["U_VIS_DirectDeposit"].ToString();
                    deposit.U_VIS_POSPay = reader["U_VIS_POSPay"].ToString();
                }
            }

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

            return deposit;
        }
        public DepositResponse GetValidDeposit(string bank, string deposit, decimal ammount, string date, string slpCode)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            DepositResponse de = new DepositResponse();
            string strSQL = string.Format("CALL {0}.APP_VALID_DEPOSIT ('{1}','{2}',{3},'{4}','{5}')", Utils.bd(), bank, deposit, ammount, date, slpCode);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);
            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    de = new DepositResponse();
                    de.ErrorCode = "0";
                    de.Message = "Depósito " + reader["Deposit"].ToString() + " agregado satisfactoriamente";
                    de.Deposit = reader["Deposit"].ToString();
                    de.Code = reader["Code"].ToString();
                }
            }
            //cobranzasD.CollectionDetail = listaCobranzaD;
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

            return de;
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
        ~DepositDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
