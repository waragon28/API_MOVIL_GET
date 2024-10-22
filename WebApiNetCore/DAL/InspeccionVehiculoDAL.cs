using GoogleApi.Entities.Maps.DistanceMatrix.Response;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Kiota.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesForce.BO;
using SalesForce.Util;
using Sap.Data.Hana;
using SAP_Core.DAL;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;
using WebApiNetCore.BO;
using static WebApiNetCore.Utils.Other;
using Method = WebApiNetCore.Utils.Other.Method;

namespace WebApiNetCore.DAL
{
    public class InspeccionVehiculoDAL : Connection, IDisposable
    {
        private ServiceLayer serviceLayer;
        private bool disposedValue;

        public InspeccionVehiculoDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }


        public async Task<ResponseData> InicioRutaSMS(int DocNum)
        {
            ResponseData rs = new ResponseData();
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            string fechaActual = DateTime.Now.ToString("yyyyMMdd");
            string strSQL = string.Format("CALL {0}.APP_NRO_CLIENTE ('{1}','{2}')", DataSource.bd(), DocNum, fechaActual);

            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                // La URL de tu API
                string url = "http://192.168.254.20:88/vs1.0/sms";

                // El objeto que necesitas serializar a JSON
              
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string Mensaje = string.Format("VISTONY : Hola {0} " + ".." +
                                    "Nos complace informarle que su pedido {1} se encuentra en ruta hacia su destino. " + ".." +
                                    "Queremos garantizar que su compra llegue a usted en perfectas condiciones y en el tiempo estipulado. ",
                                                reader["CardName"].ToString()
                                    , reader["U_NumAtCard"].ToString()); // NUMERO DE FACTURA F00X-XXXXXXXX
                        var cabeceraMensaje = cabecera_mensaje(Mensaje, "51" +"986686967"/*reader["Phone1"].ToString()*/);

                        // Serializa el objeto a JSON
                        string jsonData = JsonConvert.SerializeObject(cabeceraMensaje);

                        // Llama al método que envía la solicitud POST
                        var result = await PostRequestAsync(url, jsonData);

                        string time = DateTime.Now.ToString("hhmm");
                        StartRoute objStartRoute = startRoute("Y", Convert.ToInt32(time));
                        string jsonData2 = JsonConvert.SerializeObject(objStartRoute);
                        ResponseData response = await serviceLayer.Request("/b1s/v1/VIS_DIS_ODRT", Method.POST, jsonData2);
                    }
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "InicioRutaSMS - " + ex.Message + " - ");
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
            return rs;
        }
        public StartRoute startRoute(string Value, int Hora)
        {
            StartRoute startRoute = new StartRoute();
            startRoute.U_SendSMS = Value;
            startRoute.U_StartTime = Hora;
            return startRoute;
        }
        private static readonly HttpClient client = new HttpClient();
        public Cabecera_Mensaje cabecera_mensaje(string Mensaje, string Numero)
        {
            List<Cabecera_Mensaje> ListCabecera_Mensaje = new List<Cabecera_Mensaje>();
            Cabecera_Mensaje objCabecera_Mensaje = new Cabecera_Mensaje();
            objCabecera_Mensaje.Data = DetalleMensaje(Mensaje, Numero);
            return objCabecera_Mensaje;
        }
        public List<Data> DetalleMensaje(string Mensaje, string Numero)
        {
            List<Data> ListData = new List<Data>();
            Data objData = new Data();
            objData.Mensaje = Mensaje;
            objData.NumeroTelf = Numero;
            ListData.Add(objData);
            return ListData;
        }


        // Método para enviar la solicitud POST
        public static async Task<HttpResponseMessage> PostRequestAsync(string url, string jsonData)
        {
            // Crea el contenido de la solicitud como JSON
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Enviar la solicitud POST
            HttpResponseMessage response = await client.PostAsync(url, content);

            return response;
        }

        public async Task<ResponseData> insert(string jsonPayload)
        {
            ResponseData response = await serviceLayer.Request("/b1s/v1/VIS_DIS_CHLI", Method.POST, jsonPayload);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                dynamic responseBody = await response.Data.Content.ReadAsStringAsync();
                // Parsear el JSON
                JObject obj = JObject.Parse(responseBody);

                // Leer el valor de DocEntry
                int DocNum = (int)obj["DocNum"];

                // responseBody = JsonConvert.DeserializeObject(responseBody.ToString());  /* await new Task(async () => await*/ SolicitudDevolucion(deliveryNote, returnReasonText, sl.token)/*)*/;

                // Lanza InicioRutaSMS de manera asíncrona sin bloquear
                Task.Run(() => InicioRutaSMS(DocNum));
                response.Data ="Se creo la Inspeccion de Vehculo Nº "+DocNum;
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {

                ResponseError error = null;

                if (response.Data != null)
                {
                    var responseBody = await response.Data.Content.ReadAsStringAsync();
                    error = System.Text.Json.JsonSerializer.Deserialize<ResponseError>(responseBody);
                }

                response.StatusCode = HttpStatusCode.FailedDependency;
                response.Data = error;
            }

            return response;
        }

        public async Task<ResponseData> inspeccionVehiculo (string isnspeccionVehiculo)
        {
            ResponseData rs = new ResponseData();
            try
            {
                InspeccionVehiculo ObjInspeccionVehiculo = new InspeccionVehiculo();

                // Parsear el JSON
                JObject obj = JObject.Parse(isnspeccionVehiculo);

                ObjInspeccionVehiculo.U_fecha_inspeccion = obj["Inspeccion"]["fecha_inspeccion"].ToString();
                ObjInspeccionVehiculo.U_hora_inspeccion = obj["Inspeccion"]["hora_inspeccion"].ToString();
                ObjInspeccionVehiculo.U_conductor = obj["Inspeccion"]["conductor"].ToString();
                ObjInspeccionVehiculo.U_telefono = obj["Inspeccion"]["telefono"].ToString();
                ObjInspeccionVehiculo.U_placa = obj["Inspeccion"]["placa"].ToString();
                ObjInspeccionVehiculo.U_marca = obj["Inspeccion"]["marca"].ToString();
                ObjInspeccionVehiculo.U_modelo = obj["Inspeccion"]["modelo"].ToString();
                ObjInspeccionVehiculo.U_licencia = obj["Inspeccion"]["licencia"].ToString();
                ObjInspeccionVehiculo.U_tipo_vehiculo = obj["Inspeccion"]["tipo_vehiculo"].ToString();
                ObjInspeccionVehiculo.U_kilometraje_inicial =Convert.ToDouble(obj["Inspeccion"]["kilometraje_inicial"].ToString());
                ObjInspeccionVehiculo.U_fecha_emision_tarjeta_propiedad = obj["Inspeccion"]["fecha_emision_tarjeta_propiedad"].ToString();
                ObjInspeccionVehiculo.U_tarjeta_circulacion = obj["Inspeccion"]["tarjeta_circulacion"].ToString();
                ObjInspeccionVehiculo.U_emision_soat = obj["Inspeccion"]["emision_soat"].ToString();
                ObjInspeccionVehiculo.U_fecha_emision_revision_tecnica = obj["Inspeccion"]["fecha_emision_revision_tecnica"].ToString();
                ObjInspeccionVehiculo.U_observaciones = obj["Inspeccion"]["observaciones"].ToString();
                ObjInspeccionVehiculo.U_puntos_mejora = obj["Inspeccion"]["puntos_mejora"].ToString();
                ObjInspeccionVehiculo.VIS_DIS_HLI1Collection = Detalle(isnspeccionVehiculo);

                var jsonData = JsonConvert.SerializeObject(ObjInspeccionVehiculo);

                rs = insert(jsonData.ToString()).GetAwaiter().GetResult();
                string xd = string.Empty;

            }
            catch (Exception ex)
            {
                rs.Data = ex.Message.ToString();
            }
            finally
            {

            }
            return rs;
        }
       
        public List<VISDISHLI1Collection> Detalle(string isnspeccionVehiculo)
        {
            VISDISHLI1Collection ObjVISDISHLI1Collection = new VISDISHLI1Collection();
            List<VISDISHLI1Collection> LsVISDISHLI1Collection = new List<VISDISHLI1Collection>();

            // Parsear el JSON
            JObject obj = JObject.Parse(isnspeccionVehiculo);

            foreach (var detalle in obj["DetalleInspeccion"])
            {
                ObjVISDISHLI1Collection = new VISDISHLI1Collection();

                ObjVISDISHLI1Collection.U_seccion = detalle["seccion"].ToString();
                ObjVISDISHLI1Collection.U_pregunta = detalle["pregunta"].ToString();
                ObjVISDISHLI1Collection.U_respuesta = detalle["respuesta"].ToString();
                LsVISDISHLI1Collection.Add(ObjVISDISHLI1Collection);
            }
            return LsVISDISHLI1Collection;
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
        // ~InspeccionVehiculoDAL()
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
