using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Drogueria_el_cafetero.Models;
using Drogueria_Elcafetero.Models;
using System.Runtime.ConstrainedExecution;

namespace Drogueria_Elcafetero.Data
{
    public class Drogueria_ElcafeteroContext : DbContext
    {
        public Drogueria_ElcafeteroContext (DbContextOptions<Drogueria_ElcafeteroContext> options)
            : base(options)
        {
        }
        public DbSet<Drogueria_Elcafetero.Models.car> car { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.details_car> details_car { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.detailsProduct> detailsProduct { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.address> address { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.detailsadressess> detailsadressess { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.Employees> employees { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.city_towns> city_towns { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.VMDepartment_City_towns> department_City_Towns { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.products> products { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.expiredproduct> expiredproduct { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.purchase_orders> purchase_orders { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.detailspurchaseorders> detailspurchaseorders { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.discount> discount { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.sales> sales { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.detailssales> detailssales { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.suppliers> suppliers { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.suppliers_invoices> suppliers_invoices { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.sales_invoices> sales_invoices { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.purchase_orders_details> purchase_orders_details { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.purchase_orders_invoice> purchase_orders_invoice { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.suppliers_products> suppliers_products { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.discount_suppliers_invoices> discount_suppliers_invoices { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.detailsdiscountsupplierinvioces> detailsdiscountsupplierinvioces { get; set; }
        public DbSet<Drogueria_el_cafetero.Models.users> users { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.category> category { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.sales_details> sales_details { get; set; }
    }

}
