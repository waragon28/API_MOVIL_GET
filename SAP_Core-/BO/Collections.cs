using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class Recibo
    {
        public string DocEntry { get; set; }
    }
    public class ReciboSAP
    {
        public string Code { get; set; }
        public string U_VIS_BankID { get; set; }
        public string U_VIS_Deposit { get; set; }
        public string U_VIS_IncomeDate { get; set; }
        public string U_VIS_Banking { get; set; }
        public string U_VIS_ItemDetail { get; set; }
        public string U_VIS_CardCode { get; set; }
        public string U_VIS_DocEntry { get; set; }
        public string U_VIS_DocNum { get; set; }
        public decimal U_VIS_DocTotal { get; set; }
        public decimal U_VIS_Balance { get; set; }
        public decimal U_VIS_AmountCharged { get; set; }
        public decimal U_VIS_NewBalance { get; set; }
        public int U_VIS_Receip { get; set; }
        public string U_VIS_Status { get; set; }
        public string U_VIS_QRStatus { get; set; }
        public string U_VIS_Comments { get; set; }
        public string U_VIS_UserID { get; set; }
        public string U_VIS_SlpCode { get; set; }
        public string U_VIS_CancelReason { get; set; }
        public string U_VIS_DirectDeposit { get; set; }
        public string U_VIS_POSPay { get; set; }
        public string U_VIS_IncomeTime { get; set; }
    }
    public class ReceipResponse
    {
        public string ItemDetail { get; set; }
        public string Code { get; set; }
        public string Receip { get; set; }
        public int Number { get; set; }
        public string BankID { get; set; }
        public string Deposit { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
    public class ListReceipResponse
    {
        public List<ReceipResponse> Collections { get; set; }
    }
    public class PatchReceip
    {
        public string U_VIS_BankID { get; set; }
        public string U_VIS_Receip { get; set; }
        public string U_VIS_Deposit { get; set; }
        public string U_VIS_QRStatus { get; set; }
    }
}
