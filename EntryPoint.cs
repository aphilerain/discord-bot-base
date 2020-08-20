using System;
using Discord;
using System.Linq;
using Discord.Commands;
using System.Threading;
using bot_base.services;
using System.Reflection;
using Discord.WebSocket;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace bot_base 
{
    public class EntryPoint : IServiceProvider
    {
        #region Fields

        public static IServiceProvider? Services;
        public static Stopwatch UpTime = new Stopwatch();

        #endregion
        
        public static void Main(string[] args) => 
            new EntryPoint().MainAsync(args).GetAwaiter().GetResult();
        
        public object? GetService(Type serviceType) =>
            serviceType == typeof(EntryPoint) || serviceType == GetType() ? this : Services?.GetService(serviceType);
        
        public async Task MainAsync(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            
            Services = InitializeServices()
                .AddSingleton(this)
                .AddSingleton<CommandService>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandHandlerService>()
                .BuildServiceProvider();

            DiscordSocketClient discord = Services.GetRequiredService<DiscordSocketClient>();

            await discord.LoginAsync(TokenType.Bot, "TOKEN");
            await discord.StartAsync();

            await Services.GetRequiredService<CommandHandlerService>().LoadCommandsAsync();
            
            UpTime.Start();
            await Task.Delay(Timeout.Infinite);
        }
        
        private static IServiceCollection InitializeServices()
        {
            IEnumerable<Type> types = typeof(EntryPoint).Assembly.GetTypes()
                .Where(type => typeof(EntryPoint).IsAssignableFrom(type) &&
                               !type.GetTypeInfo()
                                   .IsInterface &&
                               !type.GetTypeInfo()
                                   .IsAbstract);

            ServiceCollection services = new ServiceCollection();

            foreach (Type serviceType in types)
                services.AddSingleton(serviceType);

            return services;
        }

        private void UnhandledException(object sender, UnhandledExceptionEventArgs e) => 
            throw new NotImplementedException();

        private void ProcessExit(object? sender, EventArgs e) =>
            throw new NotImplementedException();
    }
}