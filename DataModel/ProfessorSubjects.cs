//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProfessorSubjects
    {
        public int id { get; set; }
        public Nullable<int> professor_Id { get; set; }
        public Nullable<int> subject_Id { get; set; }
    
        public virtual Professor Professor { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
