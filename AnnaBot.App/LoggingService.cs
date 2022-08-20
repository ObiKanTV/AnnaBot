namespace AnnaBot.App
{
    public class LoggingService
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
        public Task Log(string logmessage)
        {
            Console.WriteLine($"{DateTime.UtcNow} | Message: {logmessage}");
            return Task.CompletedTask;
        }
    }
}
