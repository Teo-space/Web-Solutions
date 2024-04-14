using FluentValidation.AspNetCore;
using Identity.Infrastructure.Initializers;
using Microsoft.AspNetCore.Builder;
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
        builder.AddIdentityWithUseSqlite();
        builder.AddBasicRolesInitializer();
        builder.AddRootUserInitializer();
    }
    {
        builder.AddForumInfrastructure();
		builder.AddForumUseCases();
	}
}

var app = builder.Build();
{
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

