using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.VisualBasic;
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
        var botCard = new CardSystem();

        if (userCard.SelectedNumber > botCard.SelectedNumber)
        {
            var winMessage = new DiscordEmbedBuilder
            {
                Title = "Congratulations, You win!",
                Description = $"Your card is {userCard.SelectedCard} and my card is {botCard.SelectedCard}",
                Color = DiscordColor.Green
            };

            await ctx.Channel.SendMessageAsync(embed: winMessage);
        }
        else if (userCard.SelectedNumber < botCard.SelectedNumber)
        {
            var loseMessage = new DiscordEmbedBuilder
            {
                Title = "Sorry, You lose!",
                Description = $"Your card is {userCard.SelectedCard} and my card is {botCard.SelectedCard}",
                Color = DiscordColor.Red
            };

            await ctx.Channel.SendMessageAsync(embed: loseMessage);
        }
        else
        {
            var drawMessage = new DiscordEmbedBuilder
            {
                Title = "It's a draw!",
                Description = $"Your card is {userCard.SelectedCard} and my card is {botCard.SelectedCard}",
                Color = DiscordColor.Yellow
            };

            await ctx.Channel.SendMessageAsync(embed: drawMessage);
        }
    }

    [Command("hello")]
    public async Task Hello(CommandContext ctx)
    {
        var interactivity = Program.Client.GetInteractivity();

        var messageToRetrieve = await interactivity.WaitForMessageAsync(message => message.Content.ToLower() == "hello");

        if (messageToRetrieve.Result.Content == "hello")
        {
            await ctx.Channel.SendMessageAsync($"{ctx.User.Username} said Hello!");
        }
    }

    [Command("react")]
    public async Task React(CommandContext ctx)
    {
        var interactivity = Program.Client.GetInteractivity();

        var messageToReact = await interactivity.WaitForReactionAsync(message => message.Message.Id == 1279761914695520306);

        if (messageToReact.Result.Message.Id == 1279761914695520306)
        {
            await ctx.Channel.SendMessageAsync($"{ctx.User.Username} used the emoji {messageToReact.Result.Emoji.Name}");
        }
    }

    [Command("poll")]
    public async Task Poll(CommandContext ctx, [RemainingText] string question)
    {
        var interactivity = Program.Client.GetInteractivity();
        var pollTime = TimeSpan.FromSeconds(10);

        DiscordEmoji[] emojis = { DiscordEmoji.FromName(Program.Client, ":thumbsup:"), DiscordEmoji.FromName(Program.Client, ":thumbsdown:") };

        var pollMessage = new DiscordEmbedBuilder
        {
            Title = question,
            Description = "React with 👍 or 👎",
            Color = DiscordColor.Blue
        };

        var sentPoll = await ctx.Channel.SendMessageAsync(embed: pollMessage);
        foreach (var emoji in emojis)
        {
            await sentPoll.CreateReactionAsync(emoji);
        }


        var totalReactions = await interactivity.CollectReactionsAsync(sentPoll, pollTime);

        int thumbsUp = 0;
        int thumbsDown = 0;

        foreach(var emoji in totalReactions)
        {
            if (emoji.Emoji == emojis[0])
            {
                thumbsUp++;
            }
            else if (emoji.Emoji == emojis[1])
            {
                thumbsDown++;
            }
        }

        int totalVotes = thumbsUp + thumbsDown;
        string pollResults = $"👍: {thumbsUp} ({(thumbsUp / totalVotes) * 100}%) 👎: {thumbsDown} ({(thumbsDown / totalVotes) * 100}%)";

        var resultsMessage = new DiscordEmbedBuilder
        {
            Title = "Poll Results",
            Description = pollResults,
            Color = DiscordColor.Blue
        };

        await ctx.Channel.SendMessageAsync(embed: resultsMessage);
    }
}