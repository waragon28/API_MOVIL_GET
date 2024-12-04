
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;
using Sap.Data.Hana;
using System.Configuration;
using System.Data;
using SAP_Core.Utils;
using WebApiNetCore.DAL;
using WebApiNetCore.Utils;
using Sentry;

namespace SAP_Core.DAL
{
    public class BancoDAL : Connection, IDisposable
    {

        public ListaBanco GetBanco(string imei)
        {
            ListaBanco bancos = new ListaBanco();
            List<BancoBO> listBancoBO = new List<BancoBO>();
            BancoBO banco = null;
            string strSQL = string.Format("CALL {0}.APP_BANKS ('{1}')", DataSource.bd(), imei);


            try
            {
                using (HanaConnection connection = GetConnection())
                {
                    connection.Open();
                    using (HanaCommand command = new HanaCommand(strSQL, connection))
                    {
                        using (HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    banco = new BancoBO();
                                    banco.BankId =Other.GetStringValue(reader, "BankID");
                                    banco.BankName = Other.GetStringValue(reader, "Name").ToUpper();
                                    banco.SingleDeposit = Other.GetStringValue(reader, "UniqueDeposit");
                                    banco.Post = Other.GetStringValue(reader, "POS");
                                    listBancoBO.Add(banco);
                                }
                            }
                        }
                    }
                }

                bancos.Banks = listBancoBO;
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetBanco", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetBanco", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }

            return bancos;
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
        ~BancoDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
