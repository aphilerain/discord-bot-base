using System;
using bot_base.services;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace bot_base.commands
{
    public abstract class CustomModule<T> : CustomModule where T : CustomService
    {
        protected readonly T Service;

        public CustomModule(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Service = serviceProvider.GetRequiredService<T>();
        }
    }

    public class CustomModule : ModuleBase<SocketCommandContext>
    {
        protected readonly EntryPoint Base;
        protected readonly DiscordSocketClient Discord;

        public CustomModule(IServiceProvider serviceProvider)
        {
            Base = serviceProvider.GetRequiredService<EntryPoint>();
            Discord = serviceProvider.GetRequiredService<DiscordSocketClient>();
        }
    }
}