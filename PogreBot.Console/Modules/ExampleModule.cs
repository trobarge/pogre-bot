using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class ExampleModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong");
        }

        [Command("say")]
        public async Task Say([Remainder]string sayStr)
        {
            await ReplyAsync(sayStr);
        }

        [Command("add")]
        public async Task Add(params decimal[] numbers)
        {
            var sum = 0M;
            foreach (decimal number in numbers)
                sum += number;
            await ReplyAsync(sum.ToString());
        }
    }
}
