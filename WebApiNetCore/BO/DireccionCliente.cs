//#define VISTONY
//#define ROFALAB

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class DireccionClienteBO
    {
        //public string CardCode { get; set; }
        public string ShipToCode { get; set; }
        public int DayDelivery { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string AddressCode { get; set; }
        public string Street { get; set; }
        public string TerritoryID { get; set; }
        public string Territory { get; set; }
        public string Warehouse { get; set; }
        public string SlpCode { get; set; }
        public string ZipCode { get; set; }
        public string SlpName { get; set; }
        public string GeolocationDate { get; set; }
        public string titular { get; set; }
    }
    public class ListaDirecciones
    {
        public List<DireccionClienteBO> Addresses { get; set; }
    }



    public class inAddressesList
    {
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Token { get; set; }

        [Required]
        public Address[] Addresses { get; set; }
    }

    public class Address
    {
        [Required]
        public string AddressCode { get; set; }

        [Required]
        public string CardCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Latitude { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Longitude { get; set; }
        public string Photo { get; set; }
        public string DateGeolocation { get; set; }
        public string email { get; set; }
        public string mobilephone { get; set; }
        public string phone { get; set; }

        public string photocomprobation { get; set; }
        public string rubric { get; set; }
        public string verificationcode { get; set; }

        //public string categorycode { get; set; }
        public string lineofbusinessCode { get; set; }
        public string activityeconomiccode { get; set; }




    }

    public class AddressResponseList
    {
       public List<AddressResponse> Addresses { get; set; }
    }

    public class AddressResponse
    {
        public int AddressCode { get; set; }
        public string CardCode { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
    }

    public class Bpaddress
    {
        public string BPCode { get; set; }
        public string RowNum { get; set; }
        public string U_VIS_LongitudeApp { get; set; }
        public string U_VIS_LatitudeApp { get; set; }
        public string U_VIS_GeolocationDate { get; set; }
        public string U_VIS_Photo { get; set; }
    }

    public class ClienteDatos
    {
        public string EmailAddress { get; set; }
        public string Phone1 { get; set; }
        public string Cellular { get; set; }
        //public string U_VIS_SaleCategory { get; set; }
        public string U_SYP_CATCLI { get; set; }
        public string U_EconomyActivity { get; set; }


    }

    public class ConfirmaDatosClie
    {
        public string U_CardCode { get; set; }
        public string U_Fecha { get; set; }
        public string U_Hora { get; set; }
        public string U_rubric { get; set; }
        public string U_photocomprobation { get; set; }
        public string U_verificationcode { get; set; }
        public string U_email { get; set; }
        public string U_mobilephone { get; set; }
        public string U_phone { get; set; }

    }

}
