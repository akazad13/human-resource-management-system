using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Common;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, long, IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<WorkHistory> WorkHistories { get; set; }
        public DbSet<LeavePolicy> LeavePolicys { get; set; }
        public DbSet<EmployeeLeavePolicy> EmployeeLeavePolicys { get; set; }
        public DbSet<LeaveHistory> LeaveHistory { get; set; }
        public DbSet<LeaveStatus> LeaveStatus { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(u =>
            {
                u.ToTable(name: "Users");
                u.Property(x => x.UserName).IsRequired().HasMaxLength(20);
                u.Property(x => x.NormalizedUserName).HasMaxLength(20);
                u.Property(x => x.PhoneNumber).HasMaxLength(20);
                u.Property(x => x.Email).IsRequired().HasMaxLength(50);
                u.Property(x => x.NormalizedEmail).HasMaxLength(50);
            });

            builder.Entity<Role>(r =>
            {
                r.ToTable(name: "Roles");
                r.Property(x => x.Name).HasMaxLength(20);
                r.Property(x => x.NormalizedName).HasMaxLength(20);
            });


            builder.Entity<UserRole>(userRole =>
            {
                userRole.ToTable(name: "UserRoles");
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });


            builder.Entity<IdentityUserClaim<long>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<long>>(entity =>
            {
                entity.ToTable("UserLogins");
                entity.HasIndex(x => new { x.ProviderKey, x.LoginProvider });
            });

            builder.Entity<IdentityRoleClaim<long>>(entity =>
            {
                entity.ToTable("RoleClaims");

            });

            builder.Entity<IdentityUserToken<long>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            builder.Entity<LeavePolicy>(lp =>
            {
                lp.Property(x => x.Name).HasMaxLength(20);
            });
            builder.Entity<EmployeeLeavePolicy>(ulp =>
            {
                ulp.HasKey(ur => new { ur.EmployeeId, ur.LeavePolicyId });
                ulp.HasOne(ur => ur.LeavePolicy)
                    .WithMany(r => r.EmployeeLeavePolicies)
                    .HasForeignKey(ur => ur.LeavePolicyId)
                    .IsRequired();
                ulp.HasOne(ur => ur.Employee)
                    .WithMany(u => u.EmployeeLeavePolicies)
                    .HasForeignKey(ur => ur.EmployeeId)
                    .IsRequired();
            });

            builder.Entity<WorkHistory>(wh =>
            {
                wh.HasOne(wh => wh.Employee)
                    .WithMany(e => e.EmployeeWorkHistories)
                    .HasForeignKey(wh => wh.EmployeeId)
                    .IsRequired();
                wh.HasOne(wh => wh.Manager)
                   .WithMany(e => e.EmployeeManagerHistories)
                   .HasForeignKey(wh => wh.ManagerId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
