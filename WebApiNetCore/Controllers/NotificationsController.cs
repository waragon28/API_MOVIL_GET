using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiNetCore.BO;
using WebApiNetCore.DAL;

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        [HttpGet("APP_SALESCALENDAR/")]
        public IActionResult Get(string imei, string from, string to)
        {
            try
            {
                SalesCalendars quotas = new SalesCalendars();
                NotificationDAL quota = new NotificationDAL();
                quotas = quota.GetSalesCalendars(imei, from, to);
                return Ok(quotas);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
        [HttpGet("APP_SERVICE/")]
        public IActionResult GetService(string imei, string code)
        {
            try
            {
                Services quotas = new Services();
                NotificationDAL quota = new NotificationDAL();
                quotas = quota.GetService(imei, code);
                return Ok(quotas);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
        [HttpPost("Quotation/")]
        public IActionResult GetQuotation(GetQuotation query)
        {
            try
            {
                QuotationNotifications quotas = new QuotationNotifications();
                NotificationDAL quota = new NotificationDAL();
                quotas = quota.GetQuotaNotification(query);
                return Ok(quotas);
            }
            catch (Exception)
            {
                return BadRequest("No se pudo concluir la busqueda");
            }
        }
    }
}
