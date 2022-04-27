using Discord;
using Discord.WebSocket;
using DiscordBot;

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync(args);

    public async Task MainAsync(string[] args)
    {
        var startup = new Startup(args);
        await startup.RunAsync();
    }
}