using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace bot_base.services
{
    public class CommandHandlerService
    {
        #region Fields

        public readonly DiscordSocketClient Discord;
        public readonly CommandService CommandService;
        public readonly IServiceProvider ServiceProvider;

        #endregion
        
        public CommandHandlerService(IServiceProvider serviceProvider)
        {
            Discord = serviceProvider.GetRequiredService<DiscordSocketClient>();
            CommandService = serviceProvider.GetRequiredService<CommandService>();
            ServiceProvider = serviceProvider;
            
            Discord.MessageReceived += MessageReceived;
        }

        public async Task LoadCommandsAsync() =>
            await CommandService.AddModulesAsync(Assembly.GetEntryAssembly(), ServiceProvider);

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message) || message.Source != MessageSource.User) 
                return;
            
            int prefixOffset = 0;
            
            if (!message.HasMentionPrefix(Discord.CurrentUser, ref prefixOffset)) 
                return;
            
            SocketCommandContext context = new SocketCommandContext(Discord, message);
            await CommandService.ExecuteAsync(context, prefixOffset, ServiceProvider);
        }
    }
}