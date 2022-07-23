using AnnaBot.App;
using AnnaBot.Core.Models.Configurations;
using Microsoft.Extensions.Configuration;

public class Program
{
    private DiscordSocketClient? _client;
    private LoggingService? log;
    private IConfiguration? config;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .Build();

        _client = new DiscordSocketClient();
        log = new(_client, new CommandService());

        var section = config.GetSection(nameof(StartupConfig));
        var startupConfig = section.Get<StartupConfig>();
        var token = startupConfig.DiscordToken;


        await _client.LoginAsync(Discord.TokenType.Bot, token);
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
}