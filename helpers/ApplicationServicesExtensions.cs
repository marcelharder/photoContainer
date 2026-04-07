
using photoContainer.data.implementations;

namespace photoContainer.helpers;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var serverVersion = new MariaDbServerVersion(new Version(8, 0, 34));
        var connectionString = AppSettings.GetConnectionString();
        services.AddDbContext<ApplicationDbContext>(dbContextOptions =>
            dbContextOptions
                .UseMySql(
                    connectionString,
                    serverVersion,
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure()
                )
                .LogTo(Console.WriteLine, LogLevel.Information)
        );
        services.AddHttpContextAccessor();
        services.AddSingleton<DapperContext>();
        services.AddScoped<ComSettings>();
        services.AddScoped<IImage, ImageImplementation>();
        services.AddScoped<ICategory, CategoryImplementation>();
      
        services.AddAutoMapper(cfg =>
        {
            cfg.LicenseKey =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzk5NDUyODAwIiwiaWF0IjoiMTc2Nzk1NjQ2OSIsImFjY291bnRfaWQiOiIwMTliYTI2NjdkNjM3MzQ1OGY1NDg1N2FkNGJkZjBjZiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2VoNm5ueTVnbmdraHhoNmJuMHR2ZHFhIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.rliBrwTYH5PbjykFpQ4_sXBYMzTb0TvfmaHjPuGJJT6vQfwmEMpdYSqI9jKcd5EaJdkJ8EiFDOIIufPktE8_QA5Qkv9FLH0qulqVO6lv4JiAnh8fnj5YmzPfGhR27Y2gxh-H-k0Xr5JwaQl9GytE0ooav20eo39BEqJTMWGJoY-aAlkxzuVNoHqzOCdhpcU_gZYumHCqtnXBz5VmoyWc_6FRIFBblNsExGoyGXAwf_6f05kNPWJesMuYc1jtIT99Gmsc_rltyImMBpfpsOvYb1rnataiF-vBGlPhmyv-1WKkeHqr7fHVex1XmvCKNUIe-JwdfdJ2bOwitBFYWsgLJw";
            cfg.AddProfile<AutoMapperProfiles>();
        });
        return services;
    }
}
