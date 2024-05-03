using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TempMotoWeb.Models;

namespace TempMotoWeb.Data
{
    public class AquaContext : DbContext
    {
        public AquaContext (DbContextOptions<AquaContext> options)
            : base(options)
        {
        }

        public DbSet<Medicao> Medicao { get; set; } = default!;
    }
}
