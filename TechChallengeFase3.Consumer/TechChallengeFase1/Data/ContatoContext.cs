using Core;
using Microsoft.EntityFrameworkCore;

namespace TechChallengeFase1.Data
{
    public class ContatoContext : DbContext
    {
        public DbSet<Contato> Contatos { get; set; }
        public ContatoContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContatoContext).Assembly);
        }
    }
}
