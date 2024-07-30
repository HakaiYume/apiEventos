using System;
using System.Collections.Generic;
using ApiEventos.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEventos.Data;

public partial class DwiApieventosContext : DbContext
{
    public DwiApieventosContext()
    {
    }

    public DwiApieventosContext(DbContextOptions<DwiApieventosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Invitacione> Invitaciones { get; set; }

    public virtual DbSet<InvitadosEspeciale> InvitadosEspeciales { get; set; }

    public virtual DbSet<Lugare> Lugares { get; set; }

    public virtual DbSet<RegistroEvento> RegistroEventos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=BDConection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.IdEvento).HasName("PK__Eventos__034EFC041CF7B15D");

            entity.Property(e => e.CostoEvento).HasColumnType("decimal(9, 2)");
            entity.Property(e => e.DescripcionEvento)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EstadoEvento)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.NombreEvento)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdLugarEventoNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdLugarEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Eventos__IdLugar__6477ECF3");

            entity.HasOne(d => d.UsuarioRegistroNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.UsuarioRegistro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Eventos_Usuarios");
        });

        modelBuilder.Entity<Invitacione>(entity =>
        {
            entity.HasKey(e => e.IdInvitacion).HasName("PK__Invitaci__4EEDB6457A06B804");

            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Invitaciones)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invitacio__IdEve__693CA210");

            entity.HasOne(d => d.IdInvitadoNavigation).WithMany(p => p.Invitaciones)
                .HasForeignKey(d => d.IdInvitado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invitacio__IdInv__6A30C649");

            entity.HasOne(d => d.UsuarioRegistroNavigation).WithMany(p => p.Invitaciones)
                .HasForeignKey(d => d.UsuarioRegistro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invitaciones_Usuarios");
        });

        modelBuilder.Entity<InvitadosEspeciale>(entity =>
        {
            entity.HasKey(e => e.IdInvitado).HasName("PK__Invitado__E0064BB2C8CBF7C2");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.UsuarioRegistroNavigation).WithMany(p => p.InvitadosEspeciales)
                .HasForeignKey(d => d.UsuarioRegistro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvitadosEspeciales_Usuarios");
        });

        modelBuilder.Entity<Lugare>(entity =>
        {
            entity.HasKey(e => e.IdLugarEvento).HasName("PK__Lugares__4FB9E106B58D1A4D");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Latitud).HasColumnType("decimal(9, 5)");
            entity.Property(e => e.Longitud).HasColumnType("decimal(9, 5)");
        });

        modelBuilder.Entity<RegistroEvento>(entity =>
        {
            entity.HasKey(e => e.IdRegistro).HasName("PK__Registro__FFA45A995BC441BB");

            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.RegistroEventos)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RegistroE__IdEve__6EF57B66");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RegistroEventos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RegistroE__IdUsu__6FE99F9F");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF970960749B");

            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
