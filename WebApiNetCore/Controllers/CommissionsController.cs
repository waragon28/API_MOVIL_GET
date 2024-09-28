using Microsoft.AspNetCore.Mvc;
using QRCoder;
using SAP_Core.BO;
using SAP_Core.DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommissionsController : ControllerBase
    {
        // GET: api/<CommissionsController>
        [HttpGet]
        public IActionResult Get(string imei, string Year, string Month)
        {
            try
            {

                ListaComisiones commissions = null;
                ComisionesDAL comisionDAL = new ComisionesDAL();
                commissions = comisionDAL.GetComision(imei, Year, Month);

                return Ok(commissions);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
        [HttpGet("GetCommissionsDetail")]
        public IActionResult Get(string imei,string Variable)
        {
            try
            {
                CommissionDetals listComisionDetalle = null;
                ComisionesDAL comisionDAL = new ComisionesDAL();
                listComisionDetalle = comisionDAL.GetDetalleComision(imei, Variable);
                return Ok(listComisionDetalle);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

        [HttpGet("text")]
        public IActionResult GenerateQRCode(string text)
        {
            // Codificar el texto en Base64 para que sea seguro en una URL
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            string encodedText = Convert.ToBase64String(textBytes);

            // Crear un generador de código QR
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(encodedText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            // Crear una imagen del código QR
            Bitmap qrCodeImage = qrCode.GetGraphic(10);

            // Convertir la imagen a un arreglo de bytes
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageBytes = stream.ToArray();

                // Devolver la imagen como un archivo PNG
                return File(imageBytes, "image/png");
            }
        }




    }
}
