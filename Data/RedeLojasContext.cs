using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RedeLojas.Models;

namespace RedeLojas.Data
{
    public class RedeLojasContext : DbContext
    {
        public RedeLojasContext (DbContextOptions<RedeLojasContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Loja> Lojas { get; set; }
        public DbSet<ClienteLoja> ClienteLojas { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClienteLoja>()
                .HasKey(cl => new { cl.ClienteId, cl.LojaId });

            modelBuilder.Entity<ClienteLoja>()
                .HasOne(cl => cl.Cliente)
                .WithMany(c => c.ClienteLojas)
                .HasForeignKey(cl => cl.ClienteId);

            modelBuilder.Entity<ClienteLoja>()
                .HasOne(cl => cl.Loja)
                .WithMany(l => l.ClienteLojas)
                .HasForeignKey(cl => cl.LojaId);
        }
    }
}
