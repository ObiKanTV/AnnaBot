namespace AnnaBot.Domain.Interfaces.Services;

public interface ILoggingService
{
    Task Log(string message);
    Task Log(Exception exception);
    Task Log(Exception exception, string message);
    Task LogAsync(LogMessage logMessage);
}
