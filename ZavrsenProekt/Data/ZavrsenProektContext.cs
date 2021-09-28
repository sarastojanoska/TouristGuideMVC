using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZavrsenProekt.Models;

namespace ZavrsenProekt.Data
{
    public class ZavrsenProektContext : DbContext
    {
        public ZavrsenProektContext (DbContextOptions<ZavrsenProektContext> options)
            : base(options)
        {
        }

        public DbSet<ZavrsenProekt.Models.Poseta> Poseta { get; set; }

        public DbSet<ZavrsenProekt.Models.Turist> Turist { get; set; }

        public DbSet<ZavrsenProekt.Models.TuristickiVodic> TuristickiVodic { get; set; }

        public DbSet<ZavrsenProekt.Models.VkluciSe> VkluciSe { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<VkluciSe>()
                .HasOne<Turist>(p => p.Turist)
                .WithMany(p => p.Poseti)
                .HasForeignKey(p => p.TouristId);
            builder.Entity<VkluciSe>()
                .HasOne<Poseta>(p => p.Poseta)
                .WithMany(p => p.Turisti)
                .HasForeignKey(p => p.PosetaId);
            builder.Entity<Poseta>()
                .HasOne<TuristickiVodic>(p => p.TuristickiVodic)
                .WithMany(p => p.Poseta)
                .HasForeignKey(p => p.TuristickiVodicId);

                
        }
    }
}
