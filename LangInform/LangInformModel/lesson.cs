//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LangInformModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class lesson
    {
        public lesson()
        {
            this.scenes = new HashSet<scene>();
            this.vocabularies = new HashSet<vocabulary>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IdUnit { get; set; }
    
        public virtual unit unit { get; set; }
        public virtual ICollection<scene> scenes { get; set; }
        public virtual ICollection<vocabulary> vocabularies { get; set; }
    }
}