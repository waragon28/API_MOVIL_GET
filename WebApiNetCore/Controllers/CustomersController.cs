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

        [HttpPost("CreacionCustomers")]
        public IActionResult CreacionCustomers(CustomerCreate Customers)
        {

            HttpStatusCode Status = HttpStatusCode.OK;
            Response rs = new();

            try
            {
                CustomerCreate ObjCustomerCreate = new();
                ObjCustomerCreate.CardCode = "C" + Customers.FederalTaxID.ToString();
                ObjCustomerCreate.CardName = Customers.CardName.ToString();
                ObjCustomerCreate.CardType = Customers.CardType.ToString();
                ObjCustomerCreate.GroupCode = 102;
                ObjCustomerCreate.PayTermsGrpCode = -1;
                ObjCustomerCreate.FederalTaxID = Customers.FederalTaxID.ToString();
                ObjCustomerCreate.Currency = Customers.Currency;
                ObjCustomerCreate.EmailAddress = Customers.EmailAddress.ToString();
                ObjCustomerCreate.U_SYP_BPAP = Customers.U_SYP_BPAP.ToString();
                ObjCustomerCreate.U_SYP_BPAM = Customers.U_SYP_BPAM.ToString();
                ObjCustomerCreate.U_SYP_BPNO = Customers.U_SYP_BPNO.ToString();
                ObjCustomerCreate.U_SYP_BPN2 = Customers.U_SYP_BPN2.ToString();
                ObjCustomerCreate.U_SYP_BPTP = Customers.U_SYP_BPTP;
                ObjCustomerCreate.U_SYP_BPTD = Customers.U_SYP_BPTD;

                if (Customers.U_SYP_BPTD == "6")
                {
                    ObjCustomerCreate.U_SYP_DOCMAS = "01";
                }
                else
                {
                    ObjCustomerCreate.U_SYP_DOCMAS = "03";
                }

                ObjCustomerCreate.U_SYP_PLVAR = Customers.U_SYP_PLVAR.ToString();
                ObjCustomerCreate.U_SYP_CATCLI = Customers.U_SYP_CATCLI; //Giro_Negocio__c
                ObjCustomerCreate.Phone1 = Customers.Phone1.ToString();
                ObjCustomerCreate.Phone2 = Customers.Phone2.ToString();
                ObjCustomerCreate.BPAddresses = Customers.BPAddresses;
                ObjCustomerCreate.ContactEmployees = Customers.ContactEmployees;
                ObjCustomerCreate.U_EconomyActivity = Customers.U_EconomyActivity;//Actividad__c
                ObjCustomerCreate.U_VIS_SaleCategory = Customers.U_VIS_SaleCategory.ToString();
                ObjCustomerCreate.U_VIS_Category = Customers.U_VIS_Category.ToString();


                string Login = "https://ecs-dbs-vistony:50000/b1s/v1/Login";

                LoginRequest objLogin = new()
                {
                    CompanyDB = "B1H_VIST_PE",
                    Password = "SLV147",
                    UserName = "B1H_VIST_PE"
                };


                // Llamada al método que realiza la solicitud
                var response = ServiceLayer.SendLoginRequest(Login, objLogin);

                // Manejo de la respuesta
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    // Deserializar la respuesta JSON a un objeto dinámico
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);

                    var Url = "https://ecs-dbs-vistony:50000/b1s/v1/BusinessPartners";

                    var jsonObject = JObject.Parse(JsonConvert.SerializeObject(ObjCustomerCreate).ToString());


                    // Actualizar valores en la colección "BPAddresses"
                    foreach (var address in jsonObject["BPAddresses"])
                    {
                        address["U_VIS_VisitOrder"] = "1";
                        address["U_VIS_TerritoryID"] = "Z0001";
                    }

                    JObject updatedObject = new JObject(); // Define el objeto fuera del bloque

                    updatedObject = new JObject
                    {
                        { "PriceListNum", 28 },
                        { "U_VIST_IMPVTA", "IGV" }
                    };

                    // Agregar las propiedades restantes de jsonObject al nuevo objeto
                    foreach (var property in jsonObject.Properties())
                    {
                        updatedObject.Add(property.Name, property.Value);
                    }


                    // Convertir el objeto de nuevo a JSON
                    string updatedJson = JsonConvert.SerializeObject(updatedObject);
                    ServiceLayer SL = new ServiceLayer(_memoryCache);

                    // Realizar la segunda solicitud utilizando el token
                    rs = SL.PostWithToken(Url, jsonResponse.SessionId.ToString(), updatedJson);
                    if (rs.statusCode == HttpStatusCode.Created)
                    {
                        // Deserializar a un objeto dynamic
                        dynamic obj = JsonConvert.DeserializeObject<dynamic>(rs.data);
                        rs.data = "Se a Creado el Cliente Correctamente " + obj.CardCode;
                    }
                    else
                    {
                        rs.statusCode = rs.statusCode;
                        rs.data = rs.data;
                    }



                }






                return Ok(rs);
            }
            catch (Exception e)
            {
                return BadRequest("No se pudo concluir la busqueda" + e.Message.ToString());
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
