using Microsoft.AspNetCore.Builder;


public static class DependencyInjectionIdentity
{
    public static void AddIdentity(this WebApplicationBuilder builder, string connectionString)
    {
        builder.Services.AddDbContext<IdentDbContext>(options =>
            //options.UseSqlServer(connectionString));
            //options.UseInMemoryDatabase("Identity.DbContext"));
            options.UseSqlite("Filename=Identity.DbContext.db"));


        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<User>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            // User settings.
            options.User.RequireUniqueEmail = true;

        })
        .AddDefaultTokenProviders()
        .AddDefaultUI()
        .AddRoles<Role>()
        .AddEntityFrameworkStores<IdentDbContext>();

    }


}