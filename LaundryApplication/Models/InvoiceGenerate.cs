//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LaundryApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvoiceGenerate
    {
        public int Inv_Id { get; set; }
        public Nullable<int> CusIDFk { get; set; }
        public Nullable<int> CatIDFK { get; set; }
        public Nullable<int> PayIDFk { get; set; }
        public string Cat_Des { get; set; }
        public int Item_Quantity { get; set; }
        public string DeliveryStatus { get; set; }
        public string Invoice_Month { get; set; }
        public string Invoice_Date { get; set; }
        public string Barcode { get; set; }
        public Nullable<int> Amount { get; set; }
        public string Pay_Status { get; set; }
        public string Pay_Des { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
