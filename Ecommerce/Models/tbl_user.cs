//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ecommerce.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_user()
        {
            this.tbl_product = new HashSet<tbl_product>();
            this.tbl_invoice = new HashSet<tbl_invoice>();
        }
    
        public int u_id { get; set; }
        public string u_name { get; set; }
        public string u_email { get; set; }
        public string u_password { get; set; }
        public string u_image { get; set; }
        public string u_contact { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_product> tbl_product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_invoice> tbl_invoice { get; set; }
    }
}
