﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AffirmMedicalMainPortal
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class affirmmedicalweightlossEntities : DbContext
    {
        public affirmmedicalweightlossEntities()
            : base("name=affirmmedicalweightlossEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<account> accounts { get; set; }
        public DbSet<company> companies { get; set; }
        public DbSet<customer> customers { get; set; }
    }
}
