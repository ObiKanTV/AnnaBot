
using System.Linq;

namespace AnnaBot.App.Modules
{
    [Group]
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public async Task SayAsync([Remainder][Summary("The text to echo")] string echo) => await ReplyAsync(echo);

        [Command("ping")]
        [Summary("Checks the bot's latency.")]
        public async Task PingAsync()
        {
            var pingTime = Context.Client.Latency;
            await ReplyAsync($"Pong! My current latency is {pingTime}ms.");
        }

        [Command("serverinfo")]
        [Summary("Displays information about the server.")]
        public async Task ServerInfoAsync()
        {
            var server = Context.Guild;
            var memberCount = server.MemberCount;
            var onlineMemberCount = server.Users.Count(x => x.Status != UserStatus.Offline);

            var embed = new EmbedBuilder()
                .WithTitle("Server Information")
                .WithColor(Color.Blue)
                .AddField("Name", server.Name, true)
                .AddField("Owner", server.Owner.Mention, true)
                .AddField("Member Count", memberCount, true)
                .AddField("Online Members", onlineMemberCount, true)
                .WithThumbnailUrl(server.IconUrl)
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed: embed);
        }

        [Command("userinfo")]
        [Summary("Displays information about a user.")]
        public async Task UserInfoAsync(SocketUser user = null)
        {
            var targetUser = (user as SocketGuildUser) ?? Context.User as SocketGuildUser;

            var embed = new EmbedBuilder()
                .WithTitle("User Information")
                .WithColor(Color.Green)
                .WithThumbnailUrl(targetUser.GetAvatarUrl() ?? targetUser.GetDefaultAvatarUrl())
                .AddField("Name", targetUser.Username, true)
                .AddField("Discriminator", targetUser.Discriminator, true)
                .AddField("ID", targetUser.Id, true)
                .AddField("Joined Server", targetUser.JoinedAt?.ToString("dd/MM/yyyy"), true)
                .AddField("Joined Discord", targetUser.CreatedAt.ToString("dd/MM/yyyy"), true)
                .Build();

            await ReplyAsync(embed: embed);
        }
        [Command("botinfo")]
        [Summary("Displays information about the bot.")]
        public async Task BotInfoAsync()
        {
            var botUser = Context.Client.CurrentUser;

            var embed = new EmbedBuilder()
                .WithTitle("Bot Information")
                .WithColor(Color.Magenta)
                .AddField("Name", botUser.Username, true)
                .AddField("Version", "1.0", true)
                .AddField("Creator", "Your Name", true)
                .WithThumbnailUrl(botUser.GetAvatarUrl() ?? botUser.GetDefaultAvatarUrl())
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed: embed);
        }
        [Command("roleinfo")]
        [Summary("Displays information about a role.")]
        public async Task RoleInfoAsync(IRole role)
        {
            var server = Context.Guild;

            var memberCount = server.Users.Count(x => x.Roles.Contains(role));

            var embed = new EmbedBuilder()
                .WithTitle("Role Information")
                .WithColor(role.Color)
                .AddField("Name", role.Name, true)
                .AddField("ID", role.Id, true)
                .AddField("Color", role.Color.ToString(), true)
                .AddField("Permissions", string.Join(", ", role.Permissions.ToList()), true)
                .AddField("Member Count", memberCount, true)
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed: embed);
        }



        [Command("channelinfo")]
        [Summary("Displays information about a channel.")]
        public async Task ChannelInfoAsync(SocketGuildChannel channel)
        {
            var embed = new EmbedBuilder()
                .WithTitle("Channel Information")
                .WithColor(Color.Teal)
                .AddField("Name", channel.Name, true)
                .AddField("ID", channel.Id, true)
                .AddField("Type", channel.GetType().Name, true)
                .AddField("Topic", (channel as SocketTextChannel)?.Topic ?? "-", true)
                .AddField("Creation Date", channel.CreatedAt.ToString("dd/MM/yyyy"), true)
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed: embed);
        }
        [Command("serverroles")]
        [Summary("Lists all the roles in the server.")]
        public async Task ServerRolesAsync()
        {
            var server = Context.Guild;

            var roles = server.Roles
                .OrderByDescending(r => r.Position)
                .Select(async r =>
                {
                    var members = await server.GetUsersAsync().FlattenAsync();
                    var memberCount = members.Count(m => m.RoleIds.Contains(r.Id));
                    return $"{r.Name} ({r.Id}) - {memberCount} members";
                });

            var roleResults = await Task.WhenAll(roles);
            var rolesList = string.Join("\n", roleResults);

            var embed = new EmbedBuilder()
                .WithTitle("Server Roles")
                .WithColor(Color.Gold)
                .WithDescription(rolesList)
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed: embed);
        }



        [Command("serverchannels")]
        [Summary("Lists all the channels in the server.")]
        public async Task ServerChannelsAsync()
        {
            var server = Context.Guild;

            var channels = server.Channels
                .Where(c => c is SocketTextChannel) // Filter only text channels
                .OrderBy(c => c.Position)
                .Select(c => $"{c.Name} ({c.Id}) - Text Channel");

            var channelsList = string.Join("\n", channels);

            var embed = new EmbedBuilder()
                .WithTitle("Server Channels")
                .WithColor(Color.Orange)
                .WithDescription(channelsList)
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed: embed);
        }

        [Command("botpermissions")]
        [Summary("Displays the bot's permissions.")]
        public async Task BotPermissionsAsync()
        {
            var botUser = Context.Client.CurrentUser;
            var server = Context.Guild;

            var botPermissions = server.CurrentUser.GuildPermissions.ToList();

            var embed = new EmbedBuilder()
                .WithTitle("Bot Permissions")
                .WithColor(Color.Purple)
                .WithDescription(string.Join(", ", botPermissions))
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed: embed);
        }

        [Command("userpermissions")]
        [Summary("Displays the permissions of a user.")]
        public async Task UserPermissionsAsync(SocketUser user = null)
        {
            var targetUser = (user as SocketGuildUser) ?? Context.User as SocketGuildUser;

            var targetMember = Context.Guild.GetUser(targetUser.Id);

            var userPermissions = targetMember.GuildPermissions.ToList();

            var embed = new EmbedBuilder()
                .WithTitle("User Permissions")
                .WithColor(Color.Green)
                .WithDescription(string.Join(", ", userPermissions))
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed: embed);
        }
    }
}
