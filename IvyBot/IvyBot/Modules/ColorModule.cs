using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using IvyBot.Services;

namespace IvyBot.Modules {
    [Name ("Color Roles")]
    public class ColorModule : ModuleBase<SocketCommandContext> {
        private readonly List<ColorService.ColorDef> colors;
        public static Dictionary<string, ColorService.ColorDef> colorMap;

        public ColorModule () {
            colors = new List<ColorService.ColorDef> () {
                new ColorService.ColorDef ("Blue", Color.Blue),
                new ColorService.ColorDef ("Teal", Color.Teal),
                new ColorService.ColorDef ("Gold", Color.Gold),
                new ColorService.ColorDef ("Green", Color.Green),
                new ColorService.ColorDef ("Purple", Color.Purple),
                new ColorService.ColorDef ("Orange", Color.Orange),
                new ColorService.ColorDef ("Magenta", Color.Magenta),
                new ColorService.ColorDef ("Red", Color.Red),
                new ColorService.ColorDef ("DarkBlue", Color.DarkBlue),
                new ColorService.ColorDef ("DarkTeal", Color.DarkTeal),
                new ColorService.ColorDef ("DarkGreen", Color.DarkGreen),
                new ColorService.ColorDef ("DarkGrey", Color.DarkGrey),
                new ColorService.ColorDef ("DarkMagenta", Color.DarkMagenta),
                new ColorService.ColorDef ("DarkOrange", Color.DarkOrange),
                new ColorService.ColorDef ("DarkPurple", Color.DarkPurple),
                new ColorService.ColorDef ("DarkRed", Color.DarkRed),
                new ColorService.ColorDef ("DarkerGrey", Color.DarkerGrey),
                new ColorService.ColorDef ("LightGrey", Color.LightGrey),
                new ColorService.ColorDef ("LightOrange", Color.LightOrange),
                new ColorService.ColorDef ("LighterGrey", Color.LighterGrey),
                new ColorService.ColorDef ("Default", Color.Default),
            };

            colorMap = colors.ToDictionary (c => c.Id);
        }

        [Command ("colorlist")]
        [Summary ("Gives a list of all available color roles")]
        public async Task ColorListAsync () {
            string list = $"**Colors**\n" + string.Join ("\n", colors.Select (c => '`' + c.Name + '`'));

            await ReplyAsync (list);
        }

        [RequireBotPermission (GuildPermission.ManageRoles)]
        [Command ("colorme")]
        [Summary ("Gives you a color role of your choice")]
        public async Task ColorMeAsync ([Remainder] string colorName) {
            await ReplyAsync (ColorService.SetColorAsync ((Context.User as SocketGuildUser), colorName).Result);
        }

        [RequireBotPermission (GuildPermission.ManageRoles)]
        [Command ("colorclear")]
        [Summary ("Removes all your current color roles")]
        public async Task ColorClearAsync () {
            var colorRoles = (Context.User as SocketGuildUser).Roles.Where (r => r.Name.EqualsAny (colors.Select (c => c.Name)));

            await (Context.User as SocketGuildUser).RemoveRolesAsync (colorRoles);

            var check = new Emoji ("✅");
            await Context.Message.AddReactionAsync (check);
        }

        [RequireUserPermission (GuildPermission.ManageRoles)]
        [RequireBotPermission (GuildPermission.ManageRoles)]
        [Command ("colorset")]
        [Summary ("Gives the specified user a color of your choice")]
        public async Task ColorSetAsync (IGuildUser user, [Remainder] string colorName) {
            await ReplyAsync (ColorService.SetColorAsync ((user as SocketGuildUser), colorName).Result);
        }
    }
}