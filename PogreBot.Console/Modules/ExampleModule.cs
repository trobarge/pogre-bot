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

        [Command("muteAll")]
        public async Task MuteAll()
        {
            //retrieve user and the voice channel user is currently connected to
            var user = Context.User;
            var inVoiceChannel = Context.Guild.VoiceChannels.SingleOrDefault(x => x.Users.Contains(user));
            
            //cmd executed out of VC
            if (inVoiceChannel == null)
            {
                await ReplyAsync("Must be in a voice Channel To Execute");
                return;
            }   

            //mute each user in within the voice channel
            foreach(var VCUser in inVoiceChannel.Users)
            {
                await VCUser.ModifyAsync(props => { props.Mute = true; });
            }

            await ReplyAsync($"Muted {inVoiceChannel.Users.Count} users");
        }
    }
}
