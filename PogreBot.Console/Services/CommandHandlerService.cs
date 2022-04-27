using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class CommandHandlerService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly IConfigurationRoot _config;

        public CommandHandlerService(DiscordSocketClient client, CommandService commands, IServiceProvider services, IConfigurationRoot config)
        {
            _client = client;
            _commands = commands;
            _services = services;
            _config = config;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            var configPrefix = Convert.ToChar(_config["prefix"]);
            if (!(message.HasCharPrefix(configPrefix, ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }
    }
}
