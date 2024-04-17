using Identity.Infrastructure.Initializers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Web.Forums.Infrastructure.Initializers;

var builder = WebApplication.CreateBuilder(args);

{
	builder.Services.AddMiniProfiler();
	builder.Host.AddSerilogLogging();
	builder.Services.AddControllersWithViews();
	builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
	builder.Services.AddFluentValidationWithValidators(Assembly.GetExecutingAssembly());
    {
		var connectionString = builder.Configuration.GetConnectionString("MariaDbConnectionIdentity")
			?? throw new InvalidOperationException("Connection string 'MariaDbConnectionIdentity' not found.");

		builder.AddIdentityWithUseMySql(connectionString);
        builder.AddBasicRolesInitializer();
        builder.AddRootUserInitializer();
    }
    {
		var connectionString = builder.Configuration.GetConnectionString("MariaDbConnectionForum")
			?? throw new InvalidOperationException("Connection string 'MariaDbConnectionForum' not found.");

		builder.AddForumInfrastructure(connectionString);
		builder.AddForumUseCases();
	}
}

var app = builder.Build();
{
	IdentDbContextInitializer.Initialize(app.Services);

	BasicRolesInitializer.Initialize(app.Services);
    RootUserInitializer.Initialize(app.Services);
	ForumRolesInitializer.Initialize(app.Services);
	ForumInitializer.InitializeRoot(app.Services);
}
{
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
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

