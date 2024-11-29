using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SalesForce.Util;
using Sap.Data.Hana;
using SAP_Core.DAL;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Sentry;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using WebApiNetCore.BO;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using SalesForce.BO;
using static WebApiNetCore.Utils.Other;
using System.Net;
using SAP_Core.BO;
using SixLabors.ImageSharp.ColorSpaces;

namespace WebApiNetCore.DAL
{
    public class SurveyDAL : Connection, IDisposable
    {
        private ServiceLayer serviceLayer;
        UsuarioDAL user = new UsuarioDAL();
        private bool disposedValue;
        private static readonly string _awsAccessKey = Startup.Configuration.GetValue<string>("S3:AWSAccessKey");
        private static readonly string _awsSecretKey = Startup.Configuration.GetValue<string>("S3:AWSSecretKey");
        private static readonly string _endpoingURL = Startup.Configuration.GetValue<string>("S3:EndpoingURL");
        private static readonly string _bucketName = Startup.Configuration.GetValue<string>("S3:Bucketname");

        DateTime FechaActual = DateTime.Now;
        public SurveyDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }

        public string GetEncuesta(string IdEncuesta)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            string strSQL = string.Format("CALL {0}.APP_ENCUESTA ('{1}')", DataSource.bd(), IdEncuesta);
            var jsonArray=string.Empty;
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                HanaCommand command = new(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                
               
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string jsonString = reader["JSONRESULT"].ToString();
                        jsonString = jsonString.Replace("\\", "");
                        // Realizar el reemplazo// Realizar el reemplazo
                        jsonString = jsonString.Replace("\": \"[{", "\": [{");

                        // Corregir barras diagonales adicionales
                        string correctedJson = jsonString.Replace("\\\"", "\"").Replace("\"[{", "[{").Replace("}]\"", "}]");

                        // Eliminar comillas adicionales
                        //correctedJson = Regex.Replace(correctedJson, @"""(\[{.*}\])""", match => match.Groups[1].Value);
                        jsonArray = correctedJson;

                    }
                }
               /* despachos.Obtener_DespachoCResult = listaDespacho;*/
                connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetDespachoHeader - " + ex.Message + " " + IdEncuesta + " " + FechaActual.ToString("dd-MM-yyyy"));
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();

                SentrySdk.CaptureException(ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return jsonArray;

        }
        public async Task<ResponseData> PostEncuesta(Object JSONEncuesta)
        {
            ResponseData response =null;
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            var jsonArray = string.Empty;
            LoginSL sl = user.loginServiceLayer().GetAwaiter().GetResult();
            try
            {
                VIS_MKT_OENC objSurveyBO = new VIS_MKT_OENC();

                var s3ClientConfig = new AmazonS3Config
                {
                    ServiceURL = _endpoingURL
                };

                IAmazonS3 s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, s3ClientConfig);

                // Deserializa el JSON
                SurveyBO SurveyC = JsonConvert.DeserializeObject<SurveyBO>(JSONEncuesta.ToString()) ;
                string strSQL = string.Format("SELECT * FROM {0}.OHEM WHERE \"U_VIS_Imei\"='{1}'", DataSource.bd(), SurveyC.Imei);
                connection.Open();
                HanaCommand command = new(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                
                if (reader.HasRows)
                {
                    List<VISMKTENC1Collection> ListVISMKTENC1Collection = new List<VISMKTENC1Collection>();
                    while (reader.Read())
                    {
                        string CardCode = string.Empty;
                        objSurveyBO.U_SlpCode =Convert.ToInt32(reader["salesPrson"].ToString());
                        objSurveyBO.U_CardCode = SurveyC.cardCode;
                        CardCode= SurveyC.cardCode;

                        foreach (var Secc in SurveyC.Section)
                        {
                            int U_Seccion = Secc.U_File; 
                            string U_Desc_Seccion = Secc.U_Descr; 
                            int U_SubSeccion = 0;
                            string U_Desc_SubSeccion = string.Empty;
                            int U_Formulario = 0;
                            string U_Desc_Formulario = string.Empty;
                            int U_Preguntas = 0;
                            string U_Desc_Pregunta = string.Empty;
                            string U_Respuesta = string.Empty;
                            string U_Foto = string.Empty;
                            string U_Foto_Pregunta = string.Empty;
                            string U_RespSelect = string.Empty;
                            string U_RespText = string.Empty;
                            foreach (var SubSecc in Secc.SubSeccion)
                            {
                                DateTime thisDate1 = new DateTime();

                                U_SubSeccion = SubSecc.DocEntry;
                                U_Desc_SubSeccion = SubSecc.Titulo_SubSeccion;

                                foreach (var Form in SubSecc.Formulario)
                                {
                                    U_Formulario = Form.DocEntry;
                                    U_Desc_Formulario = Form.U_Title;
                                    string U_FotoUrl = "Trade_Marketing" + "_" + Form.U_Title + CardCode + thisDate1.ToString("yyyyddMM") + "_Document.png";
                                    if (Form.Base64 == "")
                                    {
                                        U_Foto = "";
                                    }
                                    else
                                    {
                                        U_Foto = "";// S3_Imagen.Upload(s3Client, _bucketName, Form.Base64, U_FotoUrl.Replace(" ","_"), IMAGENES.TRADE_MARKETING).GetAwaiter().GetResult();
                                    }

                                    foreach (var Pregunta in Form.Preguntas)
                                    {
                                        U_Preguntas = Pregunta.U_Question;
                                        U_Desc_Pregunta = Pregunta.U_Descr;
                                        U_RespText = Pregunta.U_Responsevalue;
                                        string U_Foto_Url_Pregunta = "Trade_Marketing" + "_" + Pregunta.U_Descr + CardCode + thisDate1.ToString("yyyyddMM") + "_Document.png";
                                        if (Pregunta.Base64=="")
                                        {
                                            U_Foto_Pregunta = "";
                                        }
                                        else
                                        {
                                            U_Foto_Pregunta = "";// S3_Imagen.Upload(s3Client, _bucketName, Pregunta.Base64, U_Foto_Url_Pregunta.Replace(" ", "_"), IMAGENES.TRADE_MARKETING).GetAwaiter().GetResult();
                                        }

                                        foreach (var Respuesta in Pregunta.Respuestas)
                                            {
                                                U_Respuesta = Respuesta.U_answer;
                                                U_RespSelect = Respuesta.U_Responsevalue;
                                                LISMKTENC1Collection(U_Seccion, U_Desc_Seccion, U_SubSeccion, U_Desc_SubSeccion, U_Formulario, U_Desc_Formulario, U_Preguntas, U_Desc_Pregunta, U_Respuesta, U_Foto, U_Foto_Pregunta, U_RespSelect, U_RespText);
                                            }
                                        
                                    }

                                }

                            }
                        }
                        objSurveyBO.VIS_MKT_ENC1Collection = ListVISMKTENC1;

                        // Serializar la instancia en formato JSON
                        string json = JsonConvert.SerializeObject(objSurveyBO);
             
                         response = await serviceLayer.Request(String.Format("/b1s/v1/VIS_MKT_OENC"), Method.POST, json, sl.token);
                      
                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            dynamic responseBody = await response.Data.Content.ReadAsStringAsync();
                            responseBody = JsonConvert.DeserializeObject(responseBody.ToString());

                            response.Data = responseBody;
                            response.StatusCode = HttpStatusCode.OK;

                        }
                        else
                        {
                            var responseBody = await response.Data.Content.ReadAsStringAsync();
                            ResponseError error = System.Text.Json.JsonSerializer.Deserialize<ResponseError>(responseBody);


                            response.StatusCode = HttpStatusCode.FailedDependency;
                            response.Data = error;
                        }


                    }
                }
                        

                
                connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                response.StatusCode = HttpStatusCode.FailedDependency;
                response.Data = ex.Message.ToString();

                string strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force POST Encuesta", "Error", "Despacho_GetDespachoHeader - " + ex.Message + " " + JSONEncuesta + " " + FechaActual.ToString("dd-MM-yyyy"));
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();

                SentrySdk.CaptureException(ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                user.LogoutServiceLayer().GetAwaiter().GetResult();
            }
            return response;

            //  return jsonArray;

        }


        public string getUrlImagen(string fileNameAndExtension)
        {
            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = _endpoingURL
            };

            IAmazonS3 s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, s3ClientConfig);
            fileNameAndExtension = @"SalesForceApp/TradeMarketing/"+ fileNameAndExtension;
            string text = "";// S3_Imagen.getUrlImage(s3Client, _bucketName, fileNameAndExtension);
            return text;
        }

        public VIS_MKT_OENC Survey(string CardCode,int CodVent, List<VISMKTENC1Collection> ListVISMKTENC1Collection)
        {
            VIS_MKT_OENC ObjVIS_MKT_OENC = new VIS_MKT_OENC();
            ObjVIS_MKT_OENC.U_CardCode = CardCode;
            ObjVIS_MKT_OENC.U_SlpCode = CodVent;
           // ObjVIS_MKT_OENC.VIS_MKT_ENC1Collection = ListVISMKTENC1Collection;
            return ObjVIS_MKT_OENC;
        }
        
        List<VISMKTENC1Collection> ListVISMKTENC1 = new List<VISMKTENC1Collection>();

        public void LISMKTENC1Collection( int U_Seccion, string U_Desc_Seccion, int U_SubSeccion,
                                                                string U_Desc_SubSeccion, int U_Formulario,
                                                                string U_Desc_Formulario, int U_Preguntas,
                                                                string U_Desc_Pregunta,string U_Respuesta,string U_Foto,string U_Foto_Pregunta,string U_RespSelect,string U_RespText)

        {
            VISMKTENC1Collection VISMKTENC1Collection = new VISMKTENC1Collection();
            VISMKTENC1Collection.U_Seccion = U_Seccion;
            VISMKTENC1Collection.U_Desc_Seccion = U_Desc_Seccion;
            VISMKTENC1Collection.U_SubSeccion = U_SubSeccion;
            VISMKTENC1Collection.U_Desc_SubSeccion = U_Desc_SubSeccion;
            VISMKTENC1Collection.U_Formulario = U_Formulario;
            VISMKTENC1Collection.U_Desc_Formulario = U_Desc_Formulario;
            VISMKTENC1Collection.U_Preguntas = U_Preguntas;
            VISMKTENC1Collection.U_Desc_Pregunta = U_Desc_Pregunta;
            VISMKTENC1Collection.U_Respuesta = U_Respuesta;
            VISMKTENC1Collection.U_Foto = U_Foto;
            VISMKTENC1Collection.U_FotoPregunta = U_Foto_Pregunta;
            VISMKTENC1Collection.U_RespSelect = U_RespSelect;
            VISMKTENC1Collection.U_RespText = U_RespText;
            ListVISMKTENC1.Add(VISMKTENC1Collection);
            //return ListVISMKTENC1Collection;
        }

        public List<SurveyBO> GetEncuestaFrm(string FechaInicio, string FechaFin,string CodVendedor)
        {
            HanaDataReader reader;
            HanaDataReader readerSubSec;
            HanaDataReader readerSec;
            HanaDataReader readerForm;
            HanaDataReader readerPregunta;
            HanaDataReader readerRespuesta;

            HanaCommand command;
            HanaCommand commandSecc;
            HanaCommand commandSubSecc;
            HanaCommand commandPregunta;
            HanaCommand commandFormu;
            HanaCommand commandRespuesta;
            HanaConnection connection = GetConnection();
            List<SurveyBO> ListListEncuesta = new List<SurveyBO>();
            string strSQL = string.Empty;
            strSQL = string.Format("CALL {0}.APP_MKT_GET_ENCUESTA_C ('{1}','{2}','{3}')", DataSource.bd(), FechaInicio, FechaFin,CodVendedor);

            try
            {
                connection.Open();
                command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                SurveyBO objSurveyBO = new SurveyBO();
               // List<SurveyBO> ListobjSurveyBO = new List<SurveyBO>();
                List<Section> ListSection = new List<Section>();
                List<SubSeccion> ListSubSeccion = new List<SubSeccion>();
                List<Formulario> ListFormulario = new List<Formulario>();
                List<Pregunta> ListPregunta = new List<Pregunta>();
                List<Respuesta> ListRespuesta = new List<Respuesta>();

                string DocEntry = string.Empty;
                if (reader.HasRows)
                {
                    while (reader.Read()) // CONSTRUIR LA CABECERA DEL JSON
                    {
                        objSurveyBO = new SurveyBO();
                        ListSection = new List<Section>();
                        objSurveyBO.cardCode = reader["CardCode"].ToString();
                        objSurveyBO.cardName = reader["CardName"].ToString();
                        objSurveyBO.SlpCode = Convert.ToInt32(reader["SlpCode"].ToString());
                        objSurveyBO.DateCreation = reader["CreateDate"].ToString();
                        DocEntry = reader["DocEntry"].ToString();
                        strSQL = string.Format("CALL {0}.APP_MKT_GET_ENCUESTA_SECC_D ('{1}')", DataSource.bd(), DocEntry);
                        commandSecc = new HanaCommand(strSQL, connection);
                        readerSec = commandSecc.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        if (readerSec.HasRows)// CONSTRUIR LA SECCION DEL JSON
                        {

                            int Orden = 1;
                            while (readerSec.Read())
                            {
                                Section objSection = new Section();
                                objSection.U_File =Convert.ToInt32(readerSec["U_Seccion"].ToString());
                                objSection.U_Descr = readerSec["U_Desc_Seccion"].ToString();
                                objSection.U_Order = readerSec["Orden"].ToString();

                                strSQL = string.Format("CALL {0}.APP_MKT_GET_ENCUESTA_SUBSECC_D ('{1}','{2}')", DataSource.bd(), DocEntry, readerSec["U_Seccion"].ToString());
                                commandSubSecc = new HanaCommand(strSQL, connection);
                                readerSubSec = commandSubSecc.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                                    while (readerSubSec.Read())//CONSTRUIR LA SUBSECCION
                                    {
                                            ListSubSeccion = new List<SubSeccion>();
                                            SubSeccion objSubSeccion = new SubSeccion();
                                            objSubSeccion.DocEntry = Convert.ToInt32(readerSubSec["DocEntry"].ToString());
                                            string DocEntrySubSec = readerSubSec["DocEntry"].ToString();
                                            objSubSeccion.Titulo_SubSeccion = readerSubSec["U_Title"].ToString();
                                            objSubSeccion.SubTitulo_SubSeccion = readerSubSec["U_SubTitle"].ToString();

                                            strSQL = string.Format("CALL {0}.APP_MKT_GET_ENCUESTA_FORMU_D ('{1}','{2}')", DataSource.bd(), DocEntry, DocEntrySubSec);
                                            commandFormu = new HanaCommand(strSQL, connection);
                                            readerForm = commandFormu.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                                                int ordenForm = 1;
                                                ListFormulario = new List<Formulario>();
                                                ListPregunta = new List<Pregunta>();
                                                while (readerForm.Read()) //CONSTRUIR FORMULARIO
                                                {
                                                        Formulario ObjFormulario = new Formulario();
                                                        ObjFormulario.DocEntry = Convert.ToInt32(readerForm["U_Formulario"].ToString());
                                                        ObjFormulario.U_Title = readerForm["U_Desc_Formulario"].ToString();
                                                        ObjFormulario.U_Order = readerForm["U_Order"].ToString();
                                                        ObjFormulario.Url = readerForm["U_Foto"].ToString();
                                                        int IDFormulario = Convert.ToInt32(readerForm["U_Formulario"].ToString());
                                                        ordenForm++;
                                                        strSQL = string.Format("CALL {0}.APP_MKT_GET_ENCUESTA_PREGUNTA_D ('{1}','{2}','{3}')", DataSource.bd(), DocEntry, DocEntrySubSec, IDFormulario);
                                                        commandPregunta = new HanaCommand(strSQL, connection);
                                                        readerPregunta = commandPregunta.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                                                            int OrdenPregunta = 1;

                                                            ListPregunta = new List<Pregunta>();
                                                            while (readerPregunta.Read()) //CONSTRUIR PREGUNTA
                                                            {
                                                                Pregunta ObjPregunta = new Pregunta();
                                                                    ObjPregunta.U_Descr = readerPregunta["U_Desc_Pregunta"].ToString();
                                                                    ObjPregunta.U_Type = readerPregunta["U_Type"].ToString();
                                                                    ObjPregunta.Url = readerPregunta["U_Foto"].ToString();
                                                                    ObjPregunta.U_Order = OrdenPregunta.ToString();
                                                                    ObjPregunta.U_Responsevalue = readerPregunta["U_RespText"].ToString(); 
                                                                    string DocEntryPregunta= readerPregunta["U_Preguntas"].ToString();
                                                               
                                                                    strSQL = string.Format("CALL {0}.APP_MKT_GET_ENCUESTA_RESPUESTA_D ('{1}','{2}','{3}','{4}')", 
                                                                        DataSource.bd(), DocEntry, DocEntrySubSec, DocEntryPregunta, IDFormulario);
                                                                    commandRespuesta = new HanaCommand(strSQL, connection);
                                                                    readerRespuesta = commandRespuesta.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                                                                    int OrdenRespuesta = 1;
                                                                    
                                                                    ListRespuesta = new List<Respuesta>();

                                                                    while (readerRespuesta.Read()) //CONSTRUIR RESPUESTA
                                                                    {
                                                                        Respuesta ObjRespuesta = new Respuesta();
                                                                        ObjRespuesta.U_answer= readerRespuesta["U_Respuesta"].ToString();
                                                                        ObjRespuesta.U_Responsevalue= readerRespuesta["U_RespSelect"].ToString();
                                                                        ObjRespuesta.U_Order = readerRespuesta["U_Order"].ToString();

                                                                        ListRespuesta.Add(ObjRespuesta);
                                                                    }
                                                                    ListPregunta.Add(ObjPregunta);
                                                                    ObjPregunta.Respuestas = ListRespuesta;
                                                                    OrdenPregunta++;
                                                                }
                                                            ListFormulario.Add(ObjFormulario);
                                                            ObjFormulario.Preguntas = ListPregunta;
                                                }
                                        ListSubSeccion.Add(objSubSeccion);
                                        objSubSeccion.Formulario = ListFormulario;
                                    }
                                objSection.SubSeccion = ListSubSeccion;
                                ListSection.Add(objSection);
                                Orden++;
                            }
                        }

                        objSurveyBO.Section = ListSection;
                        ListListEncuesta.Add(objSurveyBO);
                    }

                }
                connection.Close();

            }
            catch (Exception e)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    e.Message.ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }


            return ListListEncuesta;

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

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~SurveyDAL()
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
