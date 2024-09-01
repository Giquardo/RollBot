using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RollBot.Models;
using RollBot.Models;


namespace RollBot.Commands;

public class TestCommands : BaseCommandModule
{
    [Command("kys")]
    public async Task KillYourself(CommandContext ctx, DiscordUser user)
    {
        await ctx.Channel.SendMessageAsync($"Kill yourself {user.Mention}");
    }

    [Command("add")]
    public async Task Add(CommandContext ctx, int a, int b)
    {
        int result = a + b;
        await ctx.Channel.SendMessageAsync($"{a} + {b} = {result}");
    }

    [Command("substract")]
    public async Task Substract(CommandContext ctx, int a, int b)
    {
        int result = a - b;
        await ctx.Channel.SendMessageAsync($"{a} - {b} = {result}");
    }

    [Command("embed")]
    public async Task EmbedMessage(CommandContext ctx)
    {
        var message = new DiscordEmbedBuilder
        {
            Title = "This is my first Discord Embed",
            Description = $"This commands was executed by {ctx.User.Username}",
            Color = DiscordColor.Blue
        };

        await ctx.Channel.SendMessageAsync(embed: message);
    }

    [Command("cardgame")]
    public async Task CardGame(CommandContext ctx)
    {
        var userCard = new CardSystem();

        var userCardEmbed = new DiscordEmbedBuilder
        {
            Title = $"Your Card is {userCard.SelectedCard}",
            Color = DiscordColor.Orange
        };

        await ctx.Channel.SendMessageAsync(embed: userCardEmbed);

        var botCard = new CardSystem();

        var botCardEmbed = new DiscordEmbedBuilder
        {
            Title = $"My Card is {botCard.SelectedCard}",
            Color = DiscordColor.Orange
        };

        await ctx.Channel.SendMessageAsync(embed: botCardEmbed);

        if (userCard.SelectedNumber > botCard.SelectedNumber)
        {
            var winMessage = new DiscordEmbedBuilder
            {
                Title = "Congratulations, You win!",
                Color = DiscordColor.Green
            };

            await ctx.Channel.SendMessageAsync(embed: winMessage);
        }
        else if (userCard.SelectedNumber < botCard.SelectedNumber)
        {
            var loseMessage = new DiscordEmbedBuilder
            {
                Title = "Sorry, You lose!",
                Color = DiscordColor.Red
            };

            await ctx.Channel.SendMessageAsync(embed: loseMessage);
        }
        else
        {
            var drawMessage = new DiscordEmbedBuilder
            {
                Title = "It's a draw!",
                Color = DiscordColor.Yellow
            };

            await ctx.Channel.SendMessageAsync(embed: drawMessage);
        }
    }
}