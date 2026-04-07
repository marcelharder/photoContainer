namespace photoContainer.helpers;

public static class AppSettings
{
    private static IConfiguration? _configuration;

    public static string ConnectionString { get; private set; } = string.Empty;
    public static string VideoUrl { get; private set; } = string.Empty;



    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        ConnectionString = _configuration.GetConnectionString("SQLConnection")
            ?? throw new InvalidOperationException("Connection string 'SQLConnection' not found.");


    }

    public static string GetConnectionString()
    {
        if (string.IsNullOrWhiteSpace(ConnectionString))
            throw new InvalidOperationException("AppSettings has not been initialized.");

        return ConnectionString;
    }
}