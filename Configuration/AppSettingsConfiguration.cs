public class AppSettingsConfiguration
{
    public string ApplicationName { get; set; }
    public string Version { get; set; }
    public string Port { get; set; }
    public string Url { get; set; }
}

public class Logging
{
    public Console Console { get; set; }
    public string LogLevel { get; set; }
}

public class Console
{
    public bool Log { get; set; }
}
