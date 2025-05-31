using Microsoft.EntityFrameworkCore;
using Red_Social_Voluntarios.Models;

namespace Red_Social_Voluntarios.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<InscripcionEvento> InscripcionesEventos { get; set; }
        public DbSet<Comunidad> Comunidades { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Correo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Contrasena).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(20);

                entity.HasIndex(e => e.Correo).IsUnique();
                entity.HasIndex(e => e.NombreUsuario).IsUnique();
            });

            // Configuración para Evento
            modelBuilder.Entity<Evento>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.Ubicacion).IsRequired().HasMaxLength(300);

                entity.HasOne(e => e.Organizador)
                      .WithMany(u => u.EventosCreados)
                      .HasForeignKey(e => e.OrganizadorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para InscripcionEvento
            modelBuilder.Entity<InscripcionEvento>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.HasOne(i => i.Evento)
                      .WithMany(e => e.Inscripciones)
                      .HasForeignKey(i => i.EventoId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(i => i.Voluntario)
                      .WithMany(u => u.Inscripciones)
                      .HasForeignKey(i => i.VoluntarioId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(i => new { i.EventoId, i.VoluntarioId }).IsUnique();
            });

            // Configuración para Comunidad
            modelBuilder.Entity<Comunidad>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Contenido).IsRequired();

                entity.HasOne(c => c.Usuario)
                      .WithMany(u => u.Publicaciones)
                      .HasForeignKey(c => c.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para Mensaje
            modelBuilder.Entity<Mensaje>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Contenido).IsRequired().HasMaxLength(1000);

                entity.HasOne(m => m.Emisor)
                      .WithMany(u => u.MensajesEnviados)
                      .HasForeignKey(m => m.EmisorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Receptor)
                      .WithMany(u => u.MensajesRecibidos)
                      .HasForeignKey(m => m.ReceptorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Notificacion
            modelBuilder.Entity<Notificacion>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(n => n.Mensaje).IsRequired().HasMaxLength(500);

                entity.HasOne(n => n.Usuario)
                      .WithMany()
                      .HasForeignKey(n => n.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}