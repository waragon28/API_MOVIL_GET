using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNetCore.BO
{
    public class ListFormsHeader
    {
        public List<FormsHeader> Data { get; set; }
    }
    public class FormsHeader
    {
        public string id_supervisor { get; set; }
        public string id_vendedor { get; set; }
        public principal_data datos_principales { get; set; }
        public visit_data datos_visita { get; set; }
        public List<form> formulario { get; set; }
        public string comentario { get; set; }
        public List<GalleryList> galeria { get; set; } = new List<GalleryList>();
    }
    public class GalleryList
    {
        public string base64 { get; set; } = "";
        public string uri { get; set; } = "";
        public string status { get; set; } = "";
        public string message { get; set; } = "";
        public string ErrorCode { get; set; } = "";
    }
    public class principal_data
    {
        public string num_informe { get; set; }
        public string fecha_hoy { get; set; }
        public string nombre_supervisor { get; set; }
        public string nombre_vendedor { get; set; }
        public List<exitType> tipo_salida { get; set; }
    }
    public class exitType
    {
        public string opcion { get; set; }
        public string valor { get; set; }
        public bool marcado { get; set; }
    }
    public class principal_data_query
    {
        public string fecha { get; set; }
        public string nombre_supervisor { get; set; }
        public string nombre_vendedor { get; set; }
        public int num_informe { get; set; }
        public string tipo_salida { get; set; }
    }
    public class visit_data
    {
        public string zona { get; set; }
        public string observacion_zona { get; set; }
        public string hora_inicio { get; set; }
        public string hora_fin { get; set; }
        public List<resume> resumen { get; set; }
        public string clientes_nuevos { get; set; }
        public string clientes_empadronados { get; set; }
    }
    public class resume
    {
        public string tipo { get; set; }
        public string cantidad { get; set; }
        public string monto { get; set; }
    }
    public class visit_data_query
    {
        public string Observacion_zona { get; set; }
        public string Zona { get; set; }
        public string clientes_empadronados { get; set; }
        public string clientes_nuevos { get; set; }
        public string hora_fin { get; set; }
        public string hora_inicio { get; set; }
        public string resumen { get; set; }
    }

    public class form
    {
        public string code { get; set; }
        public string pregunta { get; set; }
        public List<options> opciones { get; set; } = new List<options>();
        public string respuesta { get; set; }
        public string valor { get; set; }
    }
    public class options
    {
        public string code { get; set; }
        public string opcion { get; set; }
        public int valor { get; set; }
    }

    public class form_query
    {
        public string code { get; set; }
        public string pregunta { get; set; }
        public string opciones { get; set; }
    }
    public class FormSAPResponse
    {
        public string code { get; set; }
        public string num_informe { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
    }
    public class FormSAP
    {
        public string Code { get; set; }
        public string U_VIS_InternalDocument { get; set; }
        public string U_VIS_SlpCodeSupervisor { get; set; }
        public string U_VIS_SlpCodeSeller { get; set; }
        public string U_VIS_Date { get; set; }
        public string U_VIS_TimeStart { get; set; }
        public string U_VIS_TimeEnd { get; set; }
        public int U_VIS_OrderQuantity { get; set; }
        public decimal U_VIS_OrderAmount { get; set; }
        public int U_VIS_CollectionQuantity { get; set; }
        public decimal U_VIS_CollectionAmount { get; set; }
        public int U_VIS_VisitsQuantity { get; set; }
        public int U_VIS_ClientNewQuantity { get; set; }
        public int U_VIS_ClientRegisteredQuantity { get; set; }
        public string U_VIS_Commentary { get; set; }
        public string U_VIS_Observation { get; set; }
        public string U_VIS_Zona { get; set; }
        public List<exitTypeSAP> VIS_APP_FTSTCollection { get; set; }
        public List<cuestionarioSAP> VIS_APP_FSCUCollection { get; set; }
        public List<GalleryCollection> VIS_APP_FSGACollection { get; set; }
    }
    public class GalleryCollection
    {
        public string U_VIS_Photo { get; set; }
    }
    public class exitTypeSAP
    {
        public string U_VIS_TypeExit { get; set; }
        public string U_VIS_TypeExitName { get; set; }
    }
    public class cuestionarioSAP
    {
        public string U_VIS_QuestionCode { get; set; }
        public string U_VIS_ResponseCode { get; set; }
        public string U_VIS_QuestionName { get; set; }
        public string U_VIS_ResponseName { get; set; }
        public string U_VIS_ResponseValue { get; set; }

    }
}
