using SalesForce.BO;
using Sap.Data.Hana;
using SAP_Core.DAL;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiNetCore.BO;

namespace WebApiNetCore.DAL
{
    public class SolicitudDevolDAL : Connection, IDisposable
    {
        private bool disposedValue;

        public async Task<ResponseData> SolicitudDevolucion(SoliDevBO soliDevBO)
        {
            ResponseData rs = new ResponseData();
            try
            {
                HanaDataReader reader;
                HanaConnection connection = GetConnection();
                string strSQL = string.Format("CALL {0}. ('{1}')", DataSource.bd(), soliDevBO.DocEntryFact);
             //   SoliDevBO ObjSoliDevBO = new SoliDevBO();
                SolicitudDevolucion ObjSolicitudDevolucion = new SolicitudDevolucion();

                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ObjSolicitudDevolucion = new SolicitudDevolucion();
                        ObjSolicitudDevolucion.DocType = "dDocument_Items";
                        ObjSolicitudDevolucion.DocDate = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.DocDueDate = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.CardCode = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.NumAtCard = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.DocCurrency = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.Comments = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.PaymentGroupCode = Convert.ToInt32(reader["DocDate"].ToString());
                        ObjSolicitudDevolucion.SalesPersonCode = Convert.ToInt32( reader["DocDate"].ToString());
                        ObjSolicitudDevolucion.DocumentsOwner = Convert.ToInt32( reader["DocDate"].ToString());
                        ObjSolicitudDevolucion.ContactPersonCode = Convert.ToInt32(reader["DocDate"].ToString());
                        ObjSolicitudDevolucion.TaxDate = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.DocObjectCode = "oReturnRequest";
                        ObjSolicitudDevolucion.ShipToCode = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.PayToCode = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDMT = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDTD = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDSD = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDCD = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_STATUS = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DETPAGADO = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_AUTODET = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_NGUIA = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDFN = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDFC = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDVN = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDVC = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDTS = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_CONSIGNADOR = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_GRFT = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_TIPO_TRANSF = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_CONMON = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_ANTPEN = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DOCAPR = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_OCTRA = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_TIPEXP = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_PNACT = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_PZCreated = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_INC = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_CON_STOK = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_PDTREV = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_PDTCRE = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIT_VENMOS = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIST_PROMADIC = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIST_SUCUSU = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIST_APSOLV = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIST_DIS = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_TVENTA = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_SalesOrderID = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_CommentApproval = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_OVCommentary = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_EVCommentary = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_INCommentary = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_CompleteOV = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_OVRejected = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_ApprovedBy = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_CONSOL = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_FCONSOL = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_HCONSOL = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_CORRDES = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_FCDES = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_OCUR = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_AYUDANTE = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_ESTDES = reader["DocDate"].ToString();



                        ObjSolicitudDevolucion.U_VIST_SUCUSU = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIST_APSOLV = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIST_DIS = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_SYP_TVENTA = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_SalesOrderID = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_CommentApproval = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_OVCommentary = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_EVCommentary = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_INCommentary = reader["DocDate"].ToString();


                    }
                }
            


            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return rs;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~SolicitudDevolDAL()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }
}
