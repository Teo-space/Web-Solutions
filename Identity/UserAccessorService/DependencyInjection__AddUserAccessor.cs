public static class DependencyInjection__AddUserAccessor
{
    public static IServiceCollection AddUserAccessor(this IServiceCollection services)
        => services.AddTransient<IUserAccessor, UserAccessor>();
}

