using AnnaBot.App;
using Discord.WebSocket;

public class Program
{
	private DiscordSocketClient _client;
	private Logger log;

	public static Task Main(string[] args) => new Program().MainAsync();

	public async Task MainAsync()
	{
		_client = new DiscordSocketClient();
		log = new();

		_client.Log += log.Log;

		var token = "OTQxMjY0OTY1OTEyODkxNDAy.GYY4Zh.fAqD5Ws9qXNT-FhHHT5G1LPhj4U7e0FkSqOS2I";


		await _client.LoginAsync(Discord.TokenType.Bot, token);
		await _client.StartAsync();

		await Task.Delay(-1);
	}
}