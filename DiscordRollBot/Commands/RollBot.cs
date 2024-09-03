using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using RollBot.Services;
using Newtonsoft.Json.Linq;
using System;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace RollBot.Commands
{
    [SlashCommandGroup("rb", "A group of commands for our RollBot")]
    public class RollBotCommands : ApplicationCommandModule
    {
        private readonly HttpClient _httpClient;
        private readonly ILoggingService _loggingService;
        private const int PacksPerPage = 7;

        public RollBotCommands(HttpClient httpClient, ILoggingService loggingService)
        {
            _httpClient = httpClient;
            _loggingService = loggingService;
        }

        [SlashCommand("join", "Join the bot")]
        public async Task Join(InteractionContext ctx)
        {
            _loggingService.LogInformation("ROLLBOT COMMAND: Join command received");
            await ExecuteJoinCommand(ctx);
        }

        private async Task ExecuteJoinCommand(InteractionContext ctx)
        {
            try
            {
                _loggingService.LogInformation("ROLLBOT COMMAND: Deferring response");
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var discordId = ctx.User.Id.ToString();
                _loggingService.LogInformation($"ROLLBOT COMMAND: Processing join command for user {discordId}");

                var getUserResponse = await _httpClient.GetAsync($"http://localhost:3000/api/users/{discordId}");
                _loggingService.LogInformation($"ROLLBOT COMMAND: GET /api/users/{discordId} responded with {getUserResponse.StatusCode}");

                if (getUserResponse.IsSuccessStatusCode)
                {
                    var embedMessage = new DiscordEmbedBuilder
                    {
                        Title = "Already Joined",
                        Description = "You have already joined the bot and can start collecting cards",
                        Color = DiscordColor.Yellow
                    };

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                    return;
                }

                var newUserDto = new { DiscordId = discordId };
                var jsonContent = JsonConvert.SerializeObject(newUserDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                _loggingService.LogInformation("ROLLBOT COMMAND: Sending POST request to create user");
                var createUserResponse = await _httpClient.PostAsync("http://localhost:3000/api/users", content);
                _loggingService.LogInformation($"ROLLBOT COMMAND: POST /api/users responded with {createUserResponse.StatusCode}");

                if (createUserResponse.IsSuccessStatusCode)
                {
                    var embedMessage = new DiscordEmbedBuilder
                    {
                        Title = "You have joined the bot",
                        Description = "You have joined the bot and can start collecting cards",
                        Color = DiscordColor.Green
                    };

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                }
                else
                {
                    var embedMessage = new DiscordEmbedBuilder
                    {
                        Title = "Error",
                        Description = "There was an error joining the bot. Please try again later.",
                        Color = DiscordColor.Red
                    };

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "ROLLBOT COMMAND: An error occurred while processing the join command");
                var embedMessage = new DiscordEmbedBuilder
                {
                    Title = "Error",
                    Description = "An unexpected error occurred. Please try again later.",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
            }
        }

        // [SlashCommand("balance", "Get your current balance")]
        // public async Task Balance(InteractionContext ctx)
        // {
        //     _loggingService.LogInformation("ROLLBOT COMMAND: Balance command received");
        //     await ExecuteBalanceCommand(ctx);
        // }

        [SlashCommand("bal", "Get your current balance")]
        public async Task BalanceAlias(InteractionContext ctx)
        {
            _loggingService.LogInformation("ROLLBOT COMMAND: Balance alias command received");
            await ExecuteBalanceCommand(ctx);
        }

        private async Task ExecuteBalanceCommand(InteractionContext ctx)
        {
            try
            {
                _loggingService.LogInformation("ROLLBOT COMMAND: Deferring response");
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var discordId = ctx.User.Id.ToString();
                _loggingService.LogInformation($"ROLLBOT COMMAND: Processing balance command for user {discordId}");

                var getUserResponse = await _httpClient.GetAsync($"http://localhost:3000/api/users/{discordId}");
                _loggingService.LogInformation($"ROLLBOT COMMAND: GET /api/users/{discordId} responded with {getUserResponse.StatusCode}");

                if (getUserResponse.IsSuccessStatusCode)
                {
                    var jsonResponse = await getUserResponse.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(jsonResponse);
                    var balance = jsonObject["balance"]?.Value<int>() ?? 0;

                    var embedMessage = new DiscordEmbedBuilder
                    {
                        Title = "Your Balance",
                        Description = $"Your current balance is {balance}",
                        Color = DiscordColor.Gold
                    };

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                }
                else
                {
                    var embedMessage = new DiscordEmbedBuilder
                    {
                        Title = "Error",
                        Description = "There was an error getting your balance. Please try again later.",
                        Color = DiscordColor.Red
                    };

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "ROLLBOT COMMAND: An error occurred while processing the balance command");
                var embedMessage = new DiscordEmbedBuilder
                {
                    Title = "Error",
                    Description = "An unexpected error occurred. Please try again later.",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
            }
        }

        [SlashCommand("packs", "Get your card packs")]
        public async Task Packs(InteractionContext ctx)
        {
            _loggingService.LogInformation("ROLLBOT COMMAND: Packs command received");
            await ExecutePacksCommand(ctx);
        }

        private async Task ExecutePacksCommand(InteractionContext ctx)
        {
            try
            {
                _loggingService.LogInformation("ROLLBOT COMMAND: Deferring response");
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var discordId = ctx.User.Id;
                _loggingService.LogInformation($"ROLLBOT COMMAND: Processing packs command for user {discordId}");

                var getPacksResponse = await _httpClient.GetAsync($"http://localhost:3000/api/CardPack/{discordId}");
                _loggingService.LogInformation($"ROLLBOT COMMAND: GET /api/CardPack/{discordId} responded with {getPacksResponse.StatusCode}");

                if (getPacksResponse.IsSuccessStatusCode)
                {
                    var jsonResponse = await getPacksResponse.Content.ReadAsStringAsync();
                    var cardPacks = JArray.Parse(jsonResponse);
                    _loggingService.LogInformation($"ROLLBOT COMMAND: Parsed card packs for user {discordId}");

                    if (cardPacks == null || !cardPacks.Any())
                    {
                        _loggingService.LogInformation("ROLLBOT COMMAND: No card packs found");
                        var embedMessage = new DiscordEmbedBuilder
                        {
                            Title = "No Card Packs",
                            Description = "You do not have any card packs. Buy some packs to get started!",
                            Color = DiscordColor.Blue
                        };

                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                    }
                    else
                    {
                        _loggingService.LogInformation($"ROLLBOT COMMAND: Displaying all card packs for user {discordId}");
                        var embedMessage = new DiscordEmbedBuilder
                        {
                            Title = "Your Card Packs",
                            Color = DiscordColor.Blue
                        };

                        var packsDescription = string.Join("\n", cardPacks.Select(cardPack =>
                        {
                            var id = cardPack["id"]?.ToString() ?? "Unknown";
                            var packType = cardPack["packType"]?.ToString() ?? "Unknown";
                            var rarity = cardPack["rarity"]?.ToString() ?? "Unknown";
                            var rarityEmoji = GetRarityEmoji(rarity);
                            return $"`{id}` - {rarityEmoji} - {packType} Pack ";
                        }));

                        embedMessage.Description = packsDescription;

                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                    }
                }
                else
                {
                    _loggingService.LogWarning($"ROLLBOT COMMAND: Failed to get card packs for user {discordId}");
                    var embedMessage = new DiscordEmbedBuilder
                    {
                        Title = "Error",
                        Description = "There was an error getting your card packs. Please try again later.",
                        Color = DiscordColor.Red
                    };

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "ROLLBOT COMMAND: An error occurred while processing the packs command");
                var embedMessage = new DiscordEmbedBuilder
                {
                    Title = "Error",
                    Description = "An unexpected error occurred. Please try again later.",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
            }
        }

        private string GetRarityEmoji(string rarity)
        {
            return rarity switch
            {
                "Common" => "⬜",
                "Uncommon" => "🟩",
                "Rare" => "🟦",
                "Epic" => "🟪",
                "Legendary" => "🟧",
                "mythic" => "🟥",
                _ => "⬛"
            };
        }

        // private async Task ShowCardPacksPage(InteractionContext ctx, JArray cardPacks, int page)
        // {
        //     _loggingService.LogInformation($"ROLLBOT COMMAND: Showing card packs page {page} for user {ctx.User.Id}");
        //     var embedMessage = new DiscordEmbedBuilder
        //     {
        //         Title = "Your Card Packs",
        //         Description = "Here are your card packs:",
        //         Color = DiscordColor.Blue
        //     };

        //     var start = page * PacksPerPage;
        //     var end = Math.Min(start + PacksPerPage, cardPacks.Count);

        //     for (var i = start; i < end; i++)
        //     {
        //         var cardPack = cardPacks[i];
        //         var packType = cardPack["packType"]?.ToString();
        //         var rarity = cardPack["rarity"]?.ToString();
        //         embedMessage.AddField($"Pack Type: {packType}", $"Rarity: {rarity}");
        //     }

        //     var builder = new DiscordWebhookBuilder().AddEmbed(embedMessage);

        //     var prevButton = new DiscordButtonComponent(ButtonStyle.Primary, $"prev_page_{page - 1}", "Previous", page == 0);
        //     var nextButton = new DiscordButtonComponent(ButtonStyle.Primary, $"next_page_{page + 1}", "Next", end >= cardPacks.Count);

        //     builder.AddComponents(prevButton, nextButton);

        //     await ctx.EditResponseAsync(builder);

        //     var response = await ctx.GetOriginalResponseAsync();
        //     var interactivity = ctx.Client.GetInteractivity();

        // while (true)
        // {
        //     _loggingService.LogInformation($"ROLLBOT COMMAND: Waiting for button interaction on page {page} for user {ctx.User.Id}");
        //     var buttonResult = await interactivity.WaitForButtonAsync(response, ctx.User, TimeSpan.FromMinutes(2));

        //     if (buttonResult.TimedOut)
        //     {
        //         _loggingService.LogInformation($"ROLLBOT COMMAND: Button interaction timed out on page {page} for user {ctx.User.Id}");
        //         await response.ModifyAsync(new DiscordMessageBuilder()
        //             .WithContent("Timed out.")
        //             .WithEmbed(embedMessage)
        //             .AddComponents(
        //                 new DiscordButtonComponent(ButtonStyle.Primary, $"prev_page_{page - 1}", "Previous", true),
        //                 new DiscordButtonComponent(ButtonStyle.Primary, $"next_page_{page + 1}", "Next", true)
        //             ));
        //         break;
        //     }

        //     try
        //     {
        //         switch (buttonResult.Result.Id)
        //         {
        //             case string id when id.StartsWith("prev_page_"):
        //                 var prevPage = int.Parse(id.Split('_').Last());
        //                 _loggingService.LogInformation($"ROLLBOT COMMAND: Previous page button clicked, navigating to page {prevPage} for user {ctx.User.Id}");
        //                 await ShowCardPacksPage(ctx, cardPacks, prevPage);
        //                 break;

        //             case string id when id.StartsWith("next_page_"):
        //                 var nextPage = int.Parse(id.Split('_').Last());
        //                 _loggingService.LogInformation($"ROLLBOT COMMAND: Next page button clicked, navigating to page {nextPage} for user {ctx.User.Id}");
        //                 await ShowCardPacksPage(ctx, cardPacks, nextPage);
        //                 break;
        //         }

        //         await buttonResult.Result.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage);
        //     }
        //     catch (Exception ex)
        //     {
        //         _loggingService.LogError(ex, "ROLLBOT COMMAND: An error occurred while handling the button interaction");
        //         await buttonResult.Result.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
        //             .WithContent("An error occurred while processing your request. Please try again later.")
        //             .AsEphemeral(true));
        //     }
        // }
        //}
    }
}