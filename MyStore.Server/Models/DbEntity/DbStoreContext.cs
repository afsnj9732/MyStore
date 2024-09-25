using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyStore.Server.Models.DbEntity;

public partial class DbStoreContext : DbContext
{
    public DbStoreContext()
    {
    }

    public DbStoreContext(DbContextOptions<DbStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TCart> TCarts { get; set; }

    public virtual DbSet<TCartItem> TCartItems { get; set; }

    public virtual DbSet<TMember> TMembers { get; set; }

    public virtual DbSet<TOrder> TOrders { get; set; }

    public virtual DbSet<TOrderItem> TOrderItems { get; set; }

    public virtual DbSet<TProduct> TProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TCart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK_Cart");

            entity.ToTable("tCart");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");

            entity.HasOne(d => d.Member).WithMany(p => p.TCarts)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Members");
        });

        modelBuilder.Entity<TCartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId);

            entity.ToTable("tCartItem");

            entity.Property(e => e.CartItemId).HasColumnName("CartItemID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Cart).WithMany(p => p.TCartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tCartItem_tCart");

            entity.HasOne(d => d.Product).WithMany(p => p.TCartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tCartItem_tProducts");
        });

        modelBuilder.Entity<TMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK_Members");

            entity.ToTable("tMembers");

            entity.HasIndex(e => e.Email, "IX_tMembers_Email");

            entity.HasIndex(e => new { e.Email, e.Password }, "IX_tMembers_Email_Password");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.ToTable("tOrder");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");

            entity.HasOne(d => d.Member).WithMany(p => p.TOrders)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tOrder_tMembers");
        });

        modelBuilder.Entity<TOrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId);

            entity.ToTable("tOrderItem");

            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.TOrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tOrderItem_tOrder");

            entity.HasOne(d => d.Product).WithMany(p => p.TOrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tOrderItem_tProducts");
        });

        modelBuilder.Entity<TProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_Products");

            entity.ToTable("tProducts");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
