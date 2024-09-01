using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Rollbot.Config;
using RollBot.Commands;

namespace RollBot;

public sealed class Program
{
    public static DiscordClient Client { get; set; }
    public static CommandsNextExtension Commands { get; set; }
    static async Task Main(string[] args)
    {
        var jsonReader = new JSONReader();
        await jsonReader.ReadJSON();

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
        var slashCommandsConfiguration = Client.UseSlashCommands();

        Commands.CommandErrored += CommandEventHandler;


        slashCommandsConfiguration.RegisterCommands<HelpCommand>();
        slashCommandsConfiguration.RegisterCommands<Calculator>();

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }

    private static async Task CommandEventHandler(CommandsNextExtension sender, CommandErrorEventArgs e)
    {
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
        return Task.CompletedTask;
    }
}