using Domain.Todos;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data;

public class ApplicationDbContext : IdentityDbContext<User, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure User entity
        builder.Entity<User>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(u => u.LastName).HasMaxLength(100).IsRequired();
        });

        // Configure TodoItem entity
        builder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).HasMaxLength(200).IsRequired();
            entity.Property(t => t.Description).HasMaxLength(1000);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed Roles
        var adminRoleId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var userRoleId = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-f23456789012");

        builder.Entity<ApplicationRole>().HasData(
            new ApplicationRole
            {
                Id = adminRoleId,
                Name = RoleNames.Admin,
                NormalizedName = RoleNames.Admin.ToUpper(),
                ConcurrencyStamp = adminRoleId.ToString()
            },
            new ApplicationRole
            {
                Id = userRoleId,
                Name = RoleNames.User,
                NormalizedName = RoleNames.User.ToUpper(),
                ConcurrencyStamp = userRoleId.ToString()
            }
        );

        // Seed Users (Note: In production, passwords should be hashed properly)
        var adminUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var regularUserId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        var hasher = new PasswordHasher<User>();

        var adminUser = new User
        {
            Id = adminUserId,
            UserName = "admin@example.com",
            NormalizedUserName = "ADMIN@EXAMPLE.COM",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "User",
            SecurityStamp = Guid.NewGuid().ToString()
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

        var regularUser = new User
        {
            Id = regularUserId,
            UserName = "user@example.com",
            NormalizedUserName = "USER@EXAMPLE.COM",
            Email = "user@example.com",
            NormalizedEmail = "USER@EXAMPLE.COM",
            EmailConfirmed = true,
            FirstName = "John",
            LastName = "Doe",
            SecurityStamp = Guid.NewGuid().ToString()
        };
        regularUser.PasswordHash = hasher.HashPassword(regularUser, "User123!");

        builder.Entity<User>().HasData(adminUser, regularUser);

        // Seed User Roles
        builder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            },
            new IdentityUserRole<Guid>
            {
                RoleId = userRoleId,
                UserId = regularUserId
            }
        );

        // Seed Todo Items
        builder.Entity<TodoItem>().HasData(
            new TodoItem
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                UserId = adminUserId,
                Title = "Complete project documentation",
                Description = "Write comprehensive documentation for the clean architecture project",
                Priority = Priority.High,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                DueDate = DateTime.UtcNow.AddDays(2)
            },
            new TodoItem
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                UserId = adminUserId,
                Title = "Review pull requests",
                Description = "Review and approve pending pull requests",
                Priority = Priority.Medium,
                IsCompleted = true,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                CompletedAt = DateTime.UtcNow.AddDays(-1)
            },
            new TodoItem
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                UserId = regularUserId,
                Title = "Learn Clean Architecture",
                Description = "Study and implement clean architecture patterns",
                Priority = Priority.High,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                DueDate = DateTime.UtcNow.AddDays(7)
            },
            new TodoItem
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                UserId = regularUserId,
                Title = "Buy groceries",
                Description = "Milk, bread, eggs, vegetables",
                Priority = Priority.Low,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                DueDate = DateTime.UtcNow.AddDays(1)
            }
        );
    }
}