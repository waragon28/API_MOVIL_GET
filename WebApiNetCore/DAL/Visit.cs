
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;
using Sap.Data.Hana;
using SAP_Core.Utils;
using System.Configuration;
using System.Data;
using SalesForce.Util;
using Microsoft.Extensions.Caching.Memory;
using SalesForce.BO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net;
using WebApiNetCore.BO;

namespace SAP_Core.DAL
{
    public class VisitDAL : Connection,IDisposable
    {
        private ServiceLayer serviceLayer;
        public VisitDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }
        public List<VisitBOs> GetVisit(string imei, string cardcode)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<VisitBOs> listVisit = new List<VisitBOs>();
            VisitBOs visit;
            string strSQL= string.Format("CALL {0}.APP_VISITS ('{1}','{2}')", DataSource.bd(), imei, cardcode);

            
            try
            {
                connection.OpenAsync();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        visit = new VisitBOs();
                        visit.Code = reader["Code"].ToString();
                        visit.Date = reader["Date"].ToString();
                        visit.Type = reader["Type"].ToString();
                        visit.Observation = reader["Observation"].ToString();
                        visit.Latitude = reader["U_VIS_Latitude"].ToString();
                        visit.Longitude = reader["U_VIS_Longitude"].ToString();
                        listVisit.Add(visit);
                    }
                }
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
            return listVisit;
        }
        public async Task<VisitResponse> GetValidateVisit(string slpCode, string fecha, string visitID, string cardCode)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            VisitResponse s = new VisitResponse();
            string strSQL = string.Format("CALL {0}.APP_VALID_VISIT ('{1}','{2}','{3}','{4}')", DataSource.bd(), slpCode, fecha, visitID, cardCode);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        s = new VisitResponse();
                        s.ErrorCode = "0";
                        s.Message = "Cobranza " + reader["U_VIS_CardCode"].ToString() + " registrada satisfactoriamente";
                        s.IdVisit = reader["Code"].ToString();
                    }
                }
                //cobranzasD.CollectionDetail = listaCobranzaD;
                connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return s;
        }

        public async Task<ResponseData> insert_test(dynamic jsonPayload)
        {
            MultipartContent mpContent = new("mixed", "batch_" + Guid.NewGuid().ToString());

            int i = 0;
            foreach (dynamic visit in jsonPayload)
            {

                var sc = new StringContent("POST /b1s/v1/VIST_VISITAS");
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/http");
                sc.Headers.Add("Content-Transfer-Encoding", "binary");
                sc.Headers.Add("Content-ID", "" + (i + 1));

                dynamic jsonData = generateJsonVisit(visit);

                mpContent.Add(sc);
                mpContent.Add(new StringContent(jsonData.ToString(), Encoding.UTF8, "application/json"));
                i++;
            }

            ResponseData response = await serviceLayer.Batch(mpContent);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                List<VisitResponse> responseList = new();

                i = 0;
                foreach (HttpResponseMessage part in response.Data)
                {
                    VisitResponse vr = new();
                    if (part.StatusCode == HttpStatusCode.Created)
                    {
                        string responseJson = await part.Content.ReadAsStringAsync();
                        VisitBO json = JsonSerializer.Deserialize<VisitBO>(responseJson);

                        vr.IdVisit = json.U_VIS_VisitID;
                        vr.ErrorCode = "0";
                        vr.Message = "B:Visita registrada satisfactoriamente";
                    }
                    else
                    {
                        var responseBody = await part.Content.ReadAsStringAsync();
                        ResponseError responseError = System.Text.Json.JsonSerializer.Deserialize<ResponseError>(responseBody);

                        vr.IdVisit = jsonPayload[i]["IdVisit"].ToString();
                        vr.ErrorCode = "1";
                        vr.Message = "B:Visita No se pudo Registrar " + responseError.error.message;

                    }

                    responseList.Add(vr);
                    i++;
                }

                response.Data = new ListVisit() { Visits = responseList };
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                var responseBody = await response.Data.Content.ReadAsStringAsync();
                ResponseError error = System.Text.Json.JsonSerializer.Deserialize<ResponseError>(responseBody);

                response.StatusCode = HttpStatusCode.FailedDependency;
                response.Data = error;
            }

            return response;
        }
        private dynamic generateJsonVisit(dynamic visit)
        {
            VisitBO visita = new();
            Guid code = Guid.NewGuid();
            visita.Code = code.ToString();

            visita.U_VIS_CardCode = visit["CardCode"].ToString();
            visita.U_VIS_Address = visit["Address"] == null ? "" : visit["Address"].ToString();

            visita.U_VIS_SlpCode = (visit["SlpCode"] == null || visit["SlpCode"] == "") ? "0" : visit["SlpCode"].ToString();
            visita.U_VIS_UserID = (visit["UserId"] == null || visit["UserId"] == "") ? "0" : visit["UserId"].ToString();

            visita.U_VIS_Date = visit["Date"].ToString();
            visita.U_VIS_Hour = visit["Hour"].ToString();
            visita.U_VIS_Type = visit["Type"].ToString();
            visita.U_VIS_Observation = visit["Observation"].ToString();
            visita.U_VIS_Longitude = visit["Longitude"].ToString();
            visita.U_VIS_Latitude = visit["Latitude"].ToString();
            visita.U_VIS_AppVersion = visit["AppVersion"].ToString();
            visita.U_VIS_Intent = visit["Intent"].ToString();
            visita.U_VIS_Brand = visit["Brand"].ToString();
            visita.U_VIS_Model = visit["Model"].ToString();
            visita.U_VIS_Version = visit["OSVersion"].ToString();
            visita.U_VIS_MobileID = visit["MobileID"].ToString();
            visita.U_VIS_StatusRoute = visit["StatusRoute"].ToString();
            visita.U_VIS_VisitID = visit["IdVisit"].ToString();
            visita.U_VIS_Amount = Convert.ToDecimal(visit["Amount"].ToString());
            visita.U_VIS_HoraText = visit["Hour"].ToString();
            visita.U_VIS_HourBefore = visit["Hour_Before"] == null ? "" : visit["Hour_Before"].ToString();

            dynamic jsonData = JsonSerializer.Serialize(visita);

            return jsonData;
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
        ~VisitDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
