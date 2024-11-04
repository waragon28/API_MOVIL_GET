using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Amazon.S3;
using Amazon;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using WebApiNetCore;
using System.Net.Http;
using Sentry;
using System.Reflection.Metadata.Ecma335;
using System.ComponentModel.DataAnnotations;

namespace SAP_Core.Utils
{
    public enum IMAGENES { DISTRIBUCIÓN, DIRECCIONES,SUPERVISORES,TRADE_MARKETING,COMPROBACION_DATOS_CLIENTE };
    public class S3_Imagen
    {
        private static readonly string CompanyDB = Startup.Configuration.GetValue<string>("ServiceLayer:PE:CompanyDB");
        private static bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }

        public static string getUrlImage2(IAmazonS3 s3Client, string BucketName, string FileName)
        {
            string path = "https://vistony.pe/wp-content/uploads/2021/04/cilindro-1-324x324.png";

            GetObjectMetadataRequest requestE = new()
            {
                BucketName = BucketName,
                //Key = "ClientesApp/Productos/" + ItemCode + ".png"
                Key = FileName
            };

            try
            {
                s3Client.GetObjectMetadataAsync(requestE).GetAwaiter().GetResult();

                GetPreSignedUrlRequest request = new()
                {
                    BucketName = BucketName,
                    Key = FileName,
                    Expires = DateTime.Now.AddMinutes(5)
                };

                s3Client.GetPreSignedURL(request);

                path = s3Client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception s3Exception)
            {
                if (s3Exception.ErrorCode != "NotFound")
                {
                    //send error sentry
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }

            return path;
        }
        public static string getUrlImage(IAmazonS3 s3Client, string BucketName,string FileName)
        {
            string path = "https://vistony.pe/wp-content/uploads/2021/04/cilindro-1-324x324.png";

            GetObjectMetadataRequest requestE = new()
            {
                BucketName = BucketName,
                //Key = "ClientesApp/Productos/" + ItemCode + ".png"
                Key = FileName
            };

            try
            {
                s3Client.GetObjectMetadataAsync(requestE).GetAwaiter().GetResult();

                GetPreSignedUrlRequest request = new()
                {
                    BucketName = BucketName,
                    Key = FileName,
                    Expires = DateTime.Now.AddMinutes(5)
                };

                s3Client.GetPreSignedURL(request);

                path = s3Client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception s3Exception)
            {
                if (s3Exception.ErrorCode != "NotFound")
                {
                    //send error sentry
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }

            return path;
        }
        public static async Task<string> Upload(IAmazonS3 s3Client,string _bucketName,string fileBase64, string fileName, IMAGENES imagenes)
        {
            byte[] bytes;

            if (fileBase64 !=null && IsBase64String(fileBase64))
            {
                bytes = Convert.FromBase64String(fileBase64);
            }
            else
            {
                return null;
            }
            
            var contents = new StreamContent(new MemoryStream(bytes));

            string path = "";

            switch (imagenes)
            {
                case IMAGENES.DIRECCIONES:
                    path= @"SalesForceApp/Direcciones/pe/" + fileName;
                    break;
                case IMAGENES.DISTRIBUCIÓN:
                    path= @"SalesForceApp/Distribucion/"+CompanyDB+"/"+ timestamp() +"/"+ fileName;
                    break;
                case IMAGENES.SUPERVISORES:
                    path = @"SalesForceApp/Supervisores/" + CompanyDB + "/" + fileName;
                    break;
                case IMAGENES.TRADE_MARKETING:
                    path = @"SalesForceApp/TradeMarketing/" + CompanyDB + "/" + timestamp() + "/" + fileName;
                    break;
                case IMAGENES.COMPROBACION_DATOS_CLIENTE:
                    path = @"SalesForceApp/ComprobacionDatosCliente/" + CompanyDB + "/" + timestamp() + "/" + fileName;
                    break;
            }

            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = path,
                    InputStream = contents.ReadAsStream(),
                    ContentType = "image/png",
                    CannedACL = S3CannedACL.AuthenticatedRead
                };

               
                string Metodo = Startup.Configuration.GetValue<string>("SL:Metodo");
                string Puerto = Startup.Configuration.GetValue<string>("SL:Puerto");
                string IP = Startup.Configuration.GetValue<string>("SL:IP_Interna") + Puerto + Metodo;


                await s3Client.PutObjectAsync(request);

                if (imagenes == IMAGENES.SUPERVISORES)
                {
                    return IP +"api/Superviser/Photo?Name=" + CompanyDB + "/" + fileName;
                }
                if (imagenes == IMAGENES.DIRECCIONES)
                {
                    return IP + "api/Customers/Address/Photo?Name="+ fileName;
                }
                if (imagenes == IMAGENES.TRADE_MARKETING)
                {
                    return IP + "api/survey/Photo?Name=" + CompanyDB + "/" + timestamp() + "/" + fileName;
                }
                if (imagenes == IMAGENES.COMPROBACION_DATOS_CLIENTE)
                {
                    return IP + "api/Customers/Address/DatosCliente?Name=" + CompanyDB + "/" + timestamp() + "/" + fileName;
                }
                return IP + "api/Dispatch/Photo?Name=" + CompanyDB  + "/" + timestamp() + "/" +  fileName;
            }
            catch (AmazonS3Exception s3Exception)
            {
                SentrySdk.CaptureException(s3Exception);
                return s3Exception.Message.ToString();
            }
        }

        private static string timestamp()
        {
            string date = DateTime.Now.Day.ToString();
            string Month = DateTime.Now.Month.ToString();
            string Year = DateTime.Now.Year.ToString();

            return Year + "/" + Month + "/" + date;
        }
    }
}
