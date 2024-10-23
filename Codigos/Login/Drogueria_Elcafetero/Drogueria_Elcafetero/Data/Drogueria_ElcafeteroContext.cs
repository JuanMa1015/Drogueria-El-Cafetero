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

        public DbSet<Drogueria_el_cafetero.Models.Customers> customers { get; set; } = default!;
        public DbSet<Drogueria_Elcafetero.Models.Address> address { get; set; }
        public DbSet<Drogueria_Elcafetero.Models.Employees> Employees { get; set; }
    }
}
