using AnnaBot.App;
using AnnaBot.App.Startup;
using AnnaBot.Application.Services;
using AnnaBot.Domain.Interfaces.Services;
using AnnaBot.Domain.Models.Configurations;
using Microsoft.Extensions.Configuration;

public class Program
{
    private DiscordSocketClient? _client;
    private ILoggingService? log;
    private IConfiguration? config;
    private CommandService? _commandService;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        try
        {
            config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();

            IServiceProvider services = BuildServices();

            var section = config.GetSection(nameof(StartupConfig));
            var startupConfig = section.Get<StartupConfig>();

            var handler = new CommandHandler(_client, _commandService, services, log);
            await handler.SetupAsync();

            await _client.LoginAsync(Discord.TokenType.Bot, token: startupConfig.DiscordToken as string);
            await _client.StartAsync();

            _client.MessageUpdated += async (before, after, channel) => await MessageUpdated(before, after, channel);
            _client.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");
                return Task.CompletedTask;
            };
        }
        catch (Exception e)
        {
            await log.Log(e.Message);
            throw;
        }
        await Task.Delay(-1);
    }

    private IServiceProvider BuildServices()
    {
        _commandService = new CommandService();
        _client = new DiscordSocketClient();
        log = new LoggingService(_client, _commandService);
        var init = new ServiceInitializer(_commandService, _client);
        var services = init.BuildServiceProvider();
        return services;
    }

    private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
    {
        // If the message was not in the cache, downloading it will result in getting a copy of `after`.
        var message = await before.GetOrDownloadAsync();
        Console.WriteLine($"{message} -> {after}");
    }
}