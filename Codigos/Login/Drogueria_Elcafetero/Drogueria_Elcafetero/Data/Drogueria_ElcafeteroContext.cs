using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Drogueria_el_cafetero.Models;
using Drogueria_Elcafetero.Models;

namespace Drogueria_Elcafetero.Data
{
    public class Drogueria_ElcafeteroContext : DbContext
    {
        public Drogueria_ElcafeteroContext (DbContextOptions<Drogueria_ElcafeteroContext> options)
            : base(options)
        {
        }

        public DbSet<Drogueria_el_cafetero.Models.customers> customers { get; set; } = default!;
        public DbSet<Drogueria_Elcafetero.Models.address> address { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.Employees> employees { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.city_towns> city_towns { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.VMDepartment_City_towns> department_City_Towns { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.products> products { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.purchase_orders> purchase_orders { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.suppliers> suppliers { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.discount> discount { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.sales> sales { get; set; }
    }
}
