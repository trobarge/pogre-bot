using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class LoggingService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public LoggingService(DiscordSocketClient client, CommandService commands)
        {
            _client = client;
            _commands = commands;

            _client.Log += OnLogAsync;
            _commands.Log += OnLogAsync;
        }

        private Task OnLogAsync(LogMessage msg)
        {
            string logText = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";

            return Console.Out.WriteLineAsync(logText);
        }
    }
}
