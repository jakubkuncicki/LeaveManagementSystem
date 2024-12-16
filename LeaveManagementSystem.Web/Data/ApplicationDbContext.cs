using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "2a4cb174-c3fe-4a25-be05-387b93c2de03",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },
                new IdentityRole
                {
                    Id = "990f4fe3-e2a5-44eb-8585-9b176fd92b7c",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR"
                },
                new IdentityRole
                {
                    Id = "31ba5abb-b764-49b4-8c90-b9969f7a5eda",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
            );

            var hasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "6c3fb309-1468-4791-af73-fb73e3efd3a6",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    UserName = "admin@localhost.com",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                    EmailConfirmed = true,
                    FirstName = "Deafault",
                    LastName = "Admin",
                    DateOfBirth = new DateOnly(1980, 12, 19)
                }
            );

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "31ba5abb-b764-49b4-8c90-b9969f7a5eda",
                    UserId = "6c3fb309-1468-4791-af73-fb73e3efd3a6"
                }
            );
        }

        public DbSet<LeaveType> LeaveTypes { get; set; }
    }
}
