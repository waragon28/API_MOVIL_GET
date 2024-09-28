
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
    public class CobranzaDDAL : Connection,IDisposable
    {
        public ListaCobranzaD GetCollections(string imei, string fecha)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListaCobranzaD cobranzasD = new ListaCobranzaD();
            List<CobranzaDBO> listaCobranzaD = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS ('{1}','{2}')", Utils.bd(), imei, fecha);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cobranzaD = new CobranzaDBO();
                        cobranzaD.Code = reader["Code"].ToString();
                        cobranzaD.BankID = reader["BankID"].ToString();
                        cobranzaD.DepositID = reader["Deposit"].ToString();
                        cobranzaD.ItemDetail = reader["ItemDetail"].ToString();
                        cobranzaD.CardCode = reader["CardCode"].ToString();
                        cobranzaD.DocNum = reader["DocNum"].ToString();
                        cobranzaD.DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString());
                        cobranzaD.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                        cobranzaD.AmountCharged = Convert.ToDecimal(reader["AmountCharged"].ToString());
                        cobranzaD.NewBalance = Convert.ToDecimal(reader["NewBalance"].ToString());
                        cobranzaD.IncomeDate = reader["IncomeDate"].ToString();
                        cobranzaD.Receip = Convert.ToInt32(reader["Receip"].ToString());
                        cobranzaD.Status = reader["Status"].ToString();
                        cobranzaD.Commentary = reader["Comments"].ToString().ToUpper();
                        cobranzaD.UserCode = reader["UserID"].ToString();
                        cobranzaD.SlpCode = reader["SlpCode"].ToString();
                        cobranzaD.CardName = reader["CardName"].ToString().ToUpper();
                        cobranzaD.LegalNumber = reader["NumAtCard"].ToString();
                        cobranzaD.Banking = reader["Banking"].ToString();
                        cobranzaD.CancellationReason = reader["CancelReason"].ToString().ToUpper();
                        cobranzaD.QRStatus = reader["QRStatus"].ToString();
                        cobranzaD.DirectDeposit = reader["DirectDeposit"].ToString();
                        cobranzaD.POSPay = reader["POSPay"].ToString();
                        listaCobranzaD.Add(cobranzaD);
                    }
                }
                cobranzasD.CollectionDetail = listaCobranzaD;
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

            return cobranzasD;
        }
        public CobranzaDBO GetCollectionDetail(string imei, string recibo)
        {
            HanaDataReader reader;
            HanaConnection connection=GetConnection();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS_DETAIL ('{1}','{2}')", Utils.bd(), imei, recibo);

            try
            {
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cobranzaD.Code = reader["Code"].ToString();
                        cobranzaD.DocEntry = reader["DocEntry"].ToString();
                        cobranzaD.BankID = reader["BankID"].ToString();
                        cobranzaD.DepositID = reader["Deposit"].ToString();
                        cobranzaD.ItemDetail = reader["ItemDetail"].ToString();
                        cobranzaD.CardCode = reader["CardCode"].ToString();
                        cobranzaD.DocEntryFT = reader["DocEntryFT"].ToString();
                        cobranzaD.DocNum = reader["DocNum"].ToString();
                        cobranzaD.DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString());
                        cobranzaD.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                        cobranzaD.AmountCharged = Convert.ToDecimal(reader["AmountCharged"].ToString());
                        cobranzaD.NewBalance = Convert.ToDecimal(reader["NewBalance"].ToString());
                        cobranzaD.IncomeDate = reader["IncomeDate"].ToString();
                        cobranzaD.Receip = Convert.ToInt32(reader["Receip"].ToString());
                        cobranzaD.Status = reader["Status"].ToString();
                        cobranzaD.Commentary = reader["Comments"].ToString().ToUpper();
                        cobranzaD.UserCode = reader["UserID"].ToString();
                        cobranzaD.SlpCode = reader["SlpCode"].ToString();
                        cobranzaD.CardName = reader["CardName"].ToString().ToUpper();
                        cobranzaD.LegalNumber = reader["NumAtCard"].ToString();
                        cobranzaD.Banking = reader["Banking"].ToString();
                        cobranzaD.CancellationReason = reader["CancelReason"].ToString().ToUpper();
                        cobranzaD.QRStatus = reader["QRStatus"].ToString();
                        cobranzaD.DirectDeposit = reader["DirectDeposit"].ToString();
                        cobranzaD.POSPay = reader["POSPay"].ToString();
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

            return cobranzaD;
        }
        public List<CobranzaDBO> GetCollectionDocument(string imei, string docEntry)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<CobranzaDBO> listCobranzaBO = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS_DOCUMENT ('{1}','{2}')", Utils.bd(), imei, docEntry);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cobranzaD = new CobranzaDBO();
                        cobranzaD.DocEntry = reader["DocEntry"].ToString();
                        cobranzaD.BankID = reader["BankID"].ToString();
                        cobranzaD.DepositID = reader["Deposit"].ToString();
                        cobranzaD.ItemDetail = reader["ItemDetail"].ToString();
                        cobranzaD.CardCode = reader["CardCode"].ToString();
                        cobranzaD.DocEntryFT = reader["DocEntryFT"].ToString();
                        cobranzaD.DocNum = reader["DocNum"].ToString();
                        cobranzaD.DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString());
                        cobranzaD.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                        cobranzaD.AmountCharged = Convert.ToDecimal(reader["AmountCharged"].ToString());
                        cobranzaD.NewBalance = Convert.ToDecimal(reader["NewBalance"].ToString());
                        cobranzaD.IncomeDate = reader["IncomeDate"].ToString();
                        cobranzaD.Receip = Convert.ToInt32(reader["Receip"].ToString());
                        cobranzaD.Status = reader["Status"].ToString();
                        cobranzaD.Commentary = reader["Comments"].ToString().ToUpper();
                        cobranzaD.UserCode = reader["UserID"].ToString();
                        cobranzaD.SlpCode = reader["SlpCode"].ToString();
                        cobranzaD.CardName = reader["CardName"].ToString().ToUpper();
                        cobranzaD.LegalNumber = reader["NumAtCard"].ToString();
                        cobranzaD.Banking = reader["Banking"].ToString();
                        cobranzaD.CancellationReason = reader["CancelReason"].ToString().ToUpper();
                        cobranzaD.QRStatus = reader["QRStatus"].ToString();
                        cobranzaD.DirectDeposit = reader["DirectDeposit"].ToString();
                        cobranzaD.POSPay = reader["POSPay"].ToString();
                        listCobranzaBO.Add(cobranzaD);
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

            return listCobranzaBO;
        }
        public ListaCobranzaD GetCollectionStatus(string imei, string status)
        {
            HanaDataReader reader;
            HanaConnection connection=GetConnection();
            ListaCobranzaD cobranzasD = new ListaCobranzaD();
            List<CobranzaDBO> listCobranzaBO = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL =  string.Format("CALL {0}.APP_COLLECTIONS_STATUS ('{1}','{2}')", Utils.bd(), imei, status);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cobranzaD = new CobranzaDBO();
                        cobranzaD.Code = reader["Code"].ToString();
                        cobranzaD.DocEntry = reader["DocEntry"].ToString();
                        cobranzaD.BankID = reader["BankID"].ToString();
                        cobranzaD.DepositID = reader["Deposit"].ToString();
                        cobranzaD.ItemDetail = reader["ItemDetail"].ToString();
                        cobranzaD.CardCode = reader["CardCode"].ToString();
                        cobranzaD.DocEntryFT = reader["DocEntryFT"].ToString();
                        cobranzaD.DocNum = reader["DocNum"].ToString();
                        cobranzaD.DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString());
                        cobranzaD.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                        cobranzaD.AmountCharged = Convert.ToDecimal(reader["AmountCharged"].ToString());
                        cobranzaD.NewBalance = Convert.ToDecimal(reader["NewBalance"].ToString());
                        cobranzaD.IncomeDate = reader["IncomeDate"].ToString();
                        cobranzaD.Receip = Convert.ToInt32(reader["Receip"].ToString());
                        cobranzaD.Status = reader["Status"].ToString();
                        cobranzaD.Commentary = reader["Comments"].ToString().ToUpper();
                        cobranzaD.UserCode = reader["UserID"].ToString();
                        cobranzaD.SlpCode = reader["SlpCode"].ToString();
                        cobranzaD.CardName = reader["CardName"].ToString().ToUpper();
                        cobranzaD.LegalNumber = reader["NumAtCard"].ToString();
                        cobranzaD.Banking = reader["Banking"].ToString();
                        cobranzaD.CancellationReason = reader["CancelReason"].ToString().ToUpper();
                        cobranzaD.QRStatus = reader["QRStatus"].ToString();
                        cobranzaD.DirectDeposit = reader["DirectDeposit"].ToString();
                        cobranzaD.POSPay = reader["POSPay"].ToString();
                        listCobranzaBO.Add(cobranzaD);
                    }
                }
                cobranzasD.CollectionDetail = listCobranzaBO;
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
            return cobranzasD;
        }
        public ListaCobranzaD GetCollectionStatus2(string imei, string status, string user)
        {
            HanaDataReader reader;
            HanaConnection connection=GetConnection();
            ListaCobranzaD cobranzasD = new ListaCobranzaD();
            List<CobranzaDBO> listCobranzaBO = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS_STATUS2 ('{1}','{2}','{3}')", Utils.bd(), imei, status, user);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    cobranzaD = new CobranzaDBO();
                    cobranzaD.Code = reader["Code"].ToString();
                    cobranzaD.DocEntry = reader["DocEntry"].ToString();
                    cobranzaD.BankID = reader["BankID"].ToString();
                    cobranzaD.DepositID = reader["Deposit"].ToString();
                    cobranzaD.ItemDetail = reader["ItemDetail"].ToString();
                    cobranzaD.CardCode = reader["CardCode"].ToString();
                    cobranzaD.DocEntryFT = reader["DocEntryFT"].ToString();
                    cobranzaD.DocNum = reader["DocNum"].ToString();
                    cobranzaD.DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString());
                    cobranzaD.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                    cobranzaD.AmountCharged = Convert.ToDecimal(reader["AmountCharged"].ToString());
                    cobranzaD.NewBalance = Convert.ToDecimal(reader["NewBalance"].ToString());
                    cobranzaD.IncomeDate = reader["IncomeDate"].ToString();
                    cobranzaD.Receip = Convert.ToInt32(reader["Receip"].ToString());
                    cobranzaD.Status = reader["Status"].ToString();
                    cobranzaD.Commentary = reader["Comments"].ToString().ToUpper();
                    cobranzaD.UserCode = reader["UserID"].ToString();
                    cobranzaD.SlpCode = reader["SlpCode"].ToString();
                    cobranzaD.CardName = reader["CardName"].ToString().ToUpper();
                    cobranzaD.LegalNumber = reader["NumAtCard"].ToString();
                    cobranzaD.Banking = reader["Banking"].ToString();
                    cobranzaD.CancellationReason = reader["CancelReason"].ToString().ToUpper();
                    cobranzaD.QRStatus = reader["QRStatus"].ToString();
                    cobranzaD.DirectDeposit = reader["DirectDeposit"].ToString();
                    cobranzaD.POSPay = reader["POSPay"].ToString();
                    listCobranzaBO.Add(cobranzaD);
                }
            }
            cobranzasD.CollectionDetail = listCobranzaBO;
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

            return cobranzasD;
        }
        public ListaCobranzaD GetCollectionDeposit(string imei, string deposit)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaCobranzaD cobranzasD = new ListaCobranzaD();
            List<CobranzaDBO> listCobranzaBO = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS_DEPOSIT ('{1}','{2}')", Utils.bd(), imei, deposit);

            try { 
            connection.OpenAsync();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    cobranzaD = new CobranzaDBO();
                    cobranzaD.Code = reader["Code"].ToString();
                    cobranzaD.BankID = reader["BankID"].ToString();
                    cobranzaD.DepositID = reader["Deposit"].ToString();
                    cobranzaD.ItemDetail = reader["ItemDetail"].ToString();
                    cobranzaD.CardCode = reader["CardCode"].ToString();
                    cobranzaD.DocNum = reader["DocNum"].ToString();
                    cobranzaD.DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString());
                    cobranzaD.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                    cobranzaD.AmountCharged = Convert.ToDecimal(reader["AmountCharged"].ToString());
                    cobranzaD.NewBalance = Convert.ToDecimal(reader["NewBalance"].ToString());
                    cobranzaD.IncomeDate = reader["IncomeDate"].ToString();
                    cobranzaD.Receip = Convert.ToInt32(reader["Receip"].ToString());
                    cobranzaD.Status = reader["Status"].ToString();
                    cobranzaD.Commentary = reader["Comments"].ToString().ToUpper();
                    cobranzaD.UserCode = reader["UserID"].ToString();
                    cobranzaD.SlpCode = reader["SlpCode"].ToString();
                    cobranzaD.CardName = reader["CardName"].ToString().ToUpper();
                    cobranzaD.LegalNumber = reader["NumAtCard"].ToString();
                    cobranzaD.Banking = reader["Banking"].ToString();
                    cobranzaD.CancellationReason = reader["CancelReason"].ToString().ToUpper();
                    cobranzaD.QRStatus = reader["QRStatus"].ToString();
                    cobranzaD.DirectDeposit = reader["DirectDeposit"].ToString();
                    cobranzaD.POSPay = reader["POSPay"].ToString();
                    listCobranzaBO.Add(cobranzaD);
                }
            }
            cobranzasD.CollectionDetail = listCobranzaBO;
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

            return cobranzasD;
        }
        public ReceipResponse GetValidCollections(string IncomeDate, string CardCode, string DocEntry, string Receip, string SlpCode)
        {
            HanaDataReader reader;
            HanaConnection connection=GetConnection();
            ReceipResponse s = new ReceipResponse();
            string strSQL = string.Format("CALL {0}.APP_VALID_COLLECTION ('{1}','{2}','{3}','{4}',{5})", Utils.bd(), IncomeDate, CardCode, DocEntry, Receip, SlpCode);

            try{
                connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);
            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    s = new ReceipResponse();
                    s.ErrorCode = "0";
                    s.Receip = reader["Receip"].ToString();
                    s.Message = "Cobranza " + reader["ItemDetail"].ToString() + " registrada satisfactoriamente";
                    s.ItemDetail = reader["ItemDetail"].ToString();
                    s.Code = reader["Code"].ToString();
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
            return s;
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
        ~CobranzaDDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
