using Amazon.S3;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SalesForce.BO;
using SalesForce.Util;
using Sap.Data.Hana;
using SAP_Core.BO;
using SAP_Core.DAL;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiNetCore.BO;
using static WebApiNetCore.Utils.Other;

namespace WebApiNetCore.DAL
{
    public class FormsDAL : Connection, IDisposable
    {
        private ServiceLayer serviceLayer;
        UsuarioDAL user = new UsuarioDAL();
        public FormsDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }
        private static readonly string _awsAccessKey = Startup.Configuration.GetValue<string>("S3:AWSAccessKey");
        private static readonly string _awsSecretKey = Startup.Configuration.GetValue<string>("S3:AWSSecretKey");
        private static readonly string _endpoingURL = Startup.Configuration.GetValue<string>("S3:EndpoingURL");
        private static readonly string _bucketName = Startup.Configuration.GetValue<string>("S3:Bucketname");
        public ListFormsHeader getFormsDate(string fini, string ffin, string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListFormsHeader result = new ListFormsHeader();
            FormsHeader fh = new FormsHeader();
            List<FormsHeader> lfh = new List<FormsHeader>();
            string strSQL = string.Format("CALL {0}.APP_FORMULARIO_SUPERVISOR_DATE('{1}','{2}','{3}')", DataSource.bd(), fini, ffin, imei);
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        fh = JsonConvert.DeserializeObject<FormsHeader>(reader["Data"].ToString());
                        lfh.Add(fh);
                    }
                }
                result.Data = lfh;
            }
            catch (Exception EX)
            {
            }
            return result;
        }
        public FormsHeader getForms(string imei, string fecha)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            FormsHeader fh = new FormsHeader();
            List<form> lf = new List<form>();
            form f = new form();
            visit_data v = new visit_data();
            principal_data p = new principal_data();
            List<options> lo = new List<options>();
            options o = new options();
            string strSQL = string.Format("CALL {0}.APP_FORMULARIO_SUPERVISOR('{1}','{2}')", DataSource.bd(), imei, fecha);
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        List<form_query> dp = JsonConvert.DeserializeObject<List<form_query>>(reader["formulario"].ToString());
                        List<visit_data_query> vd = JsonConvert.DeserializeObject<List<visit_data_query>>(reader["datos_visita"].ToString());
                        List<principal_data_query> pd = JsonConvert.DeserializeObject<List<principal_data_query>>(reader["datos_principales"].ToString());
                        fh = new FormsHeader();
                        fh.id_supervisor = reader["id_supervisor"].ToString();
                        fh.id_vendedor = reader["id_vendedor"].ToString();
                        fh.comentario = reader["comentario"].ToString();
                        for (int row = 0; row < dp.Count; row++)
                        {
                            f = new form();
                            f.code = dp[row].code;
                            f.pregunta = dp[row].pregunta;
                            f.opciones = JsonConvert.DeserializeObject<List<options>>(dp[row].opciones);
                            f.respuesta = "";
                            lf.Add(f);
                        }
                        fh.formulario = lf;
                        for (int row = 0; row < vd.Count; row++)
                        {
                            v = new visit_data();
                            v.hora_fin = vd[row].hora_fin;
                            v.hora_inicio = vd[row].hora_inicio;
                            v.observacion_zona = vd[row].Observacion_zona;
                            v.zona = vd[row].Zona;
                            v.resumen = JsonConvert.DeserializeObject<List<resume>>(vd[row].resumen);
                            v.clientes_nuevos = vd[row].clientes_nuevos;
                            v.clientes_empadronados = vd[row].clientes_empadronados;
                        }
                        fh.datos_visita = v;
                        for (int row = 0; row < pd.Count; row++)
                        {
                            p = new principal_data();
                            p.fecha_hoy = pd[row].fecha;
                            p.nombre_supervisor = pd[row].nombre_supervisor;
                            p.nombre_vendedor = pd[row].nombre_vendedor;
                            p.num_informe = pd[row].num_informe.ToString();
                            p.tipo_salida = JsonConvert.DeserializeObject<List<exitType>>(pd[row].tipo_salida);
                        }
                        fh.datos_principales = p;
                        fh.galeria = new List<GalleryList>();
                    }
                }
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetAgencias - " + ex.Message);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return fh;
        }
        public async Task<ResponseData> postForms(FormsHeader data)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();

            LoginSL sl = user.loginServiceLayer().GetAwaiter().GetResult();

            FormSAP form = new FormSAP();
            List<exitTypeSAP> let = new List<exitTypeSAP>();
            List<cuestionarioSAP> lcs = new List<cuestionarioSAP>();
            List<GalleryCollection> lgc = new List<GalleryCollection>();
            FormSAPResponse frs = new FormSAPResponse();
            ResponseData response = new ResponseData();
            string strSQL = "";
            try
            {
                form = new FormSAP();
                form.Code = Guid.NewGuid().ToString();
                form.U_VIS_InternalDocument = data.datos_principales.num_informe;
                form.U_VIS_SlpCodeSupervisor = data.id_supervisor;
                form.U_VIS_SlpCodeSeller = data.id_vendedor;
                form.U_VIS_Date = data.datos_principales.fecha_hoy;
                form.U_VIS_TimeStart = data.datos_visita.hora_inicio;
                form.U_VIS_TimeEnd = data.datos_visita.hora_fin;
                form.U_VIS_ClientNewQuantity = Convert.ToInt32(data.datos_visita.clientes_nuevos);
                form.U_VIS_ClientRegisteredQuantity = Convert.ToInt32(data.datos_visita.clientes_empadronados);
                form.U_VIS_Zona = data.datos_visita.zona;
                foreach (resume item in data.datos_visita.resumen)
                {
                    if (item.tipo == "pedidos")
                    {
                        form.U_VIS_OrderQuantity = Convert.ToInt32(item.cantidad);
                        form.U_VIS_OrderAmount = Convert.ToDecimal(item.monto);
                    }
                    if (item.tipo=="cobranza")
                    {
                        form.U_VIS_CollectionQuantity = Convert.ToInt32(item.cantidad);
                        form.U_VIS_CollectionAmount = Convert.ToDecimal(item.monto);
                    }
                    if (item.tipo=="visita")
                    {
                        form.U_VIS_VisitsQuantity = Convert.ToInt32(item.cantidad); ;
                    }
                }
                form.U_VIS_Commentary = data.comentario;
                form.U_VIS_Observation = data.datos_visita.observacion_zona;
                foreach (exitType item in data.datos_principales.tipo_salida)
                {
                    if (item.marcado)
                    {
                        exitTypeSAP ex = new exitTypeSAP();
                        ex.U_VIS_TypeExit = item.valor;
                        ex.U_VIS_TypeExitName = item.opcion;
                        let.Add(ex);
                    }
                }
                form.VIS_APP_FTSTCollection = let;
                foreach (form item in data.formulario)
                {
                    cuestionarioSAP cs = new cuestionarioSAP();
                    cs.U_VIS_QuestionCode = item.code;
                    cs.U_VIS_QuestionName = item.pregunta;
                    cs.U_VIS_ResponseCode = item.opciones[item.opciones.FindIndex(a=> a.valor == Convert.ToInt32(item.valor))].code;
                    cs.U_VIS_ResponseName = item.respuesta;
                    cs.U_VIS_ResponseValue = item.valor;
                    lcs.Add(cs);
                }
                form.VIS_APP_FSCUCollection = lcs;
                var s3ClientConfig = new AmazonS3Config
                {
                    ServiceURL = _endpoingURL
                };
                IAmazonS3 s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, s3ClientConfig);
                int i = 0;
                foreach (GalleryList item in data.galeria)
                {
                    GalleryCollection gc = new GalleryCollection();
                    gc.U_VIS_Photo = S3_Imagen.Upload(s3Client, _bucketName, item.base64, form.U_VIS_SlpCodeSupervisor + "_"+ form.U_VIS_SlpCodeSeller +"_"+ form.U_VIS_Date+"_"+i+ ".png", IMAGENES.SUPERVISORES).GetAwaiter().GetResult();
                    i++;
                    lgc.Add(gc);
                }
                form.VIS_APP_FSGACollection = lgc;
                string json = JsonConvert.SerializeObject(form);
                response = await serviceLayer.Request("/b1s/v1/VIS_APP_FSCA", Method.POST, json,sl.token);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    frs.code = form.Code;
                    frs.num_informe = form.U_VIS_InternalDocument;
                    frs.Message = "Creado satisfactoriamente";
                    frs.ErrorCode = "0";

                    response.Data = frs;
                    response.StatusCode = HttpStatusCode.Created;
                }
                else
                {
                    var responseBody = await response.Data.Content.ReadAsStringAsync();
                    frs.code = "";
                    frs.num_informe = form.U_VIS_InternalDocument;
                    frs.Message = responseBody;
                    frs.ErrorCode = "1";

                    response.Data = frs;
                    response.StatusCode = HttpStatusCode.FailedDependency;
                }
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Forms_PostForm - " + ex.Message);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
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
        }
        public string getUrlImagen(string fileNameAndExtension)
        {
            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = _endpoingURL
            };

            IAmazonS3 s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, s3ClientConfig);
            fileNameAndExtension = @"SalesForceApp/Supervisores/" + fileNameAndExtension;
            string text = S3_Imagen.getUrlImage(s3Client, _bucketName, fileNameAndExtension);
            return text;
        }
        #region Disposable

        private bool disposing = false;
        /// <summary>
        /// Método de IDisposable para desechar la clase.
        /// </summary>
        public void Dispose()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        /// <summary>
        /// Método sobrecargado de Dispose que será el que
        /// libera los recursos, controla que solo se ejecute
        /// dicha lógica una vez y evita que el GC tenga que
        /// llamar al destructor de clase.
        /// </summary>
        /// <param name=”b”></param>
        protected virtual void Dispose(bool b)
        {
            // Si no se esta destruyendo ya…
            {
                if (!disposing)

                    // La marco como desechada ó desechandose,
                    // de forma que no se puede ejecutar este código
                    // dos veces.
                    disposing = true;
                // Indico al GC que no llame al destructor
                // de esta clase al recolectarla.
                GC.SuppressFinalize(this);
                // … libero los recursos… 
            }
        }




        /// <summary>
        /// Destructor de clase.
        /// En caso de que se nos olvide “desechar” la clase,
        /// el GC llamará al destructor, que tambén ejecuta la lógica
        /// anterior para liberar los recursos.
        /// </summary>
        ~FormsDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
