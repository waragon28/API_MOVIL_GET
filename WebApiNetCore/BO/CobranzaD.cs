using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace SAP_Core.BO
{
    public class CobranzaDBO
    {
        public string Code { get; set; }
        public string BankName { get; set; }
        public string DocEntry { get; set; }
        public string BankID { get; set; }
        public string DepositID { get; set; }
        public string ItemDetail { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LegalNumber { get; set; }
        public string DocEntryFT { get; set; }
        public string DocNum { get; set; }
        public decimal DocTotal { get; set; }
        public decimal Balance { get; set; }
        public decimal AmountCharged { get; set; }
        public decimal NewBalance { get; set; }
        public string IncomeDate { get; set; }
        public int Receip { get; set; }
        public string Status { get; set; }
        public string Commentary { get; set; }
        public string SlpCode { get; set; }
        public string UserCode { get; set; }
        public string Banking { get; set; }
        public string QRStatus { get; set; }
        public string CancellationReason { get; set; }
        public string DirectDeposit { get; set; }
        public string POSPay { get; set; }
        public string U_VIS_CollectionSalesperson { get; set; }
        public string U_VIS_Type { get; set; }
    }
    public class ListaCobranzaD
    {
        public List<CobranzaDBO> CollectionDetail { get; set; }
    }

    public class ListaDepositsStatus2
    {
        public List<DepositsStatus2> Deposits { get; set; }
    }


    public class DepositsStatus2{

        public string Code { get; set; }
        public string U_VIS_BankID { get; set; }
        public string BankName { get; set; }
        public string U_VIS_IncomeType { get; set; }
        public string U_VIS_Deposit { get; set; }
        public string U_VIS_Date { get; set; }
        public string U_VIS_DeferredDate { get; set; }
        public string U_VIS_Banking { get; set; }
        public string U_VIS_UserID { get; set; }
        public string U_VIS_SlpCode { get; set; }
        public string U_VIS_AmountDeposit { get; set; }
        public string U_VIS_Status { get; set; }
        public string U_VIS_Comments { get; set; }
        public string U_VIS_CancelReason { get; set; }
        public string U_VIS_DirectDeposit { get; set; }
        public string U_VIS_POSPay { get; set; }
        public string U_VIS_CollectionSalesPerson { get; set; }
        public string U_VIS_BankValidation { get; set; }
        public string U_VIS_ObservationStatus { get; set; }

    }
}
