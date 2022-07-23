using System.Reflection;

namespace AnnaBot.App
{
    public class CommandHandler
    {
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _client;
        private readonly LoggingService _log; 

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services, LoggingService log)
        {
            _commands = commands;
            _services = services;
            _client = client;
            _log = log;
        }

        public async Task SetupAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            
            _commands.CommandExecuted += OnCommandExecutedAsync;
            
            _client.MessageReceived += HandleCommandAsync;
        }
        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

          
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: _services);
        }
        public async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // We can tell the user what went wrong
            if (!string.IsNullOrEmpty(result?.ErrorReason))
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }

           
            var commandName = command.IsSpecified ? command.Value.Name : "A command";
            await _log.LogAsync(new LogMessage(LogSeverity.Info,
                "CommandExecution",
                $"{commandName} was executed at {DateTime.UtcNow}."));
        }
        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message is null) return;

            int argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
            message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services
                );

        }

    }
}
