﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ecommerceEntities : DbContext
    {
        public ecommerceEntities()
            : base("name=ecommerceEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tbl_admin> tbl_admin { get; set; }
        public virtual DbSet<tbl_category> tbl_category { get; set; }
        public virtual DbSet<tbl_product> tbl_product { get; set; }
        public virtual DbSet<tbl_user> tbl_user { get; set; }
        public virtual DbSet<tbl_invoice> tbl_invoice { get; set; }
        public virtual DbSet<tbl_order> tbl_order { get; set; }
    }
}
