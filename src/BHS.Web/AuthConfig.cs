namespace BHS.Web;

internal static class AuthConfig
{
    public const string BlogWriteAccess = "BlogWriteAccess";
    public const string MuseumWriteAccess = "MuseumWriteAccess";
    public const string BannerWriteAccess = "BannerWriteAccess";
    public const string LeadershipWriteAccess = "LeadershipWriteAccess";

    public static IServiceCollection AddBhsAuth(this IServiceCollection services)
    {
        // Set up authentication and read from config section named "Authentication".
        services.AddAuthentication()
                .AddJwtBearer();

        // Set up claim-based authorization.
        services.AddAuthorization(opt =>
        {
            const string permissions = "permissions";
            opt.AddPolicy(BlogWriteAccess, policy => policy.RequireClaim(permissions, "write:blog"));
            opt.AddPolicy(MuseumWriteAccess, policy => policy.RequireClaim(permissions, "write:museum"));
            opt.AddPolicy(BannerWriteAccess, policy => policy.RequireClaim(permissions, "write:banners"));
            opt.AddPolicy(LeadershipWriteAccess, policy => policy.RequireClaim(permissions, "write:leadership"));
        });

        return services;
    }
}
