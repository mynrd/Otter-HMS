

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Otter.Data
{
    #region Database context

    public partial class HMSContext : Repository.Providers.EntityFramework.DataContext
    {
        private readonly IConfiguration _configuration;
        public HMSContext(IConfiguration configuration, DbContextOptions<HMSContext> options)
            : base(options)
        {
            _configuration = configuration;
            InitializePartial();
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanySite> CompanySites { get; set; }
        public DbSet<CompanySiteRole> CompanySiteRoles { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LocalResource> LocalResources { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<PolicyRole> PolicyRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UsersToken> UsersTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && _configuration != null)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString(@"HMSContext"));
            }
        }

        public bool IsSqlParameterNull(SqlParameter param)
        {
            var sqlValue = param.SqlValue;
            if (sqlValue is INullable nullableValue)
                return nullableValue.IsNull;
            return (sqlValue == null || sqlValue == DBNull.Value);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new CompanySiteConfiguration());
            modelBuilder.ApplyConfiguration(new CompanySiteRoleConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new LocalResourceConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new PolicyConfiguration());
            modelBuilder.ApplyConfiguration(new PolicyRoleConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UsersTokenConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }


        partial void InitializePartial();
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    #endregion

    #region POCO classes

    public class Company : Repository.EntityBase
    {
        public string Code { get; set; }
        public string Name { get; set; }

        // Reverse navigation
        public virtual ICollection<CompanySite> CompanySites { get; set; }

        public Company()
        {
            CompanySites = new List<CompanySite>();
        }
    }

    public class CompanySite : Repository.EntityBase
    {
        public string Code { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }

        // Reverse navigation
        public virtual ICollection<CompanySiteRole> CompanySiteRoles { get; set; }

        public virtual Company Company { get; set; }

        public CompanySite()
        {
            CompanySiteRoles = new List<CompanySiteRole>();
        }
    }

    public class CompanySiteRole : Repository.EntityBase
    {
        public int CompanySiteId { get; set; }
        public int RoleId { get; set; }

        public virtual CompanySite CompanySite { get; set; }
        public virtual Role Role { get; set; }
    }

    public class Language : Repository.EntityBase
    {
        public string Code { get; set; }
        public string Name { get; set; }

        // Reverse navigation
        public virtual ICollection<LocalResource> LocalResources { get; set; }

        public Language()
        {
            LocalResources = new List<LocalResource>();
        }
    }

    public class LocalResource : Repository.EntityBase
    {
        public int LanguageId { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceValue { get; set; }

        public virtual Language Language { get; set; }
    }

    public class Permission : Repository.EntityBase
    {
        public string Code { get; set; }
        public string Name { get; set; }

        // Reverse navigation
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }

        public Permission()
        {
            RolePermissions = new List<RolePermission>();
            UserPermissions = new List<UserPermission>();
        }
    }

    public class Policy : Repository.EntityBase
    {
        public string Code { get; set; }
        public string Name { get; set; }

        // Reverse navigation
        public virtual ICollection<PolicyRole> PolicyRoles { get; set; }

        public Policy()
        {
            PolicyRoles = new List<PolicyRole>();
        }
    }

    public class PolicyRole : Repository.EntityBase
    {
        public int PolicyId { get; set; }
        public int RoleId { get; set; }

        public virtual Policy Policy { get; set; }
        public virtual Role Role { get; set; }
    }

    public class Role : Repository.EntityBase
    {
        public string Code { get; set; }
        public string Name { get; set; }

        // Reverse navigation
        public virtual ICollection<CompanySiteRole> CompanySiteRoles { get; set; }
        public virtual ICollection<PolicyRole> PolicyRoles { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public Role()
        {
            CompanySiteRoles = new List<CompanySiteRole>();
            PolicyRoles = new List<PolicyRole>();
            RolePermissions = new List<RolePermission>();
            UserRoles = new List<UserRole>();
        }
    }

    public class RolePermission : Repository.EntityBase
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }

    public class User : Repository.EntityBase
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool? Locked { get; set; }
        public DateTime? LockedExpire { get; set; }
        public int? LoginFailedCount { get; set; }
        public int? Status { get; set; }

        // Reverse navigation
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UsersToken> UsersTokens { get; set; }

        public User()
        {
            UserPermissions = new List<UserPermission>();
            UserRoles = new List<UserRole>();
            UsersTokens = new List<UsersToken>();
        }
    }

    public class UserPermission : Repository.EntityBase
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual User User { get; set; }
    }

    public class UserRole : Repository.EntityBase
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }

    public class UsersToken : Repository.EntityBase
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime LoggedDate { get; set; }

        public virtual User User { get; set; }

        public UsersToken()
        {
            LoggedDate = DateTime.Now;
        }
    }


    #endregion

    #region POCO Configuration

    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_Companies").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            builder.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(150);
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);
        }
    }

    public class CompanySiteConfiguration : IEntityTypeConfiguration<CompanySite>
    {
        public void Configure(EntityTypeBuilder<CompanySite> builder)
        {
            builder.ToTable("CompanySites", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_CompanySites").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            builder.Property(x => x.CompanyId).HasColumnName(@"CompanyId").HasColumnType("int").IsRequired();
            builder.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(150);
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.Company).WithMany(b => b.CompanySites).HasForeignKey(c => c.CompanyId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CompanySites_Companies");
        }
    }

    public class CompanySiteRoleConfiguration : IEntityTypeConfiguration<CompanySiteRole>
    {
        public void Configure(EntityTypeBuilder<CompanySiteRole> builder)
        {
            builder.ToTable("CompanySiteRoles", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_CompanySiteRoles").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.CompanySiteId).HasColumnName(@"CompanySiteId").HasColumnType("int").IsRequired();
            builder.Property(x => x.RoleId).HasColumnName(@"RoleId").HasColumnType("int").IsRequired();
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.CompanySite).WithMany(b => b.CompanySiteRoles).HasForeignKey(c => c.CompanySiteId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CompanySiteRoles_CompanySites");
            builder.HasOne(a => a.Role).WithMany(b => b.CompanySiteRoles).HasForeignKey(c => c.RoleId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CompanySiteRoles_Roles");
        }
    }

    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("Languages", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_Languages").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            builder.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(250);
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);
        }
    }

    public class LocalResourceConfiguration : IEntityTypeConfiguration<LocalResource>
    {
        public void Configure(EntityTypeBuilder<LocalResource> builder)
        {
            builder.ToTable("LocalResources", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_LocalResources").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.LanguageId).HasColumnName(@"LanguageId").HasColumnType("int").IsRequired();
            builder.Property(x => x.ResourceKey).HasColumnName(@"ResourceKey").HasColumnType("nvarchar").IsRequired().HasMaxLength(250);
            builder.Property(x => x.ResourceValue).HasColumnName(@"ResourceValue").HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.Language).WithMany(b => b.LocalResources).HasForeignKey(c => c.LanguageId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_LocalResources_Languages");
        }
    }

    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_Permissions").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            builder.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(150);
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);
        }
    }

    public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.ToTable("Policies", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_Policies").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            builder.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(150);
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);
        }
    }

    public class PolicyRoleConfiguration : IEntityTypeConfiguration<PolicyRole>
    {
        public void Configure(EntityTypeBuilder<PolicyRole> builder)
        {
            builder.ToTable("PolicyRoles", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_PolicyRoles").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.PolicyId).HasColumnName(@"PolicyId").HasColumnType("int").IsRequired();
            builder.Property(x => x.RoleId).HasColumnName(@"RoleId").HasColumnType("int").IsRequired();
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.Policy).WithMany(b => b.PolicyRoles).HasForeignKey(c => c.PolicyId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PolicyRoles_Policies");
            builder.HasOne(a => a.Role).WithMany(b => b.PolicyRoles).HasForeignKey(c => c.RoleId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PolicyRoles_Roles");
        }
    }

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_Roles").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            builder.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(150);
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);
        }
    }

    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermissions", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_RolePermissions").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.RoleId).HasColumnName(@"RoleId").HasColumnType("int").IsRequired();
            builder.Property(x => x.PermissionId).HasColumnName(@"PermissionId").HasColumnType("int").IsRequired();
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.Permission).WithMany(b => b.RolePermissions).HasForeignKey(c => c.PermissionId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RolePermissions_Permissions");
            builder.HasOne(a => a.Role).WithMany(b => b.RolePermissions).HasForeignKey(c => c.RoleId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RolePermissions_Roles");
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_Users").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Username).HasColumnName(@"Username").HasColumnType("nvarchar").IsRequired().HasMaxLength(150);
            builder.Property(x => x.PasswordHash).HasColumnName(@"PasswordHash").HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(x => x.Email).HasColumnName(@"Email").HasColumnType("nvarchar").IsRequired().HasMaxLength(150);
            builder.Property(x => x.EmailConfirmed).HasColumnName(@"EmailConfirmed").HasColumnType("bit").IsRequired(false);
            builder.Property(x => x.Locked).HasColumnName(@"Locked").HasColumnType("bit").IsRequired(false);
            builder.Property(x => x.LockedExpire).HasColumnName(@"LockedExpire").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LoginFailedCount).HasColumnName(@"LoginFailedCount").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.Status).HasColumnName(@"Status").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);
        }
    }

    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.ToTable("UserPermissions", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_UserPermissions").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("int").IsRequired();
            builder.Property(x => x.PermissionId).HasColumnName(@"PermissionId").HasColumnType("int").IsRequired();
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.Permission).WithMany(b => b.UserPermissions).HasForeignKey(c => c.PermissionId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserPermissions_Permissions");
            builder.HasOne(a => a.User).WithMany(b => b.UserPermissions).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserPermissions_Users");
        }
    }

    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_UserRoles").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("int").IsRequired();
            builder.Property(x => x.RoleId).HasColumnName(@"RoleId").HasColumnType("int").IsRequired();
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.Role).WithMany(b => b.UserRoles).HasForeignKey(c => c.RoleId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserRoles_Roles");
            builder.HasOne(a => a.User).WithMany(b => b.UserRoles).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserRoles_Users");
        }
    }

    public class UsersTokenConfiguration : IEntityTypeConfiguration<UsersToken>
    {
        public void Configure(EntityTypeBuilder<UsersToken> builder)
        {
            builder.ToTable("UsersToken", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_UsersToken").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("int").IsRequired();
            builder.Property(x => x.Token).HasColumnName(@"Token").HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(x => x.LoggedDate).HasColumnName(@"LoggedDate").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastUpdatedBy).HasColumnName(@"LastUpdatedBy").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.User).WithMany(b => b.UsersTokens).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UsersToken_Users");
        }
    }


    #endregion

}

