public static class DependencyInjection__Identity
{
	public static void AddIdentityWithUseSqlServer(this WebApplicationBuilder builder, string connectionString)
	{
		builder.Services.AddDbContext<IdentDbContext>(options => options.UseSqlServer(connectionString));
		AddDefaultIdentity(builder);
	}


	public static void AddIdentityWithUseSqlite(this WebApplicationBuilder builder,
		string connectionString = "Filename=Data/Identity.DbContext.db")
	{
		builder.Services.AddDbContext<IdentDbContext>(options => options.UseSqlite(connectionString));
		AddDefaultIdentity(builder);
	}

	public static void AddIdentityWithUseInMemory(this WebApplicationBuilder builder,
		string connectionString = "Identity.DbContext")
	{
		builder.Services.AddDbContext<IdentDbContext>(options => options.UseInMemoryDatabase(connectionString));
		AddDefaultIdentity(builder);
	}


	private static void AddDefaultIdentity(WebApplicationBuilder builder)
	{
		//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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