//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApiVeterinario.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class payment
    {
        public int id_payment { get; set; }
        public string card_number { get; set; }
        public string card_owner { get; set; }
        public Nullable<System.DateTime> expire_date { get; set; }
        public string three_digits { get; set; }
        public int usuario_id { get; set; }
        public string usuario_cpf_cnpj { get; set; }
    
        public virtual usuario usuario { get; set; }
    }
}
