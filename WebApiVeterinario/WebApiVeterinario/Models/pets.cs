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
    
    public partial class Pets
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pets()
        {
            this.Consulta = new HashSet<Consulta>();
            this.Pet_fotos = new HashSet<Pet_fotos>();
        }
    
        public int Pet_ID { get; set; }
        public string Nome { get; set; }
        public string What_pet { get; set; }
        public string Raca { get; set; }
        public Nullable<decimal> Peso { get; set; }
        public string Tamanho { get; set; }
        public string Descricao { get; set; }
        public string Genero { get; set; }
        public Nullable<int> Idade { get; set; }
        public int Cliente_pessoa_id { get; set; }
        public string Cliente_pessoa_Email { get; set; }
    
        public virtual Cliente_Pessoa Cliente_Pessoa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Consulta> Consulta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pet_fotos> Pet_fotos { get; set; }
    }
}
