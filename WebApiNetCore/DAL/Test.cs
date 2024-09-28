using GoogleApi.Entities.Maps.DistanceMatrix.Request;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SalesForce.BO;
using SalesForce.Util;
using SAP_Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiNetCore.BO;

namespace WebApiNetCore.DAL
{
    public class TestDAL : Connection, IDisposable
    {
        private ServiceLayer serviceLayer;

        public TestDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }

        public async Task<ResponseData> getQR(ListTest value)
        {
            ResponseData response = new ResponseData();
            foreach (Test item in value.ltest)
            {
                response = await serviceLayer.Request("/b1s/v1/QrCodes", Utils.Other.Method.POST, JsonConvert.SerializeObject(item));
                var responseBody = await response.Data.Content.ReadAsStringAsync();
            }
            return response;
        }
        public async Task<ResponseData> getBoM(ListBoM value)
        {
            ResponseData response = new ResponseData();
            foreach (BoM item in value.lbom)
            {
                response = await serviceLayer.Request("/b1s/v1/ProductTrees('"+item.TreeCode+"')", Utils.Other.Method.PATCH, JsonConvert.SerializeObject(item));
                var responseBody = await response.Data.Content.ReadAsStringAsync();
            }
            return response;
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
        ~TestDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
