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
using System.Configuration;

namespace SAP_Core.Utils
{
    public class S3_Imagen
    {
        public static string SaveImage(string ImgStr, string nombre)
        {
            String path = HttpContext.Current.Server.MapPath("~/ImageStorage"); //Path

            //Check if directory exist
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            string imageName = nombre + ".jpg";

            //set the image path
            string imgPath = Path.Combine(path, imageName);

            byte[] imageBytes = Convert.FromBase64String(ImgStr);

            //sendMyFileToS3(t,"vistony","IndiaImage",imageName);
            File.WriteAllBytes(imgPath, imageBytes);
            string r = UploadFile(imgPath, imageName, "API");
            return r;
        }

        public static string UploadFile(string filePath, string fileName, string folderName)
        {
            string bucketName = "vistony";
            //public static string filePath = "d:\\test upload.txt";
            string endpoingURL = "https://nyc3.digitaloceanspaces.com";
            IAmazonS3 s3Client;
            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = endpoingURL
            };
            s3Client = new AmazonS3Client(s3ClientConfig);
            try
            {
                var fileTransferUtility = new TransferUtility(s3Client);
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName + @"/" + folderName,
                    FilePath = filePath,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 6291456, // 6 MB
                    Key = fileName,
                    CannedACL = S3CannedACL.PublicRead
                };
                fileTransferUtility.Upload(fileTransferUtilityRequest);
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
                request.BucketName = bucketName;
                request.Key = @"API/" + fileName;
                request.Expires = DateTime.Now.AddDays(20);
                request.Protocol = Protocol.HTTP;
                string url = s3Client.GetPreSignedURL(request);

                return ShortUrl.CompressURL(url);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                if (e.Message.Contains("disposed"))
                    return "No se puede concluir";
            }
            return "No se pudo conluir";
        }
        public static string DownloadApk(string version)
        {
            string url = string.Empty;
            string bucketName = "vistony";
            //public static string filePath = "d:\\test upload.txt";
            string endpoingURL = "https://nyc3.digitaloceanspaces.com";
            IAmazonS3 s3Client;
            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = endpoingURL
            };
            s3Client = new AmazonS3Client(s3ClientConfig);
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
            request.BucketName = bucketName;
            request.Key = "SalesForceApp/Release/cl/prod/" + version + ".apk";
            request.Expires = DateTime.Now.AddDays(20);
            request.Protocol = Protocol.HTTP;
            url = s3Client.GetPreSignedURL(request);
            return url;
        }
    }
}
