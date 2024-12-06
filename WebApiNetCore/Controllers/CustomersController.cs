using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesForce.BO;
using SalesForce.Util;
using SAP_Core.BO;
using SAP_Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using static WebApiNetCore.Utils.Other;

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private IMemoryCache _memoryCache;
        public CustomersController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string imei)
        {
            try
            { 
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                
                ClienteDAL clienteDAL = new(_memoryCache);
                ListaClientes2 clientes = clienteDAL.GetClientes2(imei);
                return Ok(clientes);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }


        [HttpGet("Activity_Economic")]
        public IActionResult GetActivity_Economic([FromQuery] string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }

                ClienteDAL clienteDAL = new(_memoryCache);
                Economic_Activity clientes = clienteDAL.GetActivity_Economic(imei);
                return Ok(clientes);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

        [HttpGet("Address/Photo")]
        public RedirectResult GetPhoto(string Name)
        {
            ClienteDAL formDAL = new(_memoryCache);
            return Redirect(formDAL.getUrlImagen(Name));
        }

        [HttpGet("Address/DatosCliente")]
        public RedirectResult GetPhotoDatosCliente(string Name)
        {
            ClienteDAL formDAL = new(_memoryCache);
            return Redirect(formDAL.getUrlImagenDatosCliente(Name));
        }
        
        [HttpPatch("Address/"), DisableRequestSizeLimit]
        public IActionResult UpdateAdddress([FromBody] inAddressesList parameters)
        {
            if (ModelState.IsValid)
            {
                ClienteDAL clienteDAL = new(_memoryCache);
                ResponseData apiResponse = clienteDAL.update(parameters).GetAwaiter().GetResult();

                if (apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    return Ok(apiResponse.Data);
                }
                else
                {
                    return StatusCode((int)apiResponse.StatusCode);
                }
            }
            else
            {
                return BadRequest();
            }
            return Ok();
        }


        [HttpGet("LineBusiness/")]
        public IActionResult LineBusiness(string imei)
        {
            if (ModelState.IsValid)
            {
                ClienteDAL clienteDAL = new(_memoryCache);
                OfBusiness apiResponse = clienteDAL.getLineBusiness(imei);
                return Ok(apiResponse);
               
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("Detail/")]
        public IActionResult Get2([FromQuery]string imei,string cliente)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                
                ClienteDAL clienteDAL = new ClienteDAL(_memoryCache);
                ListaClientes clientes = clienteDAL.GetCliente(imei, cliente);
                return Ok(clientes);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
        [HttpGet("ValidaCredit/")]
        public IActionResult GetValidateCredit([FromQuery] string salesPerson, string day)
        {
            try
            {
                ListaValidateCredit clientes = new ListaValidateCredit();
                ClienteDAL clienteDAL = new ClienteDAL(_memoryCache);
                clientes = clienteDAL.getValidateCredit(salesPerson, day);
                return Ok(clientes);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
    
    }
}
