using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace bot_base.commands
{
    public class Echo : CustomModule
    {
        public Echo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Command("echo", RunMode = RunMode.Async)]
        public async Task EchoAsync([Remainder] string args) => 
            await ReplyAsync(args);
    }
}