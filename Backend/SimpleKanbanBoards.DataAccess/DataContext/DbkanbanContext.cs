using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SimpleKanbanBoards.DataAccess.Models;

public partial class DbkanbanContext : DbContext
{
    public DbkanbanContext()
    {
    }

    public DbkanbanContext(DbContextOptions<DbkanbanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Board> Boards { get; set; }

    public virtual DbSet<BoardColumn> BoardColumns { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProject> UserProjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.IdBoard).HasName("board_pkey");

            entity.ToTable("board");

            entity.Property(e => e.IdBoard).HasColumnName("id_board");
            entity.Property(e => e.BoardName)
                .HasMaxLength(160)
                .HasColumnName("board_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.Boards)
                .HasForeignKey(d => d.IdProject)
                .HasConstraintName("board_id_project_fkey");
        });

        modelBuilder.Entity<BoardColumn>(entity =>
        {
            entity.HasKey(e => e.IdBoardColumn).HasName("board_column_pkey");

            entity.ToTable("board_column");

            entity.Property(e => e.IdBoardColumn).HasColumnName("id_board_column");
            entity.Property(e => e.BoardColumnName)
                .HasMaxLength(160)
                .HasColumnName("board_column_name");
            entity.Property(e => e.ColumnPosition).HasColumnName("column_position");
            entity.Property(e => e.IdBoard).HasColumnName("id_board");
            entity.Property(e => e.IsDone)
                .HasDefaultValue(false)
                .HasColumnName("is_done");
            entity.Property(e => e.IsEntry)
                .HasDefaultValue(false)
                .HasColumnName("is_entry");
            entity.Property(e => e.WipLimit)
                .HasDefaultValue(20)
                .HasColumnName("wip_limit");

            entity.HasOne(d => d.IdBoardNavigation).WithMany(p => p.BoardColumns)
                .HasForeignKey(d => d.IdBoard)
                .HasConstraintName("board_column_id_board_fkey");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.IdProject).HasName("project_pkey");

            entity.ToTable("project");

            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.MaxDevs).HasColumnName("max_devs");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Title)
                .HasMaxLength(80)
                .HasColumnName("title");
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(e => e.IdResetToken).HasName("password_reset_tokens_pkey");

            entity.ToTable("password_reset_tokens");

            entity.HasIndex(e => e.IdUser, "password_reset_tokens_id_user_key").IsUnique();

            entity.Property(e => e.IdResetToken).HasColumnName("id_reset_token");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.ResetToken)
                .HasMaxLength(255)
                .HasColumnName("reset_token");
            entity.Property(e => e.ResetTokenExpire).HasColumnName("reset_token_expire");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.IdRefreshToken).HasName("refresh_token_pkey");

            entity.ToTable("refresh_token");

            entity.Property(e => e.IdRefreshToken).HasColumnName("id_refresh_token");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpireAt).HasColumnName("expire_at");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.RefreshToken1).HasColumnName("refresh_token");
            entity.Property(e => e.ReplacedByToken).HasColumnName("replaced_by_token");
            entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("refresh_token_id_user_fkey");

            entity.HasOne(d => d.ReplacedByTokenNavigation).WithMany(p => p.InverseReplacedByTokenNavigation)
                .HasForeignKey(d => d.ReplacedByToken)
                .HasConstraintName("refresh_token_replaced_by_token_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.RolName)
                .HasMaxLength(60)
                .HasColumnName("rol_name");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.IdTask).HasName("task_pkey");

            entity.ToTable("task");

            entity.Property(e => e.IdTask).HasColumnName("id_task");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdBoardColumn).HasColumnName("id_board_column");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.ServiceClass)
                .HasMaxLength(30)
                .HasColumnName("service_class");
            entity.Property(e => e.Title)
                .HasMaxLength(80)
                .HasColumnName("title");

            entity.HasOne(d => d.IdBoardColumnNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.IdBoardColumn)
                .HasConstraintName("task_id_board_column_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("task_id_user_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");
            entity.Property(e => e.Username)
                .HasMaxLength(60)
                .HasColumnName("username");

            entity.HasMany(d => d.IdRols).WithMany(p => p.IdUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("users_roles_id_rol_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("users_roles_id_user_fkey"),
                    j =>
                    {
                        j.HasKey("IdUser", "IdRol").HasName("users_roles_pkey");
                        j.ToTable("users_roles");
                        j.IndexerProperty<int>("IdUser").HasColumnName("id_user");
                        j.IndexerProperty<int>("IdRol").HasColumnName("id_rol");
                    });
        });

        modelBuilder.Entity<UserProject>(entity =>
        {
            entity.HasKey(e => new { e.IdUser, e.IdProject }).HasName("user_project_pkey");

            entity.ToTable("user_project");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.JoinAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("join_at");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.UserProjects)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_project_id_project_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UserProjects)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_project_id_user_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
