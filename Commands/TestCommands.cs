using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;


namespace RollBot.Commands;

public class TestCommands : BaseCommandModule
{
    [Command("kys")]
    public async Task KillYourself(CommandContext ctx)
    {
        await ctx.Channel.SendMessageAsync($"no u {ctx.User.Mention}");
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
}