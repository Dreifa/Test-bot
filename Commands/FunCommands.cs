using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBot.Commands
{
    public class FunCommands : BaseCommandModule
    {
        [Command("ping")]
        public async Task Ping(CommandContext ctx)
        {
           await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }
        [Command("clear")]
        public async Task Responce(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Чищу же ну");
            while (true)
            {
                var interactivity = ctx.Client.GetInteractivity();
                var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);
                if (message.Result.Content.StartsWith("-skip") || message.Result.Content.StartsWith("-leave") || message.Result.Content.StartsWith("-stop"))
                {
                    await ctx.Channel.DeleteMessageAsync(message.Result);
                }
                else
                {
                    if (!message.Result.Content.Contains("https"))
                    {
                        await ctx.Channel.DeleteMessageAsync(message.Result);
                        var nextMessage = await ctx.Channel.GetNextMessageAsync(ctx.Guild.GetMemberAsync(234395307759108106).Result, null); //DRY!!!!!!
                        await ctx.Channel.DeleteMessageAsync(nextMessage.Result);
                        var link = nextMessage.Result.Embeds.Any() ? nextMessage.Result.Embeds.FirstOrDefault().Description : null;
                        var startIndex = link.IndexOf("https");
                        var endIndex = link.LastIndexOf(") [");
                        link = link.Substring(startIndex, endIndex - startIndex);
                        await new DiscordMessageBuilder()
                                .WithContent(link)
                                .SendAsync(ctx.Guild.GetChannel(873976127352352898));
                    }
                    else
                    {
                        var startIndex = message.Result.Content.IndexOf("https");
                        string link = message.Result.Content.Substring(startIndex);
                        if (message.Result.Content.StartsWith("-p ") || message.Result.Content.StartsWith("-play "))
                        {
                            await ctx.Channel.DeleteMessageAsync(message.Result);
                            var nextMessage = await ctx.Channel.GetNextMessageAsync(ctx.Guild.GetMemberAsync(234395307759108106).Result, null);
                            await ctx.Channel.DeleteMessageAsync(nextMessage.Result);
                            await new DiscordMessageBuilder()
                                .WithContent(link)
                                .SendAsync(ctx.Guild.GetChannel(873976127352352898));
                        }

                    }
                }
            }
        }
    }
}
