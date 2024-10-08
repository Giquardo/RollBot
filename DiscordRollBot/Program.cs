﻿using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
using Rollbot.Config;
using RollBot.Commands;
using RollBot.Services;

namespace RollBot;

public sealed class Program
{
    public static DiscordClient Client { get; set; } = null!;
    public static CommandsNextExtension Commands { get; set; } = null!;
    private static ILoggingService _logger = null!;

    static async Task Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddSingleton(new HttpClient())
            .AddSingleton<ILoggingService>(new LoggingService("logs/log.txt"))
            .BuildServiceProvider();

        _logger = services.GetRequiredService<ILoggingService>();

        _logger.LogInformation("PROGRAM: Starting bot...");

        var jsonReader = new JSONReader();
        await jsonReader.ReadJSON();
        _logger.LogInformation("PROGRAM: Configuration loaded.");

        var discordConfig = new DiscordConfiguration()
        {
            Intents = DiscordIntents.All,
            Token = jsonReader.token,
            TokenType = TokenType.Bot,
            AutoReconnect = true,
        };

        Client = new DiscordClient(discordConfig);

        Client.UseInteractivity(new InteractivityConfiguration()
        {
            Timeout = TimeSpan.FromMinutes(2)
        });

        Client.Ready += Client_Ready;

        var commandsConfig = new CommandsNextConfiguration()
        {
            StringPrefixes = new string[] { jsonReader.prefix },
            EnableMentionPrefix = true,
            EnableDms = true,
            EnableDefaultHelp = true
        };

        Commands = Client.UseCommandsNext(commandsConfig);
        Commands.CommandErrored += CommandEventHandler;

        _logger.LogInformation("PROGRAM: Registering commands...");

        var slashCommandsConfiguration = Client.UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = services
        });

        slashCommandsConfiguration.RegisterCommands<HelpCommand>();
        slashCommandsConfiguration.RegisterCommands<Calculator>();
        slashCommandsConfiguration.RegisterCommands<RollBotCommands>();
        _logger.LogInformation("PROGRAM: Commands registered.");

        await Client.ConnectAsync();
        _logger.LogInformation("PROGRAM: Bot connected.");

        await Task.Delay(-1);
    }

    private static async Task CommandEventHandler(CommandsNextExtension sender, CommandErrorEventArgs e)
    {
        _logger.LogError(e.Exception, $"PROGRAM: Command error: {e.Exception.Message}");

        if (e.Exception is ChecksFailedException exception)
        {
            string timeLeft = string.Empty;
            foreach (var check in exception.FailedChecks)
            {
                var Cooldown = (CooldownAttribute)check;
                timeLeft = Cooldown.GetRemainingCooldown(e.Context).ToString(@"hh\:mm\:ss");
            }

            var coolDownMessage = new DiscordEmbedBuilder
            {
                Title = "Cooldown",
                Description = $"You are on cooldown for {timeLeft}",
                Color = DiscordColor.Red
            };

            await e.Context.Channel.SendMessageAsync(embed: coolDownMessage);
        }
    }

    private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
    {
        _logger.LogInformation("PROGRAM: Bot is ready.");
        return Task.CompletedTask;
    }
}