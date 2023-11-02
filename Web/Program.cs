using FluentAssertions.Common;
using FluentValidation.AspNetCore;
using Identity.BasicRoles;
using Identity.RootUser;
using StackExchange.Profiling.Storage;
using Web.Forums.Infrastructure.Initializers;

var builder = WebApplication.CreateBuilder(args);

{
	// Add services to the container.
	//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
	//builder.Services.AddDbContext<ApplicationDbContext>(options =>
	//    options.UseSqlServer(connectionString));
	//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

	//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	//    .AddEntityFrameworkStores<ApplicationDbContext>();
	{
		builder.Services.AddLogging();
	}
	{
		builder.Services.AddMiniProfiler();
	}
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }
	{
		builder.Services.AddFluentValidationAutoValidation();
		builder.Services.AddFluentValidationClientsideAdapters();
		builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
	}
    {
        builder.AddIdentityWithUseSqlite();
        builder.AddBasicRolesInitializer();
        builder.AddRootUserInitializer();
        //builder.AddAuthenticationJWTBearer();
    }
    {
        builder.AddForumInfrastructure();
		builder.AddForumUseCases();

	}

    builder.Services.AddControllersWithViews();
}

var app = builder.Build();
{
    BasicRolesInitializer.Initialize(app.Services);
    RootUserInitializer.Initialize(app.Services);

	ForumRolesInitializer.Initialize(app.Services);
	ForumInitializer.InitializeRoot(app.Services);
	
}
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();
	app.UseMiniProfiler();

	app.MapControllerRoute(name: "default", pattern: "{controller=Forums}/{action=Index}");
    app.MapRazorPages();

    app.Run();
}

