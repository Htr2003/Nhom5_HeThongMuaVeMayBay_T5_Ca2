//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoAnCNPMNC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SanBay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanBay()
        {
            this.ChuyenBay = new HashSet<ChuyenBay>();
            this.ChuyenBay1 = new HashSet<ChuyenBay>();
        }
    
        public string SanBayID { get; set; }
        public string TenSanBay { get; set; }
        public string ThanhPho { get; set; }
        public string DatNuoc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChuyenBay> ChuyenBay { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChuyenBay> ChuyenBay1 { get; set; }
    }
}
