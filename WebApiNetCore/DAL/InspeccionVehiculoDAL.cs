using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesForce.BO;
using SalesForce.Util;
using SAP_Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApiNetCore.BO;
using static WebApiNetCore.Utils.Other;

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

               // responseBody = JsonConvert.DeserializeObject(responseBody.ToString());

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
