using AnnaBot.Domain.Interfaces.Services;

namespace AnnaBot.Application.Services;

public class LoggingService : ILoggingService
{
    public LoggingService(DiscordSocketClient client, CommandService command)
    {
        client.Log += LogAsync;
        command.Log += LogAsync;

    }
    public async Task LogAsync(LogMessage logMessage)
    {
        if (logMessage.Exception is CommandException cmdException)
        {
            // We can tell the user that something unexpected has happened
            await cmdException.Context.Channel.SendMessageAsync("Something went catastrophically wrong!");

            // We can also log this incident
            Console.WriteLine($"{cmdException.Context.User} failed to execute '{cmdException.Command.Name}' in {cmdException.Context.Channel}.");
            Console.WriteLine(cmdException.ToString());
        }
        else
        {
            Console.WriteLine(logMessage.ToString());
        }
    }
    public Task Log(string message)
    {
        Console.WriteLine($"{DateTime.UtcNow} | Message: {message}");
        return Task.CompletedTask;
    }
    public Task Log(Exception exception)
    {
        Console.WriteLine($"{DateTime.UtcNow} | Message: {exception.Message} | Exception: {nameof(exception)}" +
            $" | Trace: {exception.StackTrace.ToString()} | Inner Exception: {exception.InnerException}" );
        return Task.CompletedTask;
    }

    public Task Log(Exception exception, string message)
    {
        Console.WriteLine($"{DateTime.UtcNow} | Message: {message} | Exception: {nameof(exception)}" +
            $" | Trace: {exception.StackTrace.ToString()} | Inner Exception: {exception.InnerException}");
        return Task.CompletedTask;
    }
}

