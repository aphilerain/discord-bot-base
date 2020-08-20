using System;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace bot_base.services
{
    public class CustomService
    {
        protected readonly EntryPoint Base;
        protected readonly DiscordSocketClient Discord;

        public CustomService(IServiceProvider serviceProvider)
        {
            Base = serviceProvider.GetRequiredService<EntryPoint>();
            Discord = serviceProvider.GetRequiredService<DiscordSocketClient>();
        }
    }
}