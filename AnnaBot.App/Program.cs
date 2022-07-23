using Discord.WebSocket;

public class Program
{
	private DiscordSocketClient _client;

	public static Task Main(string[] args) => new Program().MainAsync();

	public async Task MainAsync()
	{
		_client = new DiscordSocketClient();

		_client.Log += Log;

	}
}