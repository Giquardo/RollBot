using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollBot.Commands;

public class HelpCommand : ApplicationCommandModule
{
    [SlashCommand("Help", "This command allows you to view all the commands")]
    public async Task Help(InteractionContext ctx)
    {
        try
        {
            // Defer the response to acknowledge the interaction
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            var calculatorButton = new DiscordButtonComponent(ButtonStyle.Success, "calculatorButton", "Calculator");

            var embed = new DiscordEmbedBuilder
            {
                Title = "Help",
                Description = "Select a category to view the commands",
                Color = DiscordColor.Black
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(embed)
                .AddComponents(calculatorButton));

            var response = await ctx.GetOriginalResponseAsync();

            var interactivity = ctx.Client.GetInteractivity();

            while (true)
            {
                var buttonResult = await interactivity.WaitForButtonAsync(response, ctx.User, TimeSpan.FromMinutes(2));

                if (buttonResult.TimedOut)
                {
                    await response.ModifyAsync(new DiscordMessageBuilder()
                        .WithContent("Timed out.")
                        .WithEmbed(embed)
                        .AddComponents(calculatorButton.Disable()));
                    break;
                }

                switch (buttonResult.Result.Id)
                {
                    case "calculatorButton":
                        var calculatorEmbed = new DiscordEmbedBuilder
                        {
                            Title = "Calculator Commands",
                            Description = "These are the calculator commands\n" + "1. Add (number 1 + number 2)\n" + "2. Substract\n" + "3. Multiply\n" + "4. Divide",
                            Color = DiscordColor.Black
                        };

                        await response.ModifyAsync(new DiscordMessageBuilder()
                            .WithEmbed(calculatorEmbed)
                            .AddComponents(calculatorButton.Disable()));
                        break;
                }
                await buttonResult.Result.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage);
            }

        }
        catch (Exception ex)
        {
            // Log the exception (you can replace this with your logging mechanism)
            Console.WriteLine($"An error occurred: {ex.Message}");

            // Send an error message to the user
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("An error occurred while processing your request"));
        }
    }
}