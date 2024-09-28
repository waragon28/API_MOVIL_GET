using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SalesForce.BO;
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
    public class SuperviserController : ControllerBase
    {
        private IMemoryCache _memoryCache;
        public SuperviserController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        [HttpGet]
        public IActionResult Get(string imei, string date)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                FormsHeader agencias = new FormsHeader();
                FormsDAL agencia = new(_memoryCache);
                agencias = agencia.getForms(imei,date);
                return Ok(agencias);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
        [HttpGet("Forms")]
        public IActionResult GetForms(string fini, string fin, string imei)
        {
            try
            {
                ListFormsHeader agencias = new ListFormsHeader();
                FormsDAL agencia = new(_memoryCache);
                agencias = agencia.getFormsDate(fini, fin, imei);
                return Ok(agencias);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FormsHeader data)
        {
            try
            {
                FormsDAL agencia = new(_memoryCache);
                ResponseData response = await agencia.postForms(data);

                return Ok(response);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
        [HttpGet("Photo")]
        public RedirectResult GetPhoto(string Name)
        {
            FormsDAL formDAL = new(_memoryCache);
            return Redirect(formDAL.getUrlImagen(Name));
        }
    }
}
