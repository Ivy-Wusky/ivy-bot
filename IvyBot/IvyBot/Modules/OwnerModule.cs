using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using IvyBot.Configuration;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace IvyBot.Modules {
    [Name ("Owner")]
    public class OwnerModule : ModuleBase<SocketCommandContext> {
        public class ScriptGlobals {
            public IvyBotClient client { get; set; }
        }

#pragma warning disable CS0649
        private static readonly IvyBotClient _client;
#pragma warning restore CS0649

        [RequireOwner]
        [Command ("eval")]
        [Summary ("Runs a C# snippet and sends the result")]
        public async Task ExecuteAsync ([Remainder] string code) {
            IEnumerable<string> refs = new List<string> () { "System", "System.Diagnostics", "System.Collections.Generic", "System.Linq", "System.Net", "System.Net.Http", "System.IO", "System.Threading.Tasks", "System.Xml", "Newtonsoft.Json" };

            var globals = new ScriptGlobals { client = _client };

            var options = ScriptOptions.Default
                .AddReferences (refs)
                .AddImports (refs);

            var text = code.Trim ('`');

            try {
                var script = CSharpScript.Create (text, options, typeof (ScriptGlobals));
                var scriptState = await script.RunAsync (globals);
                var returnValue = scriptState.ReturnValue;

                if (returnValue != null) {
                    await ReplyAsync ($"```cs\n{returnValue.ToString()}\n```");
                } else {
                    await ReplyAsync ("The script's return value output as null");
                }
            } catch (Exception ex) {
                await ReplyAsync ($"```cs\n{ex.Message}\n```");
            }
        }

        [RequireOwner]
        [Command ("changeprefix")]
        [Summary ("Modifies the prefix of the bot in all servers")]
        public async Task UpdatePrefixAsync ([Remainder] string prefix) {
            ConfigurationManager configManager = new ConfigurationManager ();
            configManager.SetValueFor (configManager.GetValueFor (Constants.BotPrefix), prefix);

            var check = new Emoji ("✅");
            await Context.Message.AddReactionAsync (check);
        }
    }
}