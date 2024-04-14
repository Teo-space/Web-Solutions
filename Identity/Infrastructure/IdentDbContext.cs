using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



public class IdentDbContext 
	: IdentityDbContext<User, Role, TKey, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{

	public IdentDbContext(DbContextOptions<IdentDbContext> options) : base(options) 
    {
        Database.EnsureCreated();
    }




	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.HasDefaultSchema("Identity");
        TableProperties(modelBuilder);
        TableRelations(modelBuilder);

    }


    protected void TableRelations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            // Each User can have many UserClaims
            b.HasMany(e => e.Claims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            b.HasMany(e => e.Logins)
                .WithOne(e => e.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            b.HasMany(e => e.Tokens)
                .WithOne(e => e.User)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<Role>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // Each Role can have many associated RoleClaims
            b.HasMany(e => e.RoleClaims)
                .WithOne(e => e.Role)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();
        });
    }

    protected void TableProperties(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("Users")
            .HasKey(x => x.Id);

            b.HasIndex(x => x.UserName)
            .IsUnique();

            b.Property(x => x.UserName).HasMaxLength(256);
            b.Property(x => x.NormalizedUserName).HasMaxLength(256);

            b.Property(x => x.Email).HasMaxLength(256);
            b.Property(x => x.NormalizedEmail).HasMaxLength(256);
            b.Property(x => x.EmailConfirmed);

            b.Property(x => x.PasswordHash).HasMaxLength(256);
            b.Property(x => x.SecurityStamp).HasMaxLength(256);
            b.Property(x => x.ConcurrencyStamp).HasMaxLength(256).IsConcurrencyToken();
            b.Property(x => x.PhoneNumber).HasMaxLength(256);
            b.Property(x => x.PhoneNumberConfirmed);

            b.Property(x => x.TwoFactorEnabled);
            b.Property(x => x.LockoutEnd);
            b.Property(x => x.LockoutEnabled);
            b.Property(x => x.AccessFailedCount);
        });

        modelBuilder.Entity<UserClaim>(b =>
        {
            b.ToTable("UserClaims").
            HasKey(x => x.Id);

            b.HasIndex(x => x.UserId);
            b.Property(x => x.ClaimType).HasMaxLength(256);
            b.Property(x => x.ClaimValue).HasMaxLength(256);
        });

        modelBuilder.Entity<UserLogin>(b =>
        {
            b.ToTable("UserLogins");
            b.HasIndex(x => x.UserId);

            b.Property(x => x.LoginProvider).HasMaxLength(256);
            b.Property(x => x.ProviderKey).HasMaxLength(256);
            b.Property(x => x.ProviderDisplayName).HasMaxLength(256);
        });

        modelBuilder.Entity<UserToken>(b =>
        {
            b.ToTable("UserTokens");
            b.HasIndex(x => x.UserId);

            b.Property(x => x.LoginProvider).HasMaxLength(256);
            b.Property(x => x.Name).HasMaxLength(256);
            b.Property(x => x.Value).HasMaxLength(256);
        });

        modelBuilder.Entity<Role>(b =>
        {
            b.ToTable("Roles").HasKey(x => x.Id);
            b.Property(x => x.Name).HasMaxLength(256);
            b.Property(x => x.NormalizedName).HasMaxLength(256);
            b.Property(x => x.ConcurrencyStamp).HasMaxLength(256).IsConcurrencyToken();
        });

        modelBuilder.Entity<RoleClaim>(b =>
        {
            b.ToTable("RoleClaims")
            .HasKey(x => x.Id);
            b.HasIndex(x => x.RoleId);

            b.Property(x => x.ClaimType).HasMaxLength(256);
            b.Property(x => x.ClaimValue).HasMaxLength(256);

        });

        modelBuilder.Entity<UserRole>(b =>
        {
            b.ToTable("UserRoles");
            b.HasIndex(x => x.UserId);
            b.HasIndex(x => x.RoleId);
        });
    }



}
