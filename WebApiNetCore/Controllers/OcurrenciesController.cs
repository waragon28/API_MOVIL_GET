﻿using Microsoft.AspNetCore.Mvc;
using SAP_Core.BO;
using SAP_Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcurrenciesController : ControllerBase
    {
        [HttpGet("Dispatch")]
        public IActionResult Get([FromQuery] string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                
                OcurrenciasDAL ocurrencia = new OcurrenciasDAL();
                ListOcurrencies ocurrencias = ocurrencia.GetOcurrenciasDispatch(imei);
                return Ok(ocurrencias);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
        
        [HttpGet("FreeTransfer")]
        public IActionResult GetFree([FromQuery] string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                else
                {
                    OcurrenciasDAL ocurrencia = new();
                    ListOcurrencies ocurrencias = ocurrencia.GetOcurrenciasFreeTransfer(imei);
                    return Ok(ocurrencias);
                }
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
    }
}
