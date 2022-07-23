using AnnaBot.App;
using AnnaBot.Core.Models.Configurations;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

public class Program
{
	private DiscordSocketClient _client;
	private Logger log;
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
		log = new();

		_client.Log += log.Log;


		//This needs to be retrieved from somewhere else. Client Secret or Decrypted somehow.
		var section = config.GetSection(nameof(StartupConfig));
		var startupConfig = section.Get<StartupConfig>();
		var token = startupConfig.DiscordToken;


		await _client.LoginAsync(Discord.TokenType.Bot, token);
		await _client.StartAsync();

		await Task.Delay(-1);
	}
}