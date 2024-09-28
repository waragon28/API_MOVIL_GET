using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SAP_Core.BO
{
    public class DepositBO
    {        
        public string Code { get; set; }
        public string U_VIS_BankID { get; set; }
        //public string BankName { get; set; }
        public string U_VIS_IncomeType { get; set; }
        public string U_VIS_Deposit { get; set; }
        public string U_VIS_Date { get; set; }
        public string U_VIS_DeferredDate { get; set; }
        public string U_VIS_Banking { get; set; }
        public string U_VIS_UserID { get; set; }
        public string U_VIS_SlpCode { get; set; }
        public decimal U_VIS_AmountDeposit { get; set; }
        public string U_VIS_Status { get; set; }
        public string U_VIS_Comments { get; set; }
        public string U_VIS_CancelReason { get; set; }
        public string U_VIS_DirectDeposit { get; set; }
        public string U_VIS_POSPay { get; set; }
    }
    public class ListDeposit
    {
        public List<DepositBO2> Deposits { get; set; }
    }
    public class DepositBO2
    {
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
        public decimal U_VIS_AmountDeposit { get; set; }
        public string U_VIS_Status { get; set; }
        public string U_VIS_Comments { get; set; }
        public string U_VIS_CancelReason { get; set; }
        public string U_VIS_DirectDeposit { get; set; }
        public string U_VIS_POSPay { get; set; }
    }
    public class Deposit
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
    }
    public class DepositResponse
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Deposit { get; set; }
        public string Code { get; set; }
    }
    public class ListDepositResponse
    {
        public List<DepositResponse> Deposits { get; set; }
    }
    public class PatchDeposit
    {
        public string Code { get; set; }
        public string U_VIS_BankID { get; set; }
        public string U_VIS_Deposit { get; set; }
        public string U_VIS_Status { get; set; }
    }
}
