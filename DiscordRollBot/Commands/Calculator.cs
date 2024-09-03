using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;


namespace RollBot.Commands;


[SlashCommandGroup("calculator", "A group of commands for basic arithmetic operations")]
public class Calculator : ApplicationCommandModule
{
    [SlashCommand("add", "Add two numbers")]
    public async Task Add(InteractionContext ctx, [Option("first", "The first number")] double first, [Option("second", "The second number")] double second)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

        var result = first + second;

        var embedMessage = new DiscordEmbedBuilder
        {
            Title = "Addition of two numbers",
            Description = $"{first} + {second} = {result}",
            Color = DiscordColor.Blue
        };

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
    }

    [SlashCommand("substract", "Substract two numbers")]
    public async Task Substract(InteractionContext ctx, [Option("first", "The first number")] double first, [Option("second", "The second number")] double second)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

        var result = first - second;

        var embedMessage = new DiscordEmbedBuilder
        {
            Title = "Substraction of two numbers",
            Description = $"{first} - {second} = {result}",
            Color = DiscordColor.Blue
        };

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
    }

    [SlashCommand("multiply", "Multiply two numbers")]
    public async Task Multiply(InteractionContext ctx, [Option("first", "The first number")] double first, [Option("second", "The second number")] double second)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

        var result = first * second;

        var embedMessage = new DiscordEmbedBuilder
        {
            Title = "Multiplication of two numbers",
            Description = $"{first} * {second} = {result}",
            Color = DiscordColor.Blue
        };

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
    }

    [SlashCommand("divide", "Divide two numbers")]
    public async Task Divide(InteractionContext ctx, [Option("first", "The first number")] double first, [Option("second", "The second number")] double second)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

        if (second == 0)
        {
            var embedMessage1 = new DiscordEmbedBuilder
            {
                Title = "Division of two numbers",
                Description = "Cannot divide by zero",
                Color = DiscordColor.Red
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage1));
            return;
        }

        var result = first / second;

        var embedMessage = new DiscordEmbedBuilder
        {
            Title = "Division of two numbers",
            Description = $"{first} / {second} = {result}",
            Color = DiscordColor.Blue
        };

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
    }
}
