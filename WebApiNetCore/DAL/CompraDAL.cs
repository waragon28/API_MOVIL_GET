using Amazon.S3;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SalesForce.Util;
using SAP_Core.DAL;
using SAP_Core.Utils;
using System;

namespace WebApiNetCore.DAL
{
    public class CompraDAL : Connection, IDisposable
    {
        private ServiceLayer serviceLayer;
        private bool disposedValue;
        private static readonly string _awsAccessKey = Startup.Configuration.GetValue<string>("S3:AWSAccessKey");
        private static readonly string _awsSecretKey = Startup.Configuration.GetValue<string>("S3:AWSSecretKey");
        private static readonly string _endpoingURL = Startup.Configuration.GetValue<string>("S3:EndpoingURL");
        private static readonly string _bucketName = Startup.Configuration.GetValue<string>("S3:Bucketname");

        public CompraDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
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


        public string getUrlImagen(string fileNameAndExtension)
        {
            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = _endpoingURL
            };

            IAmazonS3 s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, s3ClientConfig);
            fileNameAndExtension = @"SalesForceApp/Compras/VISTONY/" + fileNameAndExtension;
            string text = S3_Imagen.getUrlImage2(s3Client, _bucketName, fileNameAndExtension);
            return text;
        }


        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~CompraDAL()
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
