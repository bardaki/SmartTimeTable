//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinalProject.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class LinkTable
    {
        public int Id { get; set; }
        public Nullable<int> SubjectCode { get; set; }
        public string SubjectDescription { get; set; }
        public Nullable<int> GroupCode { get; set; }
        public Nullable<int> PracticeCode { get; set; }
        public Nullable<int> Semester { get; set; }
    }
}
