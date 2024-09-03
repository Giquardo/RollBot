using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using RollBot.Services;
using Newtonsoft.Json.Linq;

namespace RollBot.Commands
{
    [SlashCommandGroup("rb", "A group of commands for our RollBot")]
    public class RollBotCommands : ApplicationCommandModule
    {
        private readonly HttpClient _httpClient;
        private readonly ILoggingService _loggingService;

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

        [SlashCommand("j", "Join the bot")]
        public async Task JoinAlias(InteractionContext ctx)
        {
            _loggingService.LogInformation("ROLLBOT COMMAND: Join alias command received");
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

        [SlashCommand("balance", "Get your current balance")]
        public async Task Balance(InteractionContext ctx)
        {
            _loggingService.LogInformation("ROLLBOT COMMAND: Balance command received");
            await ExecuteBalanceCommand(ctx);
        }

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
    }
}