using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnaBot.App.Startup
{
	public class ServiceInitializer
	{
		private readonly CommandService _commands;
		private readonly DiscordSocketClient _client;

		public ServiceInitializer(CommandService commands = null, DiscordSocketClient client = null)
		{
			_commands = commands ?? new CommandService();
			_client = client ?? new DiscordSocketClient();
		}

		public IServiceProvider BuildServiceProvider() => new ServiceCollection()
			.AddSingleton(_client)
			.AddSingleton(_commands)
			.AddSingleton<CommandHandler>()
			.BuildServiceProvider();
	}
}
