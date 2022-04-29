using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiscordBot
{
    public class Startup
    {
        public IConfigurationRoot _configuration { get; }

        public Startup(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json");
            _configuration = builder.Build();
        }

        public async Task RunAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<LoggingService>();
            provider.GetRequiredService<CommandHandlerService>();
            await provider.GetRequiredService<CommandHandlerService>().InstallCommandsAsync();

            string? discordToken;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
                discordToken = Environment.GetEnvironmentVariable("DiscordToken");
            else
                discordToken = Environment.GetEnvironmentVariable("DiscordToken", EnvironmentVariableTarget.User);

            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception("Missing DiscordToken Environment Variable");

            await provider.GetRequiredService<DiscordSocketClient>().LoginAsync(TokenType.Bot, discordToken);
            await provider.GetRequiredService<DiscordSocketClient>().StartAsync();     

            await provider.GetRequiredService<CommandService>().AddModulesAsync(Assembly.GetEntryAssembly(), provider);
            await Task.Delay(-1);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = Discord.LogSeverity.Verbose,
                    MessageCacheSize = 1000
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    LogLevel = Discord.LogSeverity.Verbose,
                    DefaultRunMode = RunMode.Async
                }))
                .AddSingleton<HttpClient>()
                .AddSingleton<CommandHandlerService>()
                .AddSingleton<LoggingService>()
                .AddSingleton(_configuration);
        }
    }
}
