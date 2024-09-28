
using Sap.Data.Hana;
using SAP_Core.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.DAL
{
    public class KardexDAL : Connection, IDisposable
    {

        public ListaKardex getKardex(string CardCode)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaKardex kardex = new ListaKardex();
            List<KardexBO> listaKardex = new List<KardexBO>();
            KardexBO kardx = new KardexBO();
            string strSQL = string.Format("CALL {0}.P_VIS_CRE_KARDEX_CLIENTE ('{1}')", Utils.bd(), CardCode);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        kardx = new KardexBO();
                        kardx.CardCode = reader["CardCode"].ToString();
                        kardx.DocCur = reader["DocCur"].ToString();
                        kardx.NumAtCard = reader["U_SYP_MDSD"].ToString()+"-"+reader["U_SYP_MDCD"].ToString();
                        kardx.TaxDate = reader["TaxDate"].ToString();
                        kardx.DocDueDate = reader["DocDueDate"].ToString();
                        kardx.DocTotal = Convert.ToDecimal(reader["DocTotal"]);
                        kardx.DocEntry = reader["DocEntry"].ToString();
                        kardx.Balance = Convert.ToDecimal(reader["SALDO"]);
                        kardx.CardName = reader["CardName"].ToString();
                        kardx.LicTradNum = reader["LicTradNum"].ToString();
                        kardx.Mobile = reader["Phone1"]==null?"": reader["Phone1"].ToString();
                        kardx.SlpCode = reader["U_VIS_SlpCode"].ToString();
                        kardx.Street = reader["Street"].ToString();
                        kardx.PymntGroup = reader["PymntGroup"].ToString();
                        kardx.Block = reader["U_SYP_DEPA"].ToString();
                        kardx.County = reader["U_SYP_PROV"].ToString();
                        kardx.City = reader["U_SYP_DIST"].ToString();
                        kardx.AmountCharged = Convert.ToDecimal(reader["Importe Cobrado"]);
                        kardx.IncomeDate = reader["FECHA DE PAGO"].ToString();
                        kardx.OperationNumber = reader["NRO. OPERA"].ToString();
                        kardx.DocNum = reader["DocNum"].ToString();
                        kardx.JrnlMemo = reader["JrnlMemo"].ToString();
                        kardx.Comments = reader["Comments"].ToString();
                        kardx.Bank = reader["Banco"].ToString();
                        kardx.SalesInvoice = reader["VendedorFactura"].ToString();
                        kardx.CollectorInvoice = reader["CobradorFactura"].ToString();

                        listaKardex.Add(kardx);
                    }
                }
                kardex.Kardex = listaKardex;
                connection.Close();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (connection.State.Equals("Open"))
                {
                    connection.Close();
                }
            }
            return kardex;

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
        ~KardexDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
