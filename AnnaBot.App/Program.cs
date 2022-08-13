using AnnaBot.App;
using AnnaBot.Core.Models.Configurations;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    private DiscordSocketClient? _client;
    private LoggingService? log;
    private IConfiguration? config;
    private CommandService? _commandService;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .Build();

        _commandService = new CommandService();
        _client = new DiscordSocketClient();
        log = new(_client, _commandService);

        var services = CreateServices();
      
        var section = config.GetSection(nameof(StartupConfig));
        var startupConfig = section.Get<StartupConfig>();

        var handler = new CommandHandler(_client, _commandService, services,log);
        await handler.SetupAsync();

        await _client.LoginAsync(Discord.TokenType.Bot, token: startupConfig.DiscordToken as string);
        await _client.StartAsync();

        _client.MessageUpdated += MessageUpdated;
        _client.Ready += () =>
        {
            Console.WriteLine("Bot is connected!");
            return Task.CompletedTask;
        };

        await Task.Delay(-1);
    }
    private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
    {
        // If the message was not in the cache, downloading it will result in getting a copy of `after`.
        var message = await before.GetOrDownloadAsync();
        Console.WriteLine($"{message} -> {after}");
    }
    static IServiceProvider CreateServices()
    {
        var collection = new ServiceCollection()
            /*.AddSingleton()*/;

        return collection.BuildServiceProvider();
    }
}